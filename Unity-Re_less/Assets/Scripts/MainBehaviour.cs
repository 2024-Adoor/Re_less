using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Meta.XR.MRUtilityKit;
using NaughtyAttributes;
using Reless.Game;
using Reless.MR;
using Reless.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using static Reless.Chapter;

namespace Reless
{
    /// <summary>
    /// MainScene에서의 동작을 제어합니다.
    /// </summary>
    public class MainBehaviour : MonoBehaviour
    {
        /// <summary>
        /// CloseEyesToSleepPose 레퍼런스
        /// </summary>
        [Header("References")]
        [SerializeField]
        private CloseEyesToSleepPose closeEyesToSleepPose;
        
        /// <summary>
        /// 펜 레퍼런스
        /// </summary>
        [SerializeField]
        private Pen pen;
        
        [SerializeField]
        private TMP_Text drawingProgressLabel;

        /// <summary>
        /// 그릴 오브젝트 프리팹들
        /// </summary>
        [Header("Prefabs")]
        [SerializeField]
        private SketchOutline[] sketchOutlinePrefabs;
        
        /// <summary>
        /// 현재 존재하는 그릴 오브젝트
        /// </summary>
        [CanBeNull] 
        private SketchOutline _currentSketchOutline;
        
        
        /// <summary>
        /// 얻을 오브젝트 프리팹들
        /// </summary>
        [SerializeField]
        private ObtainingObject[] obtainingObjectPrefabs;
        
        /// <summary>
        /// 현재 챕터에 해당하는 얻을 오브젝트 프리팹
        /// </summary>
        private ObtainingObject _currentChapterObtainingObjectPrefab;
        
        /// <summary>
        /// 현재 존재하는 얻은 오브젝트
        /// </summary>
        [CanBeNull] 
        private ObtainingObject _currentObtainedObject;
        
        private void Awake()
        {
            OnPhaseChanged(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnPhaseChanged;
        }
        
        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnPhaseChanged;
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            // 튜토리얼 이후/엔딩 전 (= 챕터 중)에 MainScene으로 진입했다면
            if (GameManager.CurrentChapter is not null)
            {
                // 펜 활성화
                pen.gameObject.SetActive(true);
                
                var chapterSketchOutlinePrefab = sketchOutlinePrefabs.Single(sketch => sketch.chapter == (Chapter)phase);
                _currentChapterObtainingObjectPrefab = obtainingObjectPrefabs.Single(obtaining => obtaining.chapter == (Chapter)phase);
                
                SetupSketchObject(chapterSketchOutlinePrefab);

                if (phase is GamePhase.Chapter1)
                {
                    StartCoroutine(ShowingChapter1Message());
                    
                    IEnumerator ShowingChapter1Message()
                    {
                        yield return new WaitForSeconds(1f);
                        GuideText.SetText("벽 너머의 세계에서 팝업북이 넘어왔다!", 4);
                        yield return new WaitForSeconds(5f);
                        GuideText.SetText("가운데 손가락 버튼으로 팝업북의 페이지를 잡고 넘겨보세요.", 4);
                        yield return new WaitForSeconds(5f);
                        GuideText.SetText("검지 손가락 버튼으로 그림을 상상해 보세요.", 4);
                    }
                }
            }
            // 챕터 중이 아닌 경우
            else
            {
                // 펜 비활성화
                pen.gameObject.SetActive(false);
                
                // 그릴 오브젝트가 있다면 제거합니다.
                if (_currentSketchOutline.IsUnityNull() is false)
                {
                    Destroy(_currentSketchOutline);
                    _currentSketchOutline = null;
                }
                
                // 얻을 오브젝트가 있고 팝업북에 배치되지 않았다면 제거합니다.
                if (_currentObtainedObject.IsUnityNull() is false && 
                    _currentObtainedObject!.IsSnapped is false)
                {
                    Destroy(_currentObtainedObject);
                    _currentObtainedObject = null;
                }
            }
        }

