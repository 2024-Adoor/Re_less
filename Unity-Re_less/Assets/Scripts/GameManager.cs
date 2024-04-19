using System;
using System.Collections;
using System.Collections.Generic;
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
                    case Phase.Opening: openingBehaviour.StartOpening(); break;
                    case Phase.Tutorial: StartTutorial(); break;
                    case Phase.Chapter1: StartChapter1(); break;
                    case Phase.Chapter2: StartChapter2(); break;
                    case Phase.Chapter3: StartChapter3(); break;
                    case Phase.Ending: OnEnding(); break;
                }

                switch (_currentPhase)
                {
                    case Phase.Chapter1 or Phase.Chapter2 or Phase.Chapter3:
                        spawnedWallHints.ForEach(wallHint => wallHint.SetActive(true)); break;
                        default: spawnedWallHints.ForEach(wallHint => wallHint.SetActive(false)); break;
                }

                PopupBookActive = _currentPhase;
            }
        }

        [NonSerialized]
        public List<GameObject> spawnedWallHints = new List<GameObject>();
        
        [SerializeField, HideInInspector]
        private OpeningBehaviour openingBehaviour;

        private void OnEnding()
        {
        }

        private void StartChapter3()
        {
        }

        private void StartChapter2()
        {
        }

        private void StartChapter1()
        {
        }

        private void StartTutorial()
        {
        }

        private void StartTitle()
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
        
        private void Update()
        {
            ForcePhaseControlling();

            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                Debug.Log("On Y Button Pressed");
                ToggleSpaceWarp();
            }
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

        public OVRCameraRig CameraRig
        {
            get
            {
                _cameraRig = _cameraRig.AsUnityNull();
                _cameraRig ??= FindAnyObjectByType<OVRCameraRig>();
                Assert.IsNotNull(_cameraRig, "OVRCameraRig not found.");
                return _cameraRig;
            }
        }
        private OVRCameraRig _cameraRig;

        public Vector3 PlayerPosition => CameraRig.centerEyeAnchor.localPosition;


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
                var distance = RoomManager.Instance.ClosestDoorDistance(PlayerPosition, out _);
                if (distance == 0f)
                {
                    Debug.LogWarning("Door not found.");
                    yield break;
                }
                
                if (distance < 0.5f)
                {
                    onApproach?.Invoke();
                    yield break;
                }
                
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
                Debug.Log("MainScene Loaded");
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
                Debug.Log("VR Room Loaded");
            };
            return asyncLoad;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public AsyncOperation LoadExitDreamScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("ExitDream", LoadSceneMode.Additive);
            asyncLoad.completed += operation =>
            {
                Debug.Log("ExitDreamScene Loaded");
            };
            return asyncLoad;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _cameraRig ??= FindObjectOfType<OVRCameraRig>();
            openingBehaviour ??= GetComponentInChildren<OpeningBehaviour>();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SetPreviousPhase()
        {
            CurrentPhase--;
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SetNextPhase()
        {
            CurrentPhase++;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void EnableAppSW()
        {
            OVRManager.SetSpaceWarp(true);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void DisableAppSW()
        {
            OVRManager.SetSpaceWarp(false);
        }

        [ShowNativeProperty]
        private bool SpaceWarpEnabled => OVRManager.GetSpaceWarp();
#endif
        private void ToggleSpaceWarp()
        {
            OVRManager.SetSpaceWarp(!OVRManager.GetSpaceWarp());
        }
    }
}

