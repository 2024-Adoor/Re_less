using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using Reless.MR;
using Reless.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Logger = Reless.Debug.Logger;
using SceneManager = Reless.Util.SceneManager;

namespace Reless.Opening
{
    /// <summary>
    /// 게임 오프닝을 불러옵니다.
    /// </summary>
    public class OpeningBootstrapper : MonoBehaviour
    {
        /// <summary>
        /// 오프닝이 진행되는 벽
        /// </summary>
        private GameObject _openingWall;

        /// <summary>
        /// 오프닝 벽이 회전할 중심 피벗
        /// </summary>
        private GameObject _pivot;
        
        /// <summary>
        /// OpeningAnimator 레퍼런스
        /// </summary>
        private OpeningAnimator _openingAnimator;
        
        /// <summary>
        /// 오프닝 중인지 여부
        /// </summary>
        [ShowNativeProperty]
        private bool IsInOpening { get; set; }

        private void Awake()
        {
            OnOpening(GameManager.CurrentPhase);
            GameManager.PhaseChanged += OnOpening;
        }

        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnOpening;
        }
        
        private void OnOpening(GamePhase phase)
        {
            if (phase is not GamePhase.Opening) return;
            
            BootstrapOpening();
        }

        /// <summary>
        /// 오프닝을 시작합니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void BootstrapOpening()
        {
            if (IsInOpening)
            {
                Logger.LogWarning($"{nameof(OpeningBootstrapper)}: already in opening");
                return;
            }
            IsInOpening = true;
            Logger.Log($"{nameof(OpeningBootstrapper)}: BootstrapOpening");
            
            // 오프닝 벽의 피벗을 설정합니다.
            SetupOpeningWallPivot();
            
            StartCoroutine(StartRoutine());

            IEnumerator StartRoutine()
            {
                // 오프닝 씬을 비동기로 로드하고 바로 활성화하지 않습니다.
                Logger.Log($"{nameof(OpeningBootstrapper)}: loading Opening scene...");
                var asyncLoad = SceneManager.LoadAsync(BuildScene.Opening, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = false;
            
                // 1초 기다리고 패스스루를 어둡게 합니다.
                yield return new WaitForSeconds(1f);
                yield return DarkenPassthrough();
            
                // 오프닝 씬을 활성화합니다.
                asyncLoad.allowSceneActivation = true;
                Logger.Log($"{nameof(OpeningBootstrapper)}: Opening scene loaded");
                yield return null;
                
                // 룸을 오프닝 씬이 보여지도록 변환합니다.
                TransformToOpeningScene();
                
                // 이 시점에 플레이어가 뒤를 보고 있다면
                if (GameManager.EyeAnchor.forward.z < 0)
                {
                    GuideText.SetText("뒤를 돌아보세요.", duration: 3f);
                }
                
                // 2초 기다리고 오프닝 애니메이션을 재생합니다.
                yield return new WaitForSeconds(2);
                _openingAnimator = FindAnyObjectByType<OpeningAnimator>();
                yield return _openingAnimator.Play();
                
                // 애니메이션이 끝나고 3초 기다리고 오프닝 씬을 언로드합니다.
                yield return new WaitForSeconds(3);
                SceneManager.UnloadAsync(BuildScene.Opening);
                Logger.Log($"{nameof(OpeningBootstrapper)}: Opening scene unloaded");
                
                // 책을 만지면 ~~ 책을 펼치면 등등 (생략)
                
                // 방에서 작아지기
                // RoomManager.Instance.roomEnlarger.EnlargeRoom();
                
                // 튜토리얼 생략
                
                yield return new WaitForSeconds(4f);
                GameManager.CurrentPhase = GamePhase.Chapter1;
                GameManager.LoadMainScene();
                
                // 오프닝에서 했던 일을 되돌립니다.
                RoomManager.Instance.RevertRoomTransform();
                ResetTransformOpeningWall();
                
                IsInOpening = false;
            }
        }
        
        /// <summary>
        /// 공간을 오프닝 씬이 보여지도록 상대적으로 변환합니다.
        /// </summary>
        private static void TransformToOpeningScene()
        {
            var roomManager = RoomManager.Instance;
            Assert.IsNotNull(roomManager);
            
            // 오프닝 벽의 중앙에서 하단까지의 길이
            float centerToBottom = default;
            try
            {
                centerToBottom = (roomManager.KeyWall.PlaneRect?.height ?? throw new Exception()) / 2;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
                
            var openingWallBottom = roomManager.KeyWall.transform.position - new Vector3(0, centerToBottom, 0);
            var openingWallBackward = -roomManager.KeyWall.transform.forward;
            
            roomManager.SetRoomTransform(newOrigin: openingWallBottom, newForward: openingWallBackward);
        }

        /// <summary>
        /// 오프닝 벽의 피벗을 설정합니다.
        /// </summary>
        private void SetupOpeningWallPivot()
        {
            _openingWall = RoomManager.Instance.OpeningWall;
            _pivot = new GameObject("Opening Wall Pivot")
            {
                transform =
                {
                    parent = _openingWall.transform,
                    
                    // 벽의 왼쪽 중앙에 피벗을 둡니다.
                    localPosition = new Vector3((RoomManager.Instance.KeyWall.PlaneRect?.width ?? 0) / 2, 0, 0)
                }
            };
            Logger.Log($"{nameof(OpeningBootstrapper)}: set up opening wall pivot: <b>{_pivot.transform.position}</b>");
        }

        /// <summary>
        /// 패스스루를 점점 어둡게 합니다.
        /// </summary>
        private IEnumerator DarkenPassthrough()
        {
            const float duration = 4f;
            const float darkenBrightness = -0.425f;
            const float darkenContrast = -0.5f;
            const float darkenSaturation = -0.8f;
            
            var passthroughLayer = FindAnyObjectByType<OVRPassthroughLayer>();
            
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                passthroughLayer.SetBrightnessContrastSaturation(
                    brightness: Mathf.Lerp(0, darkenBrightness, timer / duration),
                    contrast: Mathf.Lerp(0, darkenContrast, timer / duration),
                    saturation: Mathf.Lerp(0, darkenSaturation, timer / duration)
                );
                yield return null;
            }
        }
        
        public IEnumerator RotatingOpeningWall(AnimationCurve curve)
        {
            // 회전 초기화
            ResetTransformOpeningWall();
                    
            const float targetAngle = -180f;
                    
            for (float timer = 0f; timer < curve.keys.Last().time; timer += Time.deltaTime)
            {
                float angle = targetAngle * curve.Evaluate(timer) - _openingWall.transform.localRotation.eulerAngles.y;
                _openingWall.transform.RotateAround(_pivot.transform.position, Vector3.up, angle);
                yield return null;
            }
        }
        
        private void ResetTransformOpeningWall()
        {
            _openingWall.transform.localPosition = Vector3.zero;
            _openingWall.transform.localRotation = Quaternion.identity;
        }
    }
}