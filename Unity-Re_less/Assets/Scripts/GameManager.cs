using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reless
{
    /// <summary>
    /// 게임의 전반을 관리하는 클래스입니다.
    /// NOTE: 향후 분리될 수 있음
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => _instance ??= new GameObject(nameof(GameManager)).AddComponent<GameManager>();
        private static GameManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public enum Phase
        {
            Title,
            Opening,
            Tutorial,
            Chapter1,
            Chapter2,
            Chapter3,
            Ending,
        }
        
        private Phase _currentPhase;

        public Phase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                _currentPhase = value;

                switch (_currentPhase)
                {
                    case Phase.Title: StartTitle(); break;
                    case Phase.Opening: openingBehaviour.enabled = true; break;
                    case Phase.Tutorial: StartTutorial(); break;
                    case Phase.Chapter1: StartChapter1(); break;
                    case Phase.Chapter2: StartChapter2(); break;
                    case Phase.Chapter3: StartChapter3(); break;
                    case Phase.Ending: OnEnding(); break;
                }
            }
        }
        
        [SerializeField, HideInInspector]
        private OpeningBehaviour openingBehaviour;

        private void OnEnding()
        {
            throw new NotImplementedException();
        }

        private void StartChapter3()
        {
            throw new NotImplementedException();
        }

        private void StartChapter2()
        {
            throw new NotImplementedException();
        }

        private void StartChapter1()
        {
        }

        private void StartTutorial()
        {
            throw new NotImplementedException();
        }

        private void StartTitle()
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
        
        /// <summary>
        /// 내부에 있는 EffectMesh의 z-fighting을 방지하기 위해 패스스루 EffectMesh들을 약간 오프셋합니다.
        /// </summary>
        private void OffsetPassthroughEffectMeshes()
        {
            float offset = 0.005f;
            
            var ceiling = _currentRoom.GetCeilingAnchor();
            Debug.Log(ceiling == null);
            {
                var mesh = FindCreatedEffectMesh(ceiling, passthroughRoom);
                Debug.Log(transform == null);
                mesh.transform.Translate(0, 0, -offset);
                _passthroughEffectMeshes.Add(mesh);
            }
            
            var floor = _currentRoom.GetFloorAnchor();
            {
                var mesh = FindCreatedEffectMesh(floor, passthroughRoom);
                mesh.transform.Translate(0, 0, -offset);
                _passthroughEffectMeshes.Add(mesh);
            }
            
            var walls = _currentRoom.GetWallAnchors();
            foreach (var wall in walls)
            {
                var mesh = FindCreatedEffectMesh(wall, passthroughRoom);
                mesh.transform.Translate(0, 0, -offset);
            }
        }

        private List<GameObject> _passthroughEffectMeshes = new();
        
        public void DestroyPassThroughEffectMeshes()
        {
            foreach (var mesh in _passthroughEffectMeshes)
            {
                Destroy(mesh);
            }
        }
        
        [SerializeField]
        private List<EffectMesh> virtualRoomEffectMeshes;
        
        [Button]
        public void CreateVirtualRoomEffectMeshes()
        {
            foreach (var effectMesh in virtualRoomEffectMeshes)
            {
                effectMesh.CreateMesh();
            }
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
        
        public MRUKAnchor KeyWall => _keyWall;
        
        public GameObject OpeningWall => FindCreatedEffectMesh(_keyWall, passthroughRoom);

        [Button]
        private void SetPhaseToOpening()
        {
            CurrentPhase = Phase.Opening;
        }

        [SerializeField]
        private EffectMesh passthroughRoom;

        private MRUKAnchor _keyWall;

        [SerializeField, HideInInspector]
        private OVRCameraRig cameraRig;
        
        public Vector3 PlayerPosition => cameraRig.centerEyeAnchor.position;
        
        [SerializeField]
        public OVRPassthroughLayer passthroughLayer;

        [ShowNonSerializedField]
        private bool _startedInRoom;
        
        private MRUKRoom _currentRoom;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        /// <summary>
        /// MRUK의 <see cref="MRUK.SceneLoadedEvent"/> 이벤트에 연결되어 호출됩니다.
        /// </summary>
        public void OnSceneLoaded()
        {
            _currentRoom = MRUK.Instance.GetCurrentRoom();
            _startedInRoom = _currentRoom.IsPositionInRoom(cameraRig.centerEyeAnchor.position);

            if (_currentPhase == Phase.Title)
            {
                if (_startedInRoom)
                {
                    // 방 안에서 시작했다면 문에 다가갔을 때 오프닝을 시작합니다.
                    StartCoroutine(CheckApproachDoor(
                        onApproach: () => { CurrentPhase = Phase.Opening; },
                        until: () => _currentPhase is Phase.Opening));
                }
                else
                {
                    // 방 안에서 시작하지 않았다면 방을 들어갈 때 오프닝을 시작합니다.
                    StartCoroutine(CheckEnterRoom(
                        onEnter: () => { CurrentPhase = Phase.Opening; },
                        until: () => _currentPhase is Phase.Opening));
                }
            }
            
            // 가장 긴 벽을 찾습니다.
            _keyWall = _currentRoom.GetKeyWall(out _);
            
            OnSceneLoadedEvent?.Invoke();
            OnSceneLoadedEvent = null;
            
            Debug.Log("OnSceneLoaded");
        }
        
        public Action OnSceneLoadedEvent { get; set; }

        /// <summary>
        /// MRUK의 다른 모든 <see cref="MRUK.SceneLoadedEvent"/> 이벤트들이 끝난 후 호출됩니다.
        /// </summary>
        public void OnEndSceneLoadedEvent()
        {
            OffsetPassthroughEffectMeshes();
        }
        
        private IEnumerator CheckEnterRoom(Action onEnter, Func<bool> until)
        {
            while (!until())
            {
                if (_currentRoom.IsPositionInRoom(cameraRig.centerEyeAnchor.position))
                {
                    Debug.Log("Enter Room");
                    onEnter?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
        
        private IEnumerator CheckApproachDoor(Action onApproach, Func<bool> until)
        {
            while (!until())
            {
                throw new NotImplementedException();
                
                yield return null;
            }
        }
          
        private void RestartCurrentPhase()
        {
            CurrentPhase = _currentPhase;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            cameraRig ??= FindObjectOfType<OVRCameraRig>();
            openingBehaviour ??= GetComponentInChildren<OpeningBehaviour>();
        }
        
        [Button]
        private void LoadVRScene()
        {
            SceneManager.LoadSceneAsync("VR Room");
        }
        
        [Button]
        private void LoadMainScene()
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
#endif
        
    }
}

