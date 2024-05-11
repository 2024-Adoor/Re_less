using System;
using System.Collections.Generic;
using Reless.Game;
using Reless.MR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static Reless.BuildScene;
using Logger = Reless.Debug.Logger;
using SceneManager = Reless.Util.SceneManager;

namespace Reless
{
    /// <summary>
    /// 게임의 전반을 관리하는 싱글톤 클래스입니다.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// 싱글톤 인스턴스
        /// </summary>
        public static GameManager Instance => instance ??= new GameManager();
        private static GameManager instance;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            // 메인 씬에서만 시작 시에 바로 방을 셋업합니다.
            if (SceneManager.ActiveScene is MainScene) RoomManager.SetupRoom();

            PhaseChanged += phase => { if (phase is GamePhase.Title) SceneManager.LoadAsync(MainScene); };
        }

        /// <summary>
        /// 게임의 현재 진행 단계
        /// </summary>
        public static GamePhase CurrentPhase
        {
            get => Instance._currentPhase;
            set
            {
                // 유효한 값으로 제한합니다.
                Instance._currentPhase = value switch
                {
                    < GamePhase.Title => GamePhase.Title,
                    > GamePhase.Ending => GamePhase.Ending,
                    _ => value
                };
                

                // 이벤트를 발생시킵니다.
                PhaseChanged?.Invoke(Instance._currentPhase);

                switch (Instance._currentPhase)
                {
                    case GamePhase.Chapter1 or GamePhase.Chapter2 or GamePhase.Chapter3:
                        Instance.spawnedWallHints.ForEach(wallHint => wallHint.SetActive(true)); break;
                        default: Instance.spawnedWallHints.ForEach(wallHint => wallHint.SetActive(false)); break;
                }

                Instance.PopupBookActive = Instance._currentPhase;
                
                Logger.Log($"{nameof(GameManager)}: phase changed to: <b>{Instance._currentPhase}</b>");
            }
        }
        private GamePhase _currentPhase;

        /// <summary>
        /// 현재 페이즈가 챕터 중인 경우 해당 챕터를 반환합니다.
        /// 페이즈가 챕터 중이 아닌 경우 null을 반환합니다.
        /// </summary>
        public static Chapter? CurrentChapter => Enum.IsDefined(typeof(Chapter), (Chapter)CurrentPhase) ? (Chapter)CurrentPhase : null;

        [NonSerialized]
        public List<GameObject> spawnedWallHints = new List<GameObject>();

        /// <summary>
        /// 게임 진행 단계가 변경될 때 호출되는 이벤트입니다.
        /// </summary>
        public static event Action<GamePhase> PhaseChanged;
        
        // TODO: Input System으로 전환
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

        /// <summary>
        /// 현재 씬의 OVRCameraRig를 반환합니다.
        /// </summary>
        public static OVRCameraRig CameraRig
        {
            get
            {
                // 캐시된 값이 없다면 찾아 캐시합니다.
                Instance._cameraRig = Instance._cameraRig.AsUnityNull();
                Instance._cameraRig ??= UnityEngine.Object.FindAnyObjectByType<OVRCameraRig>();
                
                // OVRCameraRig는 씬에서 찾을 수 있어야 합니다.
                Assert.IsNotNull(Instance._cameraRig, "OVRCameraRig not found.");
                
                return Instance._cameraRig;
            }
        }
        private OVRCameraRig _cameraRig;

        /// <summary>
        /// 카메라(플레이어의 머리)의 중앙 눈 앵커를 반환합니다.
        /// </summary>
        public static Transform EyeAnchor => CameraRig.centerEyeAnchor;
        
        /// <summary>
        /// 프로젝트 전체에서 사용할 InputActions 인스턴스를 반환합니다.
        /// </summary>
        public static InputActions InputActions
        {
            get
            {
                if (Instance._inputActions is null)
                {
                    Instance._inputActions = new InputActions();
                    Instance._inputActions.Enable();
                }
                
                return Instance._inputActions;
            }
        } 
        private InputActions _inputActions;

        public PopupBook PopupBook {
            get => _popupBook;
            set => _popupBook ??= value;
        }
        
        private PopupBook _popupBook;

        private GamePhase PopupBookActive
        {
            set
            {
                if (_popupBook.IsUnityNull()) return;

                _popupBook.gameObject.SetActive(value switch
                {
                    <= GamePhase.Tutorial or GamePhase.Ending => false,
                    > GamePhase.Tutorial => true,
                });
            }
        }
        
        public static AsyncOperation LoadMainScene()
        {
            // 메인 씬을 로드할 때는 현실 룸을 다시 활성화합니다.
            if (RoomManager.Instance is not null) RoomManager.Instance.RoomObjectActive = true;

            var asyncLoad = SceneManager.LoadAsync(MainScene);
            asyncLoad.completed += operation =>
            {
                Logger.Log("MainScene Loaded");
            };
            return asyncLoad;
        }
        
        public static AsyncOperation LoadVRScene()
        {
            // VR 씬을 로드할 때는 현실 룸을 비활성화합니다.
            if (RoomManager.Instance is not null) RoomManager.Instance.RoomObjectActive = false;
            
            var asyncLoad = SceneManager.LoadAsync(VRRoom);
            asyncLoad.completed += operation =>
            {
                Logger.Log("VR Room Loaded");
            };
            return asyncLoad;
        }

        public static AsyncOperation LoadExitDreamScene()
        {
            var asyncLoad = SceneManager.LoadAsync(ExitDream, LoadSceneMode.Additive);
            asyncLoad.completed += operation =>
            {
                Logger.Log("ExitDreamScene Loaded");
            };
            return asyncLoad;
        }
    }
}

