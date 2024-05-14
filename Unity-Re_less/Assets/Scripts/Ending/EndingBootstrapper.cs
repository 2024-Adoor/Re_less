using UnityEngine;

namespace Reless.Ending
{
    /// <summary>
    /// 엔딩을 준비하고 불러옵니다.
    /// </summary>
    public class EndingBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            if (GameManager.CurrentPhase is GamePhase.Ending) BootstrapEnding();
            GameManager.PhaseChanged += OnEnding;
        }
        
        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnEnding;
        }

        private void OnEnding(GamePhase phase)
        {
            if (phase is not GamePhase.Ending) return;
            
            BootstrapEnding();
        }
        
        /// <summary>
        /// 엔딩을 시작합니다.
        /// </summary>
        private void BootstrapEnding()
        {
            // 엔딩을 시작합니다.
        }
        
    }
}