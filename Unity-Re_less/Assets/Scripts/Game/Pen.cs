using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Reless.Game
{
    /// <summary>
    /// 그리기 게임 요소에 사용되는 펜
    /// </summary>
    public class Pen : MonoBehaviour
    {
        /// <summary>
        /// 그리는 선의 프리팹
        /// </summary>
        [SerializeField] 
        private GameObject lineSegmentPrefab;
        
        /// <summary>
        /// 현재 그리고 있는 선
        /// </summary>
        private LineRenderer _currentLine;

        /// <summary>
        /// 그리기 버튼이 눌리고 있는지 여부
        /// </summary>
        private bool _isPressed;

        /// <summary>
        /// 현재 LineRenderer에서 더 길어지지 않도록 제한합니다.
        /// </summary>
        private bool _limitLine;

        /// <summary>
        /// 한 LineRenderer에 있는 positions의 최대 개수
        /// </summary>
        [SerializeField] 
        private int maxLinePositions = 20;

        private readonly float _minInkDist = 0.01f;
        
        private readonly List<Vector3> _pointPositions = new();
        
        private readonly List<MeshCollider> _drawingMeshColliders = new();

        [SerializeField] 
        private AudioSource audioSource;

        private void Update()
        {
            // 그려질 위치에 쓸 오른손 컨트롤러 위치를 가져옵니다.
            var controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            // 그리기 버튼이 눌리고 있나요?
            if (_isPressed)
            {
                // 선이 너무 길어졌나요?
                if (_limitLine)
                {
                    CompleteLine();

                    // 마지막 포인트부터 다시 이어서 그리기 시작합니다.
                    StartDraw(_pointPositions.Last());

                    _limitLine = false;
                    return;
                }

                // 지금 그려지고 있는 선을 이어서 그립니다.
                UpdateLine(controllerPosition);

                // 플레이거가 그리기 버튼을 뗐습니다.
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {
                    CompleteLine();

                    // 새로운 선을 그리기 위해 초기화합니다.
                    _isPressed = false;

                    // 오디오 정지
                    audioSource.Stop();
                }
            }
            // 그리기 버튼이 눌리지 않았나요?
            else
            {
                // 그리기 버튼이 눌렸습니다. 
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {
                    // 그리기를 시작합니다.
                    StartDraw(controllerPosition);

                    // 오디오 재생
                    audioSource.Play();

                    _isPressed = true;
                }

                // 그리기 버튼이 눌리지 않고 있고 왼쪽 컨트롤러 X버튼을 누르면 선을 지웁니다.
                if (OVRInput.GetDown(OVRInput.Button.Three))
                {
                    ClearLines();
                }
            }
        }

        
        /// <summary>
        /// 새로운 선을 그리기 시작합니다.
        /// </summary>
        /// <param name="position">선의 시작 위치</param>
        private void StartDraw(Vector3 position)
        {
            GameObject newLine = Instantiate(lineSegmentPrefab, position, Quaternion.identity);
            _currentLine = newLine.GetComponent<LineRenderer>();
            _currentLine.positionCount = 1;
            _currentLine.SetPosition(0, position);
            _pointPositions.Clear();
            _pointPositions.Add(position);
            newLine.transform.parent = this.transform;
        }

        private void UpdateLine(Vector3 position)
        {
            if (_currentLine == null) return;

            float segmentLength = (position - _pointPositions.Last()).magnitude;
            if (segmentLength >= _minInkDist)
            {
                _pointPositions.Add(position);
                _currentLine.positionCount = _pointPositions.Count;
                _currentLine.SetPositions(_pointPositions.ToArray());
            }

            // 선이 너무 길어졌나요?
            if (_currentLine.positionCount >= maxLinePositions)
            {
                _limitLine = true;
            }
        }

        private void CompleteLine()
        {
            // 지금까지 만들어진 한 선으로부터 메시 콜라이더를 만들어 목록에 추가합니다.
            var mesh = new Mesh();
            _currentLine.BakeMesh(mesh);
            var drawingRigidbody = _currentLine.AddComponent<Rigidbody>();
            drawingRigidbody.isKinematic = true;
            var drawingMeshCollider = _currentLine.AddComponent<MeshCollider>();
            drawingMeshCollider.convex = true;
            drawingMeshCollider.sharedMesh = mesh; // 라인렌더러에서 베이크한 메시를 할당
            drawingMeshCollider.transform.position = Vector3.zero; // 이렇게 해야 콜라이더가 선 위에 제대로 자리합니다.
            _drawingMeshColliders.Add(drawingMeshCollider);
        }

        private void ClearLines()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        public bool IsDrawingMeshCollider(MeshCollider meshCollider) => _drawingMeshColliders.Contains(meshCollider);
    }
}