using System.Collections;
using Reless.MR;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Reless
{
    /// <summary>
    /// 챕터가 끝나고 꿈에서 나가기 위한 패스스루 문 동작을 담당하는 클래스입니다.
    /// </summary>
    public class ExitDreamBehaviour : MonoBehaviour
    {
        private GameManager _gameManager;
        
        private RoomManager _roomManager;
    
        [SerializeField]
        private OVRPassthroughLayer roomPassthrough;
    
        private float _initialPlayerToDoorDistance;
        
        private float _playerExitTimer;
        
        public float exitThresholdTime;
        
        private bool _isExiting;

        private void Awake()
        {
            if (RoomManager.Instance is RoomManager roomManager)
            {
                _roomManager = roomManager;
            }
            else
            {
                this.enabled = false;
                RoomManager.OnMRUKSceneLoaded += () =>
                {
                    _roomManager = RoomManager.Instance;
                    this.enabled = true;
                };
            }
        }

        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            
            // 시작 시점의 플레이어 위치와 가장 가까운 문 위치 사이의 거리를 저장합니다.
            _initialPlayerToDoorDistance = _roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out _);
            Logger.Log($"Initial Player To Door Distance : {_initialPlayerToDoorDistance}");

            // VR Room 씬에서는 패스스루가 기존에 비활성화되어 있을 것으로 기대되므로 패스스루를 활성화합니다.
            FindObjectOfType<OVRManager>().isInsightPassthroughEnabled = true;
            
            _roomManager.HidePassthroughEffectMesh = true;
            
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(SceneManager.GetScene(BuildScene.ExitDream));
            
            var sppPassthroughParent = new GameObject("SppPassthroughMeshes");
            
            // 패스스루 이펙트 메쉬를 룸 패스스루 레이어에 추가합니다.
            foreach (var sppPassthroughMesh in _roomManager.SppPassThroughMeshes)
            {
                var cloned = Instantiate(sppPassthroughMesh, sppPassthroughParent.transform, worldPositionStays: true);
                //cloned.AddComponent<MeshRenderer>().material = temp_material;
                roomPassthrough.AddSurfaceGeometry(cloned);

                /*Debug.Log($"Add Passthrough Mesh of {sppPassthroughMesh.transform.parent.name}");
                roomPassthrough.AddSurfaceGeometry(sppPassthroughMesh);*/
            }
            var centerEyeAnchor = FindObjectOfType<OVRCameraRig>().centerEyeAnchor;

            sppPassthroughParent.transform.position =
                centerEyeAnchor.TransformPoint(Vector3.zero) - centerEyeAnchor.localPosition;

            UnitySceneManager.SetActiveScene(SceneManager.GetScene(BuildScene.VRRoom));
            //clonedRoom.transform.position = FindObjectOfType<OVRCameraRig>().centerEyeAnchor.TransformPoint(Vector3.zero);
        }

        private void Update()
        {
            var distance = _roomManager.ClosestDoorDistance(GameManager.EyeAnchor.localPosition, out var doorPosition);
            
            if (distance != 0)
            {
                // 플레이어가 문과 가까운 정도를 계산합니다.
                float nearness = 1f - Mathf.Clamp01(distance / _initialPlayerToDoorDistance);
                
                // Debug.Log($"nearness : {nearness}");
                
                // 플레이어가 문에 가까워질수록 패스스루의 불투명도가 올라갑니다.
                roomPassthrough.textureOpacity = nearness;
            }

            // 플레이어가 문 밖으로 나갔나요?
            if (_roomManager.Room.IsPositionInRoom(GameManager.EyeAnchor.localPosition, testVerticalBounds: false) is false)
            {
                // 나가 있는 시간을 측정합니다.
                _playerExitTimer += Time.deltaTime;
            }
            else
            {
                // 플레이어가 다시 방 안으로 들어왔다면 0으로 초기화합니다.
                _playerExitTimer = 0f;
            }

            // 임계 시간보다 더 오래 나가 있었다면
            if (_playerExitTimer >= exitThresholdTime && !_isExiting)
            {
                // 꿈에서 나가기를 시작합니다.
                StartCoroutine(ExitingDream());
                
                // 중복 실행 방지
                _isExiting = true;
            }
        }

        private void OnDestroy()
        {
            _roomManager.HidePassthroughEffectMesh = false;
        }

        private IEnumerator ExitingDream()
        {
            Logger.Log("Exit Dream");
            
            var asyncLoad = GameManager.LoadMainScene();
            
            // 꿈에서 나갈 때 게임 단계를 다음 단계로 바꿉니다.
            GameManager.CurrentPhase++;
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
