using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Logger = Reless.Debug.Logger;
using SceneManager = Reless.Util.SceneManager;

namespace Reless.MR
{
    /// <summary>
    /// MRUK를 통한 현실 방 관리
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        /// <summary>
        /// RoomManager의 싱글톤 인스턴스를 반환합니다.
        /// 아직 룸 로드가 되지 않았다면 null을 반환합니다.
        /// </summary>
        [CanBeNull]
        public static RoomManager Instance 
        {
            get
            {
                // 인스턴스가 없으면 방을 셋업합니다.
                if (instance.IsUnityNull()) SetupRoom();
                
                return instance.AsUnityNull();
            }
        }
        private static RoomManager instance;
        
        /// <summary>
        /// RoomSetup 씬을 아직 로드한 적 없으면 true
        /// </summary>
        private static bool yetLoadScene = true;

        /// <summary>
        /// MRUK 씬이 로드되었을 때 호출될 액션.
        /// RoomManager의 인스턴스가 null인 경우 작업을 연기하는 데 사용됩니다.
        /// </summary>
        public static event Action OnMRUKSceneLoaded;
        
        /// <summary>
        /// 현재 방의 MRUKRoom 레퍼런스
        /// </summary>
        [NonSerialized]
        public MRUKRoom Room;
        
        /// <summary>
        /// VR Room의 모델과 텍스처를 재현하는 데 사용되는 이펙트 메시들
        /// </summary>
        [SerializeField]
        private List<EffectMesh> virtualRoomEffectMeshes;
        
        /// <summary>
        /// 패스스루 이펙트 메시
        /// </summary>
        [SerializeField]
        private EffectMesh passthroughRoom;
        
        /// <summary>
        /// Surface Projected PassThrough 이펙트 메시
        /// </summary>
        [SerializeField]
        private EffectMesh sppPassThroughRoom;
        
        /// <summary>
        /// 방의 엣지 이펙트 메시
        /// </summary>
        [SerializeField]
        private EffectMesh edgeEffect;
        
        public ReadOnlyCollection<GameObject> PassthroughEffectMeshes => _passthroughEffectMeshes.AsReadOnly();
        private readonly List<GameObject> _passthroughEffectMeshes = new();

        public ReadOnlyCollection<GameObject> SppPassThroughMeshes => _sppPassThroughMeshes.AsReadOnly();
        private readonly List<GameObject> _sppPassThroughMeshes = new();
        
        /// <summary>
        /// MRUK의 keyWall을 반환합니다.
        /// </summary>
        public MRUKAnchor KeyWall => _keyWall;
        private MRUKAnchor _keyWall;
        
        private Vector3? _uniqueDoorPosition;
        
        /// <summary>
        /// 방에 문이 하나라면 해당 문을 반환합니다.
        /// 문이 하나가 아니면 null입니다.
        /// </summary>
        [CanBeNull] 
        public MRUKAnchor UniqueDoor => _doors?.SingleOrDefault();

        /// <summary>
        /// 방의 문 앵커들
        /// </summary>
        private ICollection<MRUKAnchor> _doors;

        /// <summary>
        /// RoomEnlarger 레퍼런스
        /// </summary>
        [ReadOnly]
        public RoomEnlarger roomEnlarger;

