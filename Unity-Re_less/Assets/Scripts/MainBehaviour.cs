using System;
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
            if (_gameManager.CurrentPhase is > GameManager.Phase.Tutorial and < GameManager.Phase.Ending)
            {
                // 펜 활성화
                EnablePen();

                // 챕터별로 그릴 오브젝트 생성
                (GameObject sktech, GameObject obtaining) drawingPrefabPair = _gameManager.CurrentPhase switch
                {
                    GameManager.Phase.Chapter1 => (_sketchObjectPrefabs.chapter01, _obtainingObjectPrefabs.chapter01),
                    GameManager.Phase.Chapter2 => (_sketchObjectPrefabs.chapter02, _obtainingObjectPrefabs.chapter02),
                    GameManager.Phase.Chapter3 => (_sketchObjectPrefabs.chapter03, _obtainingObjectPrefabs.chapter03),
                };
                SetupSketchObject(drawingPrefabPair.sktech, drawingPrefabPair.obtaining);
            }
        }

        private void SetupSketchObject(GameObject sketchPrefab, GameObject obtainingObjectPrefab)
        {
            var sketchObject = Instantiate(sketchPrefab);
            sketchObject.transform.position = _gameManager.PlayerPosition;
            sketchObject.GetComponent<SketchOutline>().DrawingCompleted +=
                () => ObtainObject(sketchObject, obtainingObjectPrefab);
        }
        
        private void ObtainObject(GameObject sketchObject, GameObject obtainingObjectPrefab)
        {
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
            EnableCloseEyesToSleepPose();
        }
    }
}

