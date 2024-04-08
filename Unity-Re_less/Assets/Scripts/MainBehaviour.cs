using UnityEngine;

namespace Reless
{
    /// <summary>
    /// MainScene������ ������ �����մϴ�.
    /// </summary>
    public class MainBehaviour : MonoBehaviour
    {
        private GameManager _gameManager;
    
        [SerializeField]
        private FindSpawnPositions popupBookSpawner;
        
        [SerializeField]
        private CloseEyesToSleepPose closeEyesToSleepPose;
        
        void Start()
        {
            _gameManager = GameManager.Instance;
            
            // Ʃ�丮�� ���Ŀ� MainScene���� �����ߴٸ�
            if (_gameManager.CurrentPhase > GameManager.Phase.Tutorial)
            {
                // �˾��� ����
                popupBookSpawner.StartSpawn();
                
                // ���� ���� �ڴ� ���� Ȱ��ȭ
                EnableCloseEyesToSleepPose();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void EnableCloseEyesToSleepPose()
        {
            closeEyesToSleepPose.transform.gameObject.SetActive(true);
        }
    }
}

