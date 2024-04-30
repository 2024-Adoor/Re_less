using System.Collections;
using System.Linq;
using Reless.MR;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
            GameManager.OnOpening += StartOpening;
        }

        private void OnDestroy()
        {
            GameManager.OnOpening -= StartOpening;
        }

        /// <summary>
        /// 오프닝을 시작합니다.
        /// </summary>
        private void StartOpening()
        {
            SetupOpeningWallPivot();
            
            StartCoroutine(StartRoutine());
            
            IEnumerator StartRoutine()
            {
                yield return LoadingOpeningScene();
                
                var openingScene = SceneManager.GetScene(BuildScene.Opening);
                var rootGameObjects = openingScene.GetRootGameObjects();
                var rootGameObject = rootGameObjects.First(go => go.name == "Root");
                RedundantResolve(rootGameObjects);
                TransformOpeningScene(rootGameObject);

                yield return new WaitForSeconds(3);
                yield return _openingAnimator.Play();
                yield return new WaitForSeconds(3);
                
                // 오프닝 씬 언로드
                SceneManager.UnloadAsync(BuildScene.Opening);
                
                // 책을 만지면 ~~ 책을 펼치면 등등 (생략)
                
                // 패스스루 이펙트 메쉬 숨기기
                Assert.IsNotNull(RoomManager.Instance);
                RoomManager.Instance.HidePassthroughEffectMesh = true;
                RoomManager.Instance.CreateVirtualRoomEffectMeshes();
                
                yield return new WaitForSeconds(2f);
                
                // 방에서 작아지기
                Destroy(rootGameObject);
                RoomManager.Instance.roomEnlarger.EnlargeRoom();
                
                // 튜토리얼 생략
                
                yield return new WaitForSeconds(4f);
                GameManager.Instance.CurrentPhase = GamePhase.Chapter1;
                GameManager.Instance.LoadMainScene();
                
                // 오프닝에서 했던 일을 되돌립니다.
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
            var asyncLoad = SceneManager.LoadAsync(BuildScene.Opening, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            
            yield return new WaitForSeconds(1f);
            
            yield return DarkenPassthrough();
            
            asyncLoad.allowSceneActivation = true;

            yield return null;
            
            _openingAnimator = FindAnyObjectByType<OpeningAnimator>();
        }
        
        private void TransformOpeningScene(GameObject rootGameObject)
        {
            var keyWall = RoomManager.Instance.KeyWall;
            rootGameObject.transform.parent = keyWall.transform;
            rootGameObject.transform.localPosition = new Vector3(0, (-keyWall.PlaneRect?.height ?? 0) / 2, 0);
            rootGameObject.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        private void RedundantResolve(GameObject[] rootGameObjects)
        {
            foreach (var go in rootGameObjects)
            {
                if (go.GetComponent<Camera>() != null || 
                    go.name == "Directional Light"
                    )
                {
                    Debug.Log($"Destroying {go.name}");
                    Destroy(go);
                }
            }
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