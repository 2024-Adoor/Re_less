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
        
        [SerializeField]
        private Pen pen;

        [SerializeField] 
        private GameObject ch01SketchObject;
        
        [SerializeField] 
        private GameObject ch02SketchObject;
        
        [SerializeField] 
        private GameObject ch03SketchObject;
        
        [SerializeField]
        private GameObject ch01Object;
        
        [SerializeField]
        private GameObject ch02Object;
        
        [SerializeField]
        private GameObject ch03Object;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }
        
        private void Start()
        {
            
            // 튜토리얼 이후에 MainScene으로 진입했다면
            if (_gameManager.CurrentPhase > GameManager.Phase.Tutorial)
            {
                // 눈을 감고 자는 포즈 활성화
                EnableCloseEyesToSleepPose();
                
                // 펜 활성화
                EnablePen();

                var sketch = Instantiate(ch02SketchObject);
                sketch.GetComponent<SketchOutline>().DrawingCompleted += () =>
                {
                    var obtainObject = Instantiate(ch02Object);
                    obtainObject.transform.position = sketch.transform.position;
                    Destroy(sketch);
                };
            }
            
        }

        public void OnSceneLoaded()
        {
            // 튜토리얼 이후에 MainScene으로 진입했다면
            if (GameManager.Instance.CurrentPhase > GameManager.Phase.Tutorial)
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
        
        private void EnablePen()
        {
            pen.gameObject.SetActive(true);
        }
    }
}

