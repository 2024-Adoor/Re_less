using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Reless.MR
{
    /// <summary>
    /// MRUK를 통한 현실 방 관리
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        [NonSerialized]
        public MRUKRoom Room;
        
        [SerializeField]
        private List<EffectMesh> virtualRoomEffectMeshes;
        
        [SerializeField]
        private EffectMesh passthroughRoom;
        
        [SerializeField]
        private EffectMesh sppPassThroughRoom;
        
        [SerializeField]
        private EffectMesh edgeEffect;
        
        public ReadOnlyCollection<GameObject> PassthroughEffectMeshes => _passthroughEffectMeshes.AsReadOnly();
        
        private readonly List<GameObject> _passthroughEffectMeshes = new();

        public ReadOnlyCollection<GameObject> SppPassThroughMeshes => _sppPassThroughMeshes.AsReadOnly();
        
        private readonly List<GameObject> _sppPassThroughMeshes = new();
        
        public MRUKAnchor KeyWall => _keyWall;
        
        private MRUKAnchor _keyWall;

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
            
            _doors ??= Room.Anchors
                .Where(anchor => anchor.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.DOOR_FRAME)
                .AsReadOnlyCollection();
            
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
        /// 문이 하나라면 해당 문을 캐시합니다.
        /// </summary>
        private Vector3? _uniqueDoorPosition;

        private ICollection<MRUKAnchor> _doors;
        

        [ReadOnly]
        public RoomEnlarger roomEnlarger;

        public GameObject OpeningWall => FindCreatedEffectMesh(_keyWall, passthroughRoom);
        
        private void Awake()
        {
            Assert.IsTrue(instance is null || instance == this, "there are multiple RoomManager instances.");
        }
        
        public void OnSceneLoaded()
        {
            Debug.Log($"{nameof(RoomManager)}: MRUK Scene loaded.");
            
            Assert.IsNull(instance, "RoomManager is singleton but already exists.");
            instance = this;
            Debug.Log("dont destroy on load");
            DontDestroyOnLoad(instance.gameObject);
            
            DebugRoomInfo();
            
            // 생성된 방을 자식으로 설정합니다.
            Room = MRUK.Instance.GetCurrentRoom();
            Room.transform.parent = this.transform;
            
            // 가장 긴 벽을 찾습니다.
            _keyWall = Room.GetKeyWall(out _);
            
            edgeEffect.CreateMesh();
            passthroughRoom.CreateMesh();
            sppPassThroughRoom.CreateMesh();

            FindAndAddSppPassThroughMesh(Room.CeilingAnchor);
            FindAndAddSppPassThroughMesh(Room.FloorAnchor);
            foreach (var wall in Room.WallAnchors)
            {
                FindAndAddSppPassThroughMesh(wall);
            }
            
            void FindAndAddSppPassThroughMesh(MRUKAnchor anchor)
            {
                for (int i = 0; i < anchor.transform.childCount; i++)
                {
                    var childMesh = anchor.transform.GetChild(i);
                    
                    // MeshRenderer가 없는 경우 SPP 패스스루 메쉬로 간주합니다. (할당된 머티리얼 없으면 EffectMesh가 렌더러를 생성하지 않는듯함)
                    // 향후 더 나은 방법으로 수정 필요.
                    if (childMesh.GetComponent<MeshRenderer>() == null)
                    {
                        Debug.Log($"SPP PassThrough Mesh Found. Adding to list.");
                        _sppPassThroughMeshes.Add(anchor.transform.GetChild(i).gameObject);
                    }
                }
            }
            
            OffsetPassthroughEffectMeshes();
            
            GameManager.Instance.OnSceneLoaded();
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

        public bool RoomObjectActive { set => gameObject.SetActive(value); }
            
        

        private void DebugRoomInfo()
        {
            var rooms = MRUK.Instance.Rooms;
            Debug.Log($"{nameof(RoomManager)}: Room count: {rooms.Count}");
            foreach (var room in rooms)
            {
                Debug.Log($"{nameof(RoomManager)}: Room name: {room.name}");
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
        

        // Update is called once per frame
        void Update()
        {
        }
        
        [CanBeNull]
        public static RoomManager Instance 
        {
            get
            {
                if (instance.IsUnityNull() && yetLoadScene)
                {
                    // 인스턴스가 없으면 RoomSetup 씬이 로드되지 않은 것입니다. 씬을 한 번만 로드합니다.
                    SceneManager.LoadSceneAsync("RoomSetup", LoadSceneMode.Additive);
                    yetLoadScene = false;
                }
                
                return instance.AsUnityNull();
            }
        }
        private static RoomManager instance;
        
        /// <summary>
        /// 씬을 아직 로드한 적 없으면 true
        /// </summary>
        private static bool yetLoadScene = true;

        /// <summary>
        /// MRUK 씬이 로드되었을 때 호출될 액션
        /// RoomManager의 인스턴스가 null인 경우 작업을 연기하는 데 사용됩니다.
        /// </summary>
        public static Action OnMRUKSceneLoaded { get; set; }

        private void OnValidate()
        {
            roomEnlarger = FindObjectOfType<RoomEnlarger>();
        }
    }
}
