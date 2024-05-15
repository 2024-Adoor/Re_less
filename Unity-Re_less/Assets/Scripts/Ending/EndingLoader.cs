using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = Reless.Util.SceneManager;

namespace Reless.Ending
{
    /// <summary>
    /// 엔딩 씬을 불러옵니다.
    /// </summary>
    public class EndingLoader : MonoBehaviour
    {
        [ShowNonSerializedField]
        private bool _isInEnding;
        
        private void Awake()
        {
            OnEnding(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnEnding;
        }
        
        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnEnding;
        }
        
        private void OnEnding(GamePhase phase)
        {
            if (phase is not GamePhase.Ending)
            {
                ExitEnding();
                return;
            }
            
            LoadEnding();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LoadEnding()
        {
            if (_isInEnding) return;
            
            SceneManager.LoadAsync(BuildScene.Ending, LoadSceneMode.Additive);
            
            _isInEnding = true;
        }
        
        private void ExitEnding()
        {
            // 기존에 엔딩이였던 것이 아니라면 무시합니다.
            if (!_isInEnding) return;
            
            SceneManager.UnloadAsync(BuildScene.Ending);
            
            _isInEnding = false;
        }
    }
}