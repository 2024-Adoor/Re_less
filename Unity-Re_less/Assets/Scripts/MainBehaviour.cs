using System;
using NaughtyAttributes;
using UnityEngine;
using static Reless.Chapter;

namespace Reless
{
    /// <summary>
    /// MainScene에서의 동작을 제어합니다.
    /// </summary>
    public class MainBehaviour : MonoBehaviour
    {
        private GameManager _gameManager;
    
        [SerializeField]
        private CloseEyesToSleepPose closeEyesToSleepPose;
        
        [SerializeField]
        private Pen pen;

        [Serializable]
        private struct SketchObjectPrefabs
        {
            public GameObject chapter01;
            public GameObject chapter02;
            public GameObject chapter03;
        }
        
        [SerializeField]
        private SketchObjectPrefabs _sketchObjectPrefabs;
        
        [Serializable]
        private struct ObtainingObjectPrefabs
        {
            public GameObject chapter01;
            public GameObject chapter02;
            public GameObject chapter03;
        }
        
        [SerializeField]
        private ObtainingObjectPrefabs _obtainingObjectPrefabs;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }
        
        private void Start()
        {
            // 튜토리얼 이후/엔딩 전 (= 챕터 중)에 MainScene으로 진입했다면
            if (_gameManager.CurrentChapter is not null)
            {
                // 펜 활성화
                EnablePen();
                
                // 그릴 오브젝트 생성
                SetupSketchObject(_sketchObjectPrefabs.chapter01, _obtainingObjectPrefabs.chapter01).transform.Translate(-1, 0, 0);
                SetupSketchObject(_sketchObjectPrefabs.chapter02, _obtainingObjectPrefabs.chapter02);
                SetupSketchObject(_sketchObjectPrefabs.chapter03, _obtainingObjectPrefabs.chapter03).transform.Translate(1, 0, 0);
                
                /* SUSPENDED: 전시용으로 모든 챕터의 그림을 한번에 그리는 것으로 기획이 변경됨
                 
                // 챕터별로 그릴 오브젝트 생성
                (GameObject sktech, GameObject obtaining) drawingPrefabPair = _gameManager.CurrentChapter switch
                {
                    Chapter1 => (_sketchObjectPrefabs.chapter01, _obtainingObjectPrefabs.chapter01),
                    Chapter2 => (_sketchObjectPrefabs.chapter02, _obtainingObjectPrefabs.chapter02),
                    Chapter3 => (_sketchObjectPrefabs.chapter03, _obtainingObjectPrefabs.chapter03),
                    _ => throw new ArgumentOutOfRangeException()
                };
                SetupSketchObject(drawingPrefabPair.sktech, drawingPrefabPair.obtaining);
                */
            }
        }

        /// <summary>
        /// 그릴 오브젝트와 얻을 오브젝트를 설정합니다.
        /// </summary>
        /// <param name="sketchPrefab">그릴 오브젝트의 프리팹</param>
        /// <param name="obtainingObjectPrefab">얻을 오브젝트의 프리팹</param>
        /// <returns>그릴 오브젝트</returns>
        private GameObject SetupSketchObject(GameObject sketchPrefab, GameObject obtainingObjectPrefab)
        {
            // 그릴 오브젝트를 생성합니다.
            var sketchObject = Instantiate(sketchPrefab);
            
            // 그릴 오브젝트를 플레이어의 시야에 배치합니다.
            sketchObject.transform.position = GameManager.EyeAnchor.position + GameManager.EyeAnchor.forward * 0.5f;
            
            // 그릴 오브젝트가 그려졌을 때 얻을 오브젝트를 획득하게 합니다.
            sketchObject.GetComponent<SketchOutline>().DrawingCompleted +=
                () => ObtainObject(sketchObject, obtainingObjectPrefab);
            
            return sketchObject;
        }
        
        /// <summary>
        /// 얻을 오브젝트를 획득합니다.
        /// </summary>
        /// <param name="sketchObject">그릴 오브젝트</param>
        /// <param name="obtainingObjectPrefab">얻을 오브젝트의 프리팹</param>
        private void ObtainObject(GameObject sketchObject, GameObject obtainingObjectPrefab)
        {
            // 그릴 오브젝트 위치에 얻을 오브젝트를 생성하고 그릴 오브젝트를 제거합니다.
            Instantiate(obtainingObjectPrefab).transform.position = sketchObject.transform.position;
            Destroy(sketchObject);
        }
        
        private void EnableCloseEyesToSleepPose()
        {
            closeEyesToSleepPose.transform.gameObject.SetActive(true);
        }
        
        private void EnablePen()
        {
            pen.gameObject.SetActive(true);
        }

        [Button]
        public void AchieveEnterCondition()
        {
            AchieveEnterCondition(_gameManager.CurrentChapter switch
            { 
                Chapter1 => 1, 
                Chapter2 => 2,
                Chapter3 => 3,
                _ => 0
            });
        }
        
        public void AchieveEnterCondition(int chapter)
        {
            EnableCloseEyesToSleepPose();
        }
    }
}