        /// <summary>
        /// 그릴 오브젝트와 얻을 오브젝트를 설정합니다.
        /// </summary>
        /// <param name="sketchOutlinePrefab">그릴 오브젝트의 프리팹</param>
        /// <returns>그릴 오브젝트</returns>
        private void SetupSketchObject(SketchOutline sketchOutlinePrefab)
        {
            // 그릴 오브젝트를 생성합니다.
            _currentSketchOutline = Instantiate(sketchOutlinePrefab);
            Assert.IsNotNull(_currentSketchOutline);
            
            _currentSketchOutline.ProgressLabel = drawingProgressLabel;

            // 그릴 오브젝트를 플레이어의 시야에 배치합니다.
            StartCoroutine(PlaceToLeftController());
            
            // 그릴 오브젝트가 그려졌을 때 얻을 오브젝트를 획득하게 합니다.
            _currentSketchOutline.DrawingCompleted += ObtainObject;

            IEnumerator PlaceToLeftController()
            {
                // 시작 프레임에서 바로 배치하면 트래킹되지 않아 제대로 배치되지 않으므로 한 프레임 대기합니다.
                yield return new WaitForEndOfFrame();
                _currentSketchOutline!.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            }
        }
        
        /// <summary>
        /// 얻을 오브젝트를 획득합니다.
        /// </summary>
        /// <param name="sketchObject">스케치 오브젝트</param>
        private void ObtainObject(SketchOutline sketchObject)
        {
            // 그릴 오브젝트 위치에 얻을 오브젝트를 생성하고 그릴 오브젝트를 제거합니다.
            _currentObtainedObject = Instantiate(_currentChapterObtainingObjectPrefab, sketchObject.transform.position, Quaternion.identity);
            Assert.IsNotNull(_currentObtainedObject);
            Destroy(sketchObject.gameObject);
            
            // 그려진 선을 제거하고 펜을 비활성화합니다.
            pen.ClearLines();
            pen.gameObject.SetActive(false);
            
            _currentObtainedObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
                
            GuideText.SetText("그림이 그려졌어요!", duration: 2f);
            
            int page = _currentObtainedObject.chapter switch { Chapter1 => 1, Chapter2 => 2, Chapter3 => 3, _ => 0 };
            GuideText.SetText($"그려진 오브젝트를 잡아서 팝업북의 {page}번째 페이지에 배치해 보세요.", setAfter: 3f);
        }
        
        private void EnableCloseEyesToSleepPose()
        {
            closeEyesToSleepPose.transform.gameObject.SetActive(true);
        }

        /// <summary>
        /// 꿈으로 들어가는 조건을 달성합니다.
        /// </summary>
        [Button]
        public void AchieveEnterCondition()
        {
            EnableCloseEyesToSleepPose();

            // 현재 기획: 바로 자동으로 꿈으로 들어가기
            EnterDream();
        }

        /// <summary>
        /// 꿈 속으로 들어갑니다.
        /// </summary>
        private void EnterDream()
        {
            RoomManager.TryInvokeAction(EnterDream);
        }
        
        /// <summary>
        /// 꿈 속으로 들어가는 과정을 진행 시작합니다.
        /// </summary>
        /// <param name="roomManager">roomManager 레퍼런스</param>
        private void EnterDream(RoomManager roomManager)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                GuideText.SetText("팝업북에 오브젝트가 붙었다.");
                yield return new WaitForSeconds(1f);
                
                roomManager.HidePassthroughEffectMesh = true;
                roomManager.CreateVirtualRoomEffectMeshes();
                roomManager.PopupBookTable.SetActive(true);
                
                yield return new WaitForSeconds(1f);
                
                // 방에서 작아지기
                yield return roomManager.roomEnlarger.EnlargingRoom();
                
                FindAnyObjectByType<OVRPassthroughLayer>().textureOpacity = 0f;
                
                GuideText.SetText("꿈 속으로 들어갑니다...");
                
                // VR 씬 로드
                GameManager.LoadVRScene();
               
                
                // 작아지는 중에 했던 일을 되돌립니다.
                roomManager.HidePassthroughEffectMesh = false;
                roomManager.DestroyVirtualRoomEffectMeshes();
                roomManager.PopupBookTable.SetActive(false);
                roomManager.roomEnlarger.RestoreRoomScale();
            }
        }
    }
}

