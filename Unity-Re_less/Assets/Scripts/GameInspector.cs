#if UNITY_EDITOR

using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// 에디터 전용 디버그 오브젝트에 붙어 인스펙터에서 확인하거나 조작할 것을 다루는 클래스입니다.
    /// </summary>
    internal class GameInspector : MonoBehaviour
    {
        /// <summary>
        /// 유일한 DontDestroyOnLoad 인스턴스
        /// 이 클래스는 싱글톤이여야 할 필요는 없지만 DontDestroyOnLoad에 여러번 스팸하지 않도록 인스턴스를 저장합니다.
        /// </summary>
        private static GameInspector dontDestroyInstance;
        
        /// <summary>
        /// GameManager 레퍼런스
        /// </summary>
        private GameManager _gameManager;
        
        private bool PlayMode => Application.isPlaying;
        
        /// <summary>
        /// 현재 게임 단계를 설정합니다.
        /// </summary>
        [SerializeField, EnableIf(nameof(PlayMode))]
        private GamePhase setGamePhaseTo;
        
        /// <summary>
        /// 비교를 위해 캐시된 기존 게임 단계
        /// </summary>
        private GamePhase _cachedSetGamePhaseTo;
        
        [ShowNativeProperty]
        private bool SpaceWarpEnabled => OVRManager.GetSpaceWarp();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadMainScene() => _gameManager?.LoadMainScene();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadVRScene() => _gameManager?.LoadVRScene();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadExitDreamScene() => _gameManager?.LoadExitDreamScene();
        
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
        
        private void ToggleSpaceWarp()
        {
            OVRManager.SetSpaceWarp(!OVRManager.GetSpaceWarp());
        }
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;

            if (dontDestroyInstance.IsUnityNull())
            {
                dontDestroyInstance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Update()
        {
            if (_cachedSetGamePhaseTo != setGamePhaseTo)
            {
                Debug.Log($"Current Phase : {_gameManager.CurrentPhase} -> Set Phase : {setGamePhaseTo}");
                _gameManager.CurrentPhase = setGamePhaseTo;
                _cachedSetGamePhaseTo = setGamePhaseTo;
            }
            _cachedSetGamePhaseTo = setGamePhaseTo = _gameManager.CurrentPhase;
        }
    }
}
#endif