        /// <summary>
        /// 오프닝에 쓰일 열리는 벽 오브젝트를 반환합니다.
        /// </summary>
        public GameObject OpeningWall => FindCreatedEffectMesh(_keyWall, passthroughRoom);
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // RoomSetup 씬에서 시작했다면 이미 현재 씬이므로 yetLoadScene을 false로 설정합니다.
            if (SceneManager.ActiveScene is BuildScene.RoomSetup) { yetLoadScene = false; }
        }
        
        /// <summary>
        /// RoomSetup 씬을 로드하여 방을 셋업하도록 합니다.
        /// </summary>
        public static void SetupRoom()
        {
            // 기존에 로드 호출이 없었던 경우에만 호출하여 중복 로드를 방지합니다.
            if (yetLoadScene)
            {
                SceneManager.LoadAsync(BuildScene.RoomSetup, LoadSceneMode.Additive);
                yetLoadScene = false;
            }
        }
        
        private void Awake()
        {
            // Awake 시점에 인스턴스는 아직 대입되지 않았거나(룸 로드가 끝나지 않음) 하나여야 합니다.
            Assert.IsTrue(instance is null || instance == this, "there are multiple RoomManager instances.");
        }

        /// <summary>
        /// MRUK 씬이 로드되었을 때 호출됩니다.
        /// </summary>
        public void OnSceneLoaded()
        {
            Logger.Log($"{nameof(RoomManager)}: MRUK Scene Loaded. Setting up Room.");
            Assert.IsNull(instance, "RoomManager is singleton but already exists.");
            
            DebugRoomInfo();
            
            // MRUK 씬이 로드될 때 RoomManager의 싱글톤 인스턴스를 대입합니다.
            instance = this;
            
            // RoomManager와 MRUK를 씬이 바뀌어도 파괴되지 않도록 설정합니다.
            DontDestroyOnLoad(instance.gameObject);
            DontDestroyOnLoad(MRUK.Instance);
            
            // 생성된 방을 자식으로 설정합니다.
            Room = MRUK.Instance.GetCurrentRoom();
            Room.transform.parent = this.transform;
            Room.transform.hasChanged = false;
            
            // 키 월을 찾습니다.
            _keyWall = Room.GetKeyWall(out _);
            
            // 방의 문 앵커들을 찾습니다.
            _doors ??= Room.Anchors
                .Where(anchor => anchor.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.DOOR_FRAME)
                .AsReadOnlyCollection();
            
            // 이펙트 메시들을 생성합니다.
            edgeEffect.CreateMesh();
            passthroughRoom.CreateMesh();
            sppPassThroughRoom.CreateMesh();

            // 생성된 SPP 패스스루 메시들을 찾아 리스트에 저장합니다.
            FindAndAddSppPassThroughMesh(Room.CeilingAnchor);
            FindAndAddSppPassThroughMesh(Room.FloorAnchor);
            Room.WallAnchors.ForEach(FindAndAddSppPassThroughMesh);
            
            void FindAndAddSppPassThroughMesh(MRUKAnchor anchor)
            {
                for (int i = 0; i < anchor.transform.childCount; i++)
                {
                    var childMesh = anchor.transform.GetChild(i);
                    
                    // MeshRenderer가 없는 경우 SPP 패스스루 메쉬로 간주합니다. (할당된 머티리얼 없으면 EffectMesh가 렌더러를 생성하지 않는듯함)
                    // 향후 더 나은 방법으로 수정 필요.
                    if (childMesh.GetComponent<MeshRenderer>() == null)
                    {
                        Logger.Log($"SPP PassThrough Mesh Found. Adding to list.");
                        _sppPassThroughMeshes.Add(anchor.transform.GetChild(i).gameObject);
                    }
                }
            }
            
            // 패스스루 이펙트 메쉬들을 약간 오프셋합니다.
            OffsetPassthroughEffectMeshes();
            
            OnMRUKSceneLoaded?.Invoke();
        }

        [Button]
        public void CreateVirtualRoomEffectMeshes()
        {
            foreach (var effectMesh in virtualRoomEffectMeshes)
            {
                effectMesh.CreateMesh();
            }
        }
        
        public void DestroyVirtualRoomEffectMeshes()
        {
            foreach (var effectMesh in virtualRoomEffectMeshes)
            {
                effectMesh.DestroyMesh();
            }
        }

        /// <summary>
        /// 현실 룸 전체를 활성화 또는 비활성화합니다.
        /// </summary>
        public bool RoomObjectActive { set => gameObject.SetActive(value); }
            
        [Conditional("UNITY_EDITOR")]
        private void DebugRoomInfo()
        {
            var rooms = MRUK.Instance.Rooms;
            Logger.Log($"{nameof(RoomManager)}: Room count: {rooms.Count}");
            foreach (var room in rooms)
            {
                Logger.Log($"{nameof(RoomManager)}: Room name: {room.name}");
            }
        }
        
        /// <summary>
        /// 내부에 있는 EffectMesh의 z-fighting을 방지하기 위해 패스스루 EffectMesh들을 약간 오프셋합니다.
        /// </summary>
        private void OffsetPassthroughEffectMeshes()
        {
            float offset = 0.005f;
            
            var ceiling = Room.CeilingAnchor;
            {
                var mesh = FindCreatedEffectMesh(ceiling, passthroughRoom);
                mesh.transform.Translate(0, 0, -offset);
                _passthroughEffectMeshes.Add(mesh);
            }
            
            var floor = Room.FloorAnchor;
            {
                var mesh = FindCreatedEffectMesh(floor, passthroughRoom);
                mesh.transform.Translate(0, 0, -offset);
                _passthroughEffectMeshes.Add(mesh);
            }
            
            foreach (var wall in Room.WallAnchors)
            {
                var mesh = FindCreatedEffectMesh(wall, passthroughRoom);
                mesh.transform.Translate(0, 0, -offset);
                _passthroughEffectMeshes.Add(mesh);
            }
        }
        
        /// <summary>
        /// 패스스루 이펙트 메쉬들의 가시성을 설정합니다.
        /// </summary>
        public bool HidePassthroughEffectMesh
        {
            set => passthroughRoom.HideMesh = value;
        }
        
        /// <summary>
        /// 앵커에서 특정 EffectMesh로 생성된 메시를 찾습니다.
        /// </summary>
        /// <param name="anchor">찾을 앵커</param>
        /// <param name="effectMesh">메시를 생성한 EffectMesh</param>
        /// <returns>메시의 transform</returns>
        public GameObject FindCreatedEffectMesh(MRUKAnchor anchor, EffectMesh effectMesh) => anchor
            .GetComponentsInChildren<MeshRenderer>()
            .First(mesh => mesh.sharedMaterial == effectMesh.MeshMaterial).gameObject;
        
        /// <summary>
        /// 주어진 위치에서 가장 가까운 문 사이의 거리를 반환합니다.
        /// </summary>
        /// <param name="position">대상 위치</param>
        /// <param name="doorPosition">가장 가까운 문의 위치, 문이 없으면 Vector3.zero</param>
        /// <returns>문의 위치, 문이 없으면 0</returns>
        public float ClosestDoorDistance(Vector3 position, out Vector3 doorPosition)
        {
            // 캐시된 단일 문 위치가 있으면 바로 반환합니다.
            if (_uniqueDoorPosition is not null)
            {
                doorPosition = _uniqueDoorPosition.Value;
                return Vector3.Distance(position, _uniqueDoorPosition.Value);
            };
            
            // 문이 하나라면 해당 문을 캐시합니다.
            if (_doors.Count == 1)
            {
                _uniqueDoorPosition = _doors.First().transform.position;
                doorPosition = _uniqueDoorPosition.Value;
                return Vector3.Distance(position, _uniqueDoorPosition.Value);
            }

            // 여러 문 중 현재 위치에서 가장 가까운 문을 찾아 반환합니다.
            doorPosition = _doors.OrderBy(door => Vector3.Distance(door.transform.position, position))
                .FirstOrDefault()?.transform.position ?? Vector3.zero;
             
            return Vector3.Distance(position, doorPosition);
        }
        
        /// <summary>
        /// 룸의 트랜스폼을 대상 위치와 방향이 월드 원점과 월드 forward가 되도록 변환합니다.
        /// </summary>
        /// <param name="newOrigin">원점이 될 위치</param>
        /// <param name="newForward">앞쪽이 될 방향</param>
        /// <remarks>카메라는 룸의 하위 계층이 아니므로 트래킹 스페이스를 같이 변환하여 룸이 변하는 것처럼 보이지 않도록 합니다.</remarks>
        public void SetRoomTransform(Vector3 newOrigin, Vector3 newForward)
        {
            // newOrigin 및 newforward는 룸의 트랜스폼에 아무 변환이 없는 상태에서의 상대적인 위치여야 하므로,
            // 트랜스폼이 변경된 상태로 이 함수가 호출되었다면 먼저 원래대로 되돌립니다.
            if (Room.transform.hasChanged)
            {
                Logger.Log($"{nameof(RoomManager)}.{nameof(SetRoomTransform)}: " +
                           $"Room transform has changed. Reverting to original transform before setting new transform.");
                
                newOrigin = Room.transform.InverseTransformPoint(newOrigin);
                newForward = Room.transform.InverseTransformDirection(newForward);
                RevertRoomTransform();
                newOrigin = Room.transform.TransformPoint(newOrigin);
                newForward = Room.transform.TransformDirection(newForward);
            }
            
            Logger.Log($"{nameof(RoomManager)}: Set room transform to <b>{newOrigin}</b> becoming origin and <b>{newForward}</b> becoming forward.");
            
            // 변환 전 룸에 상대적인 트래킹 스페이스 저장
            var initialTrackingSpacePosition = Room.transform.InverseTransformPoint(GameManager.CameraRig.trackingSpace.position);
            var initialTrackingSpaceRotation = Quaternion.Inverse(Room.transform.rotation) * GameManager.CameraRig.trackingSpace.rotation;
            
            // 룸을 newForward로 회전
            var targetRotation = Quaternion.LookRotation(newForward);
            Room.transform.rotation *= Quaternion.Inverse(targetRotation);
            
            // 룸을 newOrigin이 원점이 되도록 이동
            Room.transform.position -= Room.transform.TransformPoint(newOrigin);
            
            // 트래킹 스페이스를 변환된 룸에 대해서 변환
            GameManager.CameraRig.trackingSpace.position = Room.transform.TransformPoint(initialTrackingSpacePosition);
            GameManager.CameraRig.trackingSpace.rotation = Room.transform.rotation * initialTrackingSpaceRotation;
        }
        
        /// <summary>
        /// 룸의 트랜스폼을 원래대로 되돌립니다.
        /// </summary>
        public void RevertRoomTransform()
        {
            Room.transform.position = Vector3.zero;
            Room.transform.rotation = Quaternion.identity;
            
            GameManager.CameraRig.trackingSpace.position = Vector3.zero;
            GameManager.CameraRig.trackingSpace.rotation = Quaternion.identity;
            
            Room.transform.hasChanged = false;
        }
            

        private void OnValidate()
        {
            if (roomEnlarger.IsUnityNull()) roomEnlarger = FindAnyObjectByType<RoomEnlarger>();
        }
    }
}
