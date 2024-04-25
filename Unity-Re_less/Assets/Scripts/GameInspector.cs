#if UNITY_EDITOR

using System;
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
        private static GameInspector dontDestroyInstance;
        
        private GameManager _gameManager;
        
        [SerializeField]
        private GamePhase setGamePhaseTo;
        
        private GamePhase _cachedSetGamePhaseTo;
        
        [ShowNativeProperty]
        private GamePhase CurrentGamePhase => _gameManager?.CurrentPhase ?? default;
        
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
