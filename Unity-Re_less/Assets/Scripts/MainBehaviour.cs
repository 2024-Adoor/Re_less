using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using UnityEngine;

namespace Reless
{
    /// <summary>
    /// MainScene에서의 동작을 제어합니다.
    /// </summary>
    public class MainBehaviour : MonoBehaviour
    {
        private GameManager _gameManager;
    
        [SerializeField]
        private FindSpawnPositions popupBookSpawner;
        
        [SerializeField]
        private CloseEyesToSleepPose closeEyesToSleepPose;
        
        private void Start()
        {
            _gameManager = GameManager.Instance;
            
            // 튜토리얼 이후에 MainScene으로 진입했다면
            if (_gameManager.CurrentPhase > GameManager.Phase.Tutorial)
            {
                // 눈을 감고 자는 포즈 활성화
                EnableCloseEyesToSleepPose();
            }
        }

        public void OnSceneLoaded()
        {
            // 튜토리얼 이후에 MainScene으로 진입했다면
            if (_gameManager.CurrentPhase > GameManager.Phase.Tutorial)
            {
                SpawnPopupBook();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        [Button]
        private void SpawnPopupBook()
        {
            popupBookSpawner.StartSpawn();
        }
        
        
        private void EnableCloseEyesToSleepPose()
        {
            closeEyesToSleepPose.transform.gameObject.SetActive(true);
        }
    }
}

