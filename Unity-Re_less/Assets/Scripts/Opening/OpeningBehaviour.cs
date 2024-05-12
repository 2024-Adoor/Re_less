using System;
using System.Collections;
using System.Linq;
using Reless.MR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Logger = Reless.Debug.Logger;
using SceneManager = Reless.Util.SceneManager;

namespace Reless.Opening
{
    /// <summary>
    /// 게임 오프닝을 담당합니다.
    /// </summary>
    public class OpeningBehaviour : MonoBehaviour
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

        private void Awake()
        {
            GameManager.PhaseChanged += OnOpening;
        }

        private void OnDestroy()
        {
            GameManager.PhaseChanged -= OnOpening;
        }
        
        private void OnOpening(GamePhase phase)
        {
            if (phase is not GamePhase.Opening) return;
            
            StartOpening();
        }

        /// <summary>
        /// 오프닝을 시작합니다.
        /// </summary>
        private void StartOpening()
        {
            Logger.Log($"{nameof(OpeningBehaviour)}: StartOpening");
            
            // 오프닝 벽의 피벗을 설정합니다.
            SetupOpeningWallPivot();
            
            StartCoroutine(StartRoutine());

            IEnumerator StartRoutine()
            {
                // 오프닝 씬 로드
                yield return LoadingOpeningScene();
                
                _openingAnimator = FindAnyObjectByType<OpeningAnimator>();
                
                TransformToOpeningScene();

                yield return new WaitForSeconds(3);
                yield return _openingAnimator.Play();
                yield return new WaitForSeconds(3);
                
                // 오프닝 씬 언로드
                SceneManager.UnloadAsync(BuildScene.Opening);
                Logger.Log($"{nameof(OpeningBehaviour)}: Opening scene unloaded");
                
                // 책을 만지면 ~~ 책을 펼치면 등등 (생략)
                
                // 패스스루 이펙트 메쉬 숨기기
                Assert.IsNotNull(RoomManager.Instance);
                RoomManager.Instance.HidePassthroughEffectMesh = true;
                RoomManager.Instance.CreateVirtualRoomEffectMeshes();
                
                yield return new WaitForSeconds(2f);
                
                // 방에서 작아지기
                RoomManager.Instance.roomEnlarger.EnlargeRoom();
                
                // 튜토리얼 생략
                
                yield return new WaitForSeconds(4f);
                GameManager.CurrentPhase = GamePhase.Chapter1;
                GameManager.LoadMainScene();
                
                // 오프닝에서 했던 일을 되돌립니다.
                RevertTransform();
                ResetTransformOpeningWall();
                RoomManager.Instance.HidePassthroughEffectMesh = false;
                RoomManager.Instance.DestroyVirtualRoomEffectMeshes();
                RoomManager.Instance.roomEnlarger.RestoreRoomScale();
            }
        }
        
        /// <summary>
        /// 오프닝 씬을 로드합니다.
        /// </summary>
        private IEnumerator LoadingOpeningScene()
        {
            Logger.Log($"{nameof(OpeningBehaviour)}: loading Opening scene...");
            
            var asyncLoad = SceneManager.LoadAsync(BuildScene.Opening, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            
            yield return new WaitForSeconds(1f);
            
            yield return DarkenPassthrough();
            
            asyncLoad.allowSceneActivation = true;
            Logger.Log($"{nameof(OpeningBehaviour)}: Opening scene loaded");

            yield return null;
        }
        
        /// <summary>
        /// 공간을 오프닝 씬이 보여지도록 상대적으로 변환합니다.
        /// </summary>
        private static void TransformToOpeningScene()
        {
            var roomManager = RoomManager.Instance;
            Assert.IsNotNull(roomManager);
            
            var room = roomManager.Room.transform;
            
            // 변환 전 룸에 상대적인 트래킹 스페이스 저장
            var initialTrackingSpacePosition = room.InverseTransformPoint(GameManager.CameraRig.trackingSpace.position);
            var initialTrackingSpaceRotation = Quaternion.Inverse(room.rotation) * GameManager.CameraRig.trackingSpace.rotation;
            
            // 방을 기준이 되는 오프닝 벽의 -forward가 월드 forward를 바라보도록 회전
            var targetRotation = Quaternion.LookRotation(-roomManager.KeyWall.transform.forward, Vector3.up);
            room.rotation *= Quaternion.Inverse(targetRotation);

            // 기준이 되는 오프닝 벽의 하단이 월드의 중앙에 오도록 룸을 이동
            Vector3 targetPosition; 
            {
                // 오프닝 벽의 중앙에서 하단까지의 길이
                float centerToBottom = default;
                try { centerToBottom = (roomManager.KeyWall.PlaneRect?.height ?? throw new Exception()) / 2; } catch (Exception e) { Logger.LogException(e); }
                
                // 하단으로 오프셋
                targetPosition = roomManager.KeyWall.transform.position - new Vector3(0, centerToBottom, 0);
            }
            room.position -= targetPosition;
            
            // 트래킹 스페이스를 변환된 룸에 대해서 변환
            GameManager.CameraRig.trackingSpace.position = room.TransformPoint(initialTrackingSpacePosition);
            GameManager.CameraRig.trackingSpace.rotation = room.rotation * initialTrackingSpaceRotation;
        }

        /// <summary>
        /// 공간을 원래대로 되돌립니다.
        /// </summary>
        private void RevertTransform()
        {
            Assert.IsNotNull(RoomManager.Instance);
            
            RoomManager.Instance.Room.transform.position = Vector3.zero;
            RoomManager.Instance.Room.transform.rotation = Quaternion.identity;
            
            GameManager.CameraRig.trackingSpace.position = Vector3.zero;
            GameManager.CameraRig.trackingSpace.rotation = Quaternion.identity;
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
            Logger.Log($"{nameof(OpeningBehaviour)}: set up opening wall pivot: <b>{_pivot.transform.position}</b>");
        }

        private IEnumerator DarkenPassthrough()
        {
            const float duration = 4f;
            const float darkenBrightness = -0.5f;
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