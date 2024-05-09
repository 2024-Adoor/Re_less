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
        
        private bool PlayMode => Application.isPlaying;
        
        /// <summary>
        /// 현재 게임 단계를 설정합니다.
        /// </summary>
        [SerializeField, OnValueChanged(nameof(OnPhaseChanged)), EnableIf(nameof(PlayMode))]
        private GamePhase setGamePhaseTo;
        
        [ShowNativeProperty]
        private bool SpaceWarpEnabled => OVRManager.GetSpaceWarp();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadMainScene() => GameManager.LoadMainScene();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadVRScene() => GameManager.LoadVRScene();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadExitDreamScene() => GameManager.LoadExitDreamScene();
        
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
            if (dontDestroyInstance.IsUnityNull())
            {
                dontDestroyInstance = this;
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// <see cref="setGamePhaseTo"/>가 인스펙터에서 변경될 때 호출됩니다.
        /// </summary>
        private void OnPhaseChanged()
        {
            Debug.Log($"{nameof(GameInspector)}: set game phase to <b>{setGamePhaseTo}</b>");
            GameManager.CurrentPhase = setGamePhaseTo;
        }
    }
}
