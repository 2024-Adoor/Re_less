using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Reless.Debug
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
        [SerializeField, OnValueChanged(nameof(OnSetGamePhaseToChanged)), EnableIf(nameof(PlayMode))]
        private GamePhase setGamePhaseTo;
        
        [ShowNativeProperty]
        private bool SpaceWarpEnabled => OVRManager.GetSpaceWarp();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadMainScene() => GameManager.LoadMainScene();
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadVRScene() => GameManager.LoadVRScene();
        
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
            // dontDestroyInstance가 없다면 진행합니다.
            if (dontDestroyInstance.IsUnityNull())
            {
                // 자신을 dontDestroyInstance 인스턴스에 저장합니다.
                dontDestroyInstance = this;
                
                // 플레이모드에서도 기존 계층구조의 위치에서 찾기 쉽도록 자신을 복제합니다.
                Instantiate(this, parent: this.transform.parent)
                    // GameInspector는 Debug 오브젝트 하위의 가장 앞에 위치한다고 가정합니다.
                    .transform.SetAsFirstSibling();

                
                // dontDestroyInstance를 씬이 바뀌어도 파괴되지 않도록 설정합니다.
                // 이렇게 하면 플레이 전 에디터 계층구조에서 선택했던 GameInspector 오브젝트가 씬이 바뀌어도 선택 해제되지 않으므로 다시 선택할 필요가 없습니다.
                transform.SetParent(null); // DontDestroyOnLoad로 만들기 위해 부모를 해제합니다.
                DontDestroyOnLoad(this);
            }
            
            // 게임 단계가 바뀔 때 setGamePhaseTo를 업데이트합니다.
            GameManager.PhaseChanged += phase => setGamePhaseTo = phase;
        }

        /// <summary>
        /// <see cref="setGamePhaseTo"/>가 인스펙터에서 변경될 때 호출됩니다.
        /// </summary>
        private void OnSetGamePhaseToChanged()
        {
            Logger.Log($"{nameof(GameInspector)}: set game phase to <b>{setGamePhaseTo}</b>");
            GameManager.CurrentPhase = setGamePhaseTo;
        }
    }
}
