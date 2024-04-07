using System;
using System.Collections;
using System.Linq;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Reless
{
    /// <summary>
    /// 게임의 전반을 관리하는 클래스입니다.
    /// NOTE: 향후 분리될 수 있음
    /// </summary>
    public class GameManager : MonoBehaviour
    {
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
            throw new NotImplementedException();
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
            float scale = 1.1f;
            
            var ceiling = _currentRoom.GetCeilingAnchor();
            Debug.Log(ceiling == null);
            {
                var mesh = FindCreatedEffectMesh(ceiling, passthroughRoom);
                Debug.Log(transform == null);
                //transform.localScale = new Vector3(scale, scale, 1);
                mesh.transform.Translate(0, 0, -offset);
            }
            
            var floor = _currentRoom.GetFloorAnchor();
            {
                var mesh = FindCreatedEffectMesh(floor, passthroughRoom);
                //transform.localScale = new Vector3(scale, scale, 1);
                mesh.transform.Translate(0, 0, -offset);
            }
            
            var walls = _currentRoom.GetWallAnchors();
            foreach (var wall in walls)
            {
                var mesh = FindCreatedEffectMesh(wall, passthroughRoom);
                //transform.localScale = new Vector3(scale, scale, 1);
                mesh.transform.Translate(0, 0, -offset);
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
            
            // 가장 긴 벽을 찾습니다.
            _keyWall = _currentRoom.GetKeyWall(out _);
        }

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

        private void OnValidate()
        {
            cameraRig ??= FindObjectOfType<OVRCameraRig>();
            openingBehaviour ??= GetComponentInChildren<OpeningBehaviour>();
        }
    }
}

