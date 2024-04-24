#if UNITY_EDITOR

using UnityEngine;

namespace Reless
{
    /// <summary>
    /// 에디터 전용 디버그 오브젝트에 붙어 인스펙터에서 확인하거나 조작할 것을 다루는 클래스입니다.
    /// </summary>
    public class GameInspector : MonoBehaviour
    {
        private GameManager _gameManager;
        
        public GamePhase setGamePhaseTo;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Update()
        {
            if (_gameManager.CurrentPhase != setGamePhaseTo)
            {
                _gameManager.CurrentPhase = setGamePhaseTo;
            }
        }
        
    }
}
#endif
