using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Reless.Game;
using Reless.MR;
using UnityEngine;
using static Reless.Chapter;

namespace Reless
{
    /// <summary>
    /// MainScene에서의 동작을 제어합니다.
    /// </summary>
    public class MainBehaviour : MonoBehaviour
    {
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
        
        public List<ObtainingObject> ObtainingObjects { get; private set; }

        [SerializeField]
        private GameObject polaroidsPrefab;
        
        private GameObject _polaroids;
        
        private void Awake()
        {
            OnEnding(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnEnding;
        }
        
        private void Start()
        {
            // 튜토리얼 이후/엔딩 전 (= 챕터 중)에 MainScene으로 진입했다면
            if (GameManager.CurrentChapter is not null)
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

        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnEnding;
        }

        /// <summary>
        /// 현재 게임 단계가 Ending이면 엔딩에 필요한 동작을 수행합니다.
        /// </summary>
        private void OnEnding(GamePhase phase)
        {
            if (phase is not GamePhase.Ending) return;

            if (RoomManager.Instance is not null)
            {
                // 엔딩에서 폴라로이드를 생성합니다.
                CreatePolaroids();
            }
            else
            {
                // RoomManager가 없다면 방이 로드된 뒤에 생성되도록 이벤트에 등록합니다.
                RoomManager.OnMRUKSceneLoaded += CreatePolaroids;
            }
            
            void CreatePolaroids()
            {
                Transform popupBook = GameManager.Instance.PopupBook.transform;

                // 팝업북 위치에 생성
                _polaroids = Instantiate(polaroidsPrefab, popupBook.position, popupBook.rotation, popupBook.parent);
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
            var obtainObjects = Instantiate(obtainingObjectPrefab);
            obtainObjects.transform.position = sketchObject.transform.position;
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
        
        /// <summary>
        /// 모든 얻은 오브젝트가 스냅되었는지 확인합니다.
        /// </summary>
        public void CheckAchieveEnterCondition()
        {
            if (ObtainingObjects.TrueForAll(obtainingObj => obtainingObj.IsSnapped))
            {
                AchieveEnterCondition();
            }
        }

        [Button]
        public void AchieveEnterCondition()
        {
            AchieveEnterCondition(GameManager.CurrentChapter switch
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

