using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Logger = Reless.Debug.Logger;

namespace Reless.Game
{
    /// <summary>
    /// 그려야 할 오브젝트의 밑그림에 부착되는 컴포넌트
    /// </summary>
    public class SketchOutline : MonoBehaviour
    {
        /// <summary>
        /// 밑그림이 속한 챕터
        /// </summary>
        [SerializeField] 
        public Chapter chapter;
        
        /// <summary>
        ///  edge를 따라 생성될 콜라이더의 두께
        /// </summary>
        private const float ColliderThickness = 0.01f;

        private readonly List<DrawingChecker> _drawingCheckers = new();
        
        private float _minEdgeLength = 0.05f;
        private float _maxEdgeLength = 0.5f;
        private float _edgeInset = 0.01f;
        
        public event Action<SketchOutline> DrawingCompleted;
        
        [ShowNativeProperty]
        public bool IsCompleted => _isCompleted;
        
        private bool _isCompleted;
        
        [ShowNativeProperty]
        public float FillRatio => _fillRatio;
        private float _fillRatio;    
        
        private void Start()
        {
            GenerateEdgeColliders();
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            }
            
            // 모든 DrawingChecker가 체크되었는지 확인합니다.
            if (_drawingCheckers.Count > 0)
            {
                _fillRatio = (float)_drawingCheckers.Count(checker => checker.Checked) / _drawingCheckers.Count;

                // 임시: 100%를 충족하는게 너무 어려울 것으로 기대되므로 80% 이상이면 완성으로 간주합니다.
                if (_fillRatio > 0.8)
                {
                    _isCompleted = true;
                    OnDrawingComplete();
                }
                
                if (_drawingCheckers.TrueForAll(checker => checker.Checked))
                {
                    // 플레이어가 그린 선이 모든 DrawingChecker에 닿았습니다 - 그림이 완성되었습니다.
                    Logger.Log($"Drawing {this.gameObject.name} is completed!");
                    _isCompleted = true;
                    OnDrawingComplete();
                }
            }
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void OnDrawingComplete()
        {
            DrawingCompleted?.Invoke(this);
        }

        private void GenerateEdgeColliders()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            // 모든 엣지의 키를 저장할 딕셔너리
            var edgeKeys = new Dictionary<(int, int), bool>();
            
            // 삼각형에서 각 edge의 시작점과 끝점을 계산합니다.
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int index1 = triangles[i];
                int index2 = triangles[i + 1];
                int index3 = triangles[i + 2];

                // 버텍스 인덱스 쌍을 만들고 정렬하여 키로 사용
                AddEdgeKey(index1, index2);
                AddEdgeKey(index2, index3);
                AddEdgeKey(index3, index1);
                continue;

                void AddEdgeKey(int indexA, int indexB)
                {
                    int minIndex = Math.Min(indexA, indexB);
                    int maxIndex = Math.Max(indexA, indexB);
                    var edgeKey = (minIndex, maxIndex);

                    // 추가되어있지 않다면 추가합니다.
                    edgeKeys.TryAdd(edgeKey, true);
                }
            }

            // 중복되지 않은 고유한 엣지에서 콜라이더를 생성합니다.
            foreach (var (index1, index2) in edgeKeys.Keys)
            {
                Vector3 vertex1 = transform.TransformPoint(vertices[index1]);
                Vector3 vertex2 = transform.TransformPoint(vertices[index2]);

                GenerateEdgeColliders(vertex1, vertex2);
            }
        }

        private void GenerateEdgeColliders(Vector3 startPoint, Vector3 endPoint)
        {
            float distance = Vector3.Distance(startPoint, endPoint);
            
            // edge가 너무 짧으면 생성하지 않습니다.
            if (!(distance > _minEdgeLength)) return;
            
            // edge가 너무 긴가요?
            if (distance > _maxEdgeLength)
            {
                // edge를 최대 길이보다 작도록 여러 개로 나눕니다.
                var edges = SplitEdge(startPoint, endPoint, _maxEdgeLength);
                    
                // 나눈 edge들에 콜라이더를 생성합니다.
                foreach (var edge in edges)
                {
                    CreateEdgeCollider(edge);
                }
            }
            else
            {
                // edge에 콜라이더를 생성합니다.
                CreateEdgeCollider(startPoint, endPoint);
            }
        }

        private void CreateEdgeCollider((Vector3, Vector3) edge) => CreateEdgeCollider(edge.Item1, edge.Item2);
        
        private void CreateEdgeCollider(Vector3 startPoint, Vector3 endPoint)
        {
            // 콜라이더를 담을 오브젝트를 만들고 DrawingChecker 컴포넌트를 추가합니다.
            GameObject colliderObject = new GameObject("EdgeCollider");
            var rigidbody = colliderObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            var drawingChecker = colliderObject.AddComponent<DrawingChecker>();
            drawingChecker.Pen = FindObjectOfType<Pen>();
            _drawingCheckers.Add(drawingChecker);

            // 만든 오브젝트를 자식으로 만들고 위치를 조정합니다.
            colliderObject.transform.parent = transform;
            colliderObject.transform.position = (startPoint + endPoint) * 0.5f;

            // edge에 박스 콜라이더를 추가합니다.
            Vector3 edgeDirection = endPoint - startPoint;
            float edgeLength = edgeDirection.magnitude - _edgeInset - ColliderThickness;

            if (!(edgeLength > 0)) return;
            
            BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();
            edgeDirection.Normalize();
            boxCollider.size = new Vector3(edgeLength, ColliderThickness / transform.lossyScale.x, ColliderThickness / transform.lossyScale.z);
            boxCollider.isTrigger = true;

            // edge 방향에 따라 박스 콜라이더를 회전시킵니다.
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, edgeDirection);
            boxCollider.transform.rotation = rotation;
        }

        private static IEnumerable<(Vector3, Vector3)> SplitEdge(Vector3 start, Vector3 end, float maxEdgeDistance)
        {
            var edges = new List<(Vector3, Vector3)>();
            Vector3 direction = (end - start).normalized;
            float distance = (end - start).magnitude;
            int numSegments = Mathf.CeilToInt(distance / maxEdgeDistance);

            Vector3 currentPoint = start;
            for (int i = 0; i < numSegments; i++)
            {
                Vector3 nextPoint = start + direction * Mathf.Min(distance, (i + 1) * maxEdgeDistance);
                edges.Add((currentPoint, nextPoint));
                currentPoint = nextPoint;
            }

            return edges;
        }
    }
}


