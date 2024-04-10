using System;
using System.Collections;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using Reless.MR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Reless
{
    /// <summary>
    /// 게임의 전반을 관리하는 클래스입니다.
    /// NOTE: 향후 분리될 수 있음
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance 
        {
            get
            {
                if (_instance == null)
                {
                    // 씬에서 게임 매니저 오브젝트를 찾습니다.
                    _instance = FindObjectOfType<GameManager>();
                    
                    if (_instance == null)
                    {
                        Debug.LogWarning($"{nameof(GameManager)}: There is no instance in the scene. Creating new one.");
                        _instance = new GameObject(nameof(GameManager)).AddComponent<GameManager>();
                    }
                }
                
                // 게임 매니저 오브젝트는 씬이 바뀌어도 파괴되지 않습니다.
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }
        private static GameManager _instance;
        
        public static bool NotInThisScene => _instance == null;

        public void Awake()
        {
            if (_instance is not null && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            SceneManager.LoadSceneAsync("RoomSetup", LoadSceneMode.Additive)
                .completed += operation =>
            {
                Debug.Log("RoomSetupScene Loaded");
            };
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

        [ShowNativeProperty]
        public Phase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                // 클램프
                if (value < Phase.Title) _currentPhase = Phase.Title;
                else if (value > Phase.Ending) _currentPhase = Phase.Ending;
                else _currentPhase = value;

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

                PopupBookActive = _currentPhase;
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
        
        private void Update()
        {
            ForcePhaseControlling();
        }

        /// <summary>
        /// 특수 상황용, 특정 키 조합으로 강제로 페이즈를 변경합니다.
        /// </summary>
        private void ForcePhaseControlling()
        {
            // Start 버튼(왼쪽 컨트롤러 메뉴 버튼)이 눌려 있는 채로
            if (OVRInput.Get(OVRInput.Button.Start, OVRInput.Controller.LTouch))
            {
                // 왼쪽 스틱 버튼을 왼쪽으로 눌렀을 때
                if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
                {
                    // 페이즈 단계 줄이기
                    CurrentPhase--;
                }
                // 왼쪽 스틱 버튼을 오른쪽으로 눌렀을 때
                else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
                {
                    // 페이즈 단계 늘리기
                    CurrentPhase++;
                }
                // 왼쪽 스틱 버튼이 위쪽으로 눌렸을 때
                else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
                {
                    // 메인 씬 로드
                    LoadMainScene();
                }    
                // 왼쪽 스틱 버튼이 아래쪽으로 눌렸을 때
                else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
                {
                    // VR 씬 로드
                    LoadVRScene();
                }
            }
        }

        [SerializeField, HideInInspector]
        private OVRCameraRig cameraRig;
        
        public Vector3 PlayerPosition => cameraRig.centerEyeAnchor.localPosition;
        


        public void OnSceneLoaded()
        {
            _startedInRoom = RoomManager.Instance.Room.IsPositionInRoom(PlayerPosition);

            if (_startedInRoom)
            {
                // 방 안에서 시작했다면 문에 다가갔을 때 오프닝을 시작합니다.
                StartCoroutine(CheckingApproachDoor(
                    onApproach: () => { CurrentPhase = Phase.Opening; },
                    until: () => _currentPhase is not Phase.Title));
            }
            else
            {
                // 방 안에서 시작하지 않았다면 방을 들어갈 때 오프닝을 시작합니다.
                StartCoroutine(CheckingEnterRoom(
                    onEnter: () => { CurrentPhase = Phase.Opening; },
                    until: () => _currentPhase is not Phase.Title));
            }
        }
        
        /// <summary>
        /// 방 안에서 시작했는지 여부
        /// </summary>
        [ShowNonSerializedField]
        private bool _startedInRoom;        
        
        private IEnumerator CheckingEnterRoom(Action onEnter, Func<bool> until)
        {
            while (!until())
            {
                if (RoomManager.Instance.Room.IsPositionInRoom(PlayerPosition))
                {
                    onEnter?.Invoke();
                    yield break;
                }
                
                yield return null;
            }
        }
        
        private IEnumerator CheckingApproachDoor(Action onApproach, Func<bool> until)
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

        public PopupBook PopupBook { set => _popupBook ??= value; }
        
        private PopupBook _popupBook;

        private Phase PopupBookActive
        {
            set
            {
                if (_popupBook.IsUnityNull()) return;

                _popupBook.gameObject.SetActive(value switch
                {
                    <= Phase.Tutorial => false,
                    > Phase.Tutorial => true,
                });
            }
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public AsyncOperation LoadMainScene()
        {
            // 메인 씬을 로드할 때는 현실 룸을 다시 활성화합니다.
            RoomManager.Instance.RoomObjectActive = true;
            
            var asyncLoad = SceneManager.LoadSceneAsync("MainScene");
            asyncLoad.completed += operation =>
            {
                // 씬을 다시 불러왔으므로 카메라 리그를 다시 찾습니다.
                Debug.Log("MainScene Loaded");
                Debug.Log("Finding OVRCameraRig");
                cameraRig = FindObjectOfType<OVRCameraRig>();
                Assert.IsNotNull(cameraRig, "OVRCameraRig not found");
            };
            return asyncLoad;
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public AsyncOperation LoadVRScene()
        {
            // VR 씬을 로드할 때는 현실 룸을 비활성화합니다.
            RoomManager.Instance.RoomObjectActive = false;
            
            var asyncLoad = SceneManager.LoadSceneAsync("VR Room");
            asyncLoad.completed += operation =>
            {
                // 씬을 다시 불러왔으므로 카메라 리그를 다시 찾습니다.
                Debug.Log("VR Room Loaded");
                Debug.Log("Finding OVRCameraRig");
                cameraRig = FindObjectOfType<OVRCameraRig>();
                Assert.IsNotNull(cameraRig, "OVRCameraRig not found");
            };
            return asyncLoad;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public AsyncOperation LoadExitDreamScene()
        {
            return SceneManager.LoadSceneAsync("ExitDream", LoadSceneMode.Additive);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            cameraRig ??= FindObjectOfType<OVRCameraRig>();
            openingBehaviour ??= GetComponentInChildren<OpeningBehaviour>();
        }
        
        [Button]
        private void SetPhaseToOpening()
        {
            CurrentPhase = Phase.Opening;
        }
        
        [Button]
        private void SetPhaseToChapter1()
        {
            CurrentPhase = Phase.Chapter1;
        }
#endif
        
    }
}

