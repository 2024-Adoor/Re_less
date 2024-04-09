using System.Collections;
using System.Linq;
using Reless.MR;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reless
{
    /// <summary>
    /// 게임 오프닝을 담당합니다.
    /// </summary>
    public class OpeningBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 벽이 열릴 때의 애니메이션의 커브
        /// </summary>
        [SerializeField] 
        private AnimationCurve wallOpening;

        /// <summary>
        /// 최초로 벽이 열릴 때의 애니메이션의 커브
        /// </summary>
        [SerializeField] private AnimationCurve wallOpeningFirst;

        private GameObject _openingWall;

        private GameObject _pivot;
        
        [SerializeField, HideInInspector]
        private GameManager gameManager;

        private void Start()
        {
            SetupOpeningWallPivot();
            StartCoroutine(StartRoutine());
            
            IEnumerator StartRoutine()
            {
                yield return LoadingOpeningScene();
                yield return null;
                
                var openingScene = SceneManager.GetSceneByName("Opening");
                var rootGameObjects = openingScene.GetRootGameObjects();
                var rootGameObject = rootGameObjects.First(go => go.name == "Root");
                RedundantResolve(rootGameObjects);
                TransformOpeningScene(rootGameObject);
                
                yield return new WaitForSeconds(4f);
                yield return RotatingOpeningWall(wallOpeningFirst);
                yield return new WaitForSeconds(4f);
                yield return RotatingOpeningWall(wallOpening);
                yield return new WaitForSeconds(4f);
                yield return RotatingOpeningWall(wallOpening);
                yield return new WaitForSeconds(4f);
                yield return RotatingOpeningWall(wallOpening);
                yield return new WaitForSeconds(4f);
                
                // 오프닝 씬 언로드
                SceneManager.UnloadSceneAsync("Opening");
                
                // 책을 만지면 ~~ 책을 펼치면 등등 (생략)
                
                // 패스스루 이펙트 메쉬 숨기기
                RoomManager.Instance.HidePassthroughEffectMesh = true;
                RoomManager.Instance.CreateVirtualRoomEffectMeshes();
                
                yield return new WaitForSeconds(2f);
                
                // 방에서 작아지기
                Destroy(rootGameObject);
                RoomManager.Instance.roomEnlarger.EnlargeRoom();
                
                // 튜토리얼 생략
                
                yield return new WaitForSeconds(4f);
                gameManager.LoadMainScene();
                gameManager.CurrentPhase = GameManager.Phase.Chapter1;
                
                // 오프닝에서 했던 일을 되돌립니다.
                ResetTransformOpeningWall();
                RoomManager.Instance.HidePassthroughEffectMesh = false;
                RoomManager.Instance.DestroyVirtualRoomEffectMeshes();
                RoomEnlarger.RestoreRoomScale();
            }
        }
        
        private IEnumerator LoadingOpeningScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("Opening", LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            
            yield return new WaitForSeconds(1f);
            
            yield return DarkenPassthrough();
            
            asyncLoad.allowSceneActivation = true;
        }
        
        private void TransformOpeningScene(GameObject rootGameObject)
        {
            var keyWall = RoomManager.Instance.KeyWall;
            rootGameObject.transform.parent = keyWall.transform;
            rootGameObject.transform.localPosition = new Vector3(0, -keyWall.GetAnchorSize().y / 2, 0);
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
        
        private void SetupOpeningWallPivot()
        {
            _openingWall = RoomManager.Instance.OpeningWall;
            _pivot = new GameObject("Opening Wall Pivot")
            {
                transform =
                {
                    parent = _openingWall.transform,
                    localPosition = new Vector3(RoomManager.Instance.KeyWall.GetAnchorSize().x / 2, 0, 0)
                }
            };
        }

        private IEnumerator DarkenPassthrough()
        {
            const float duration = 4f;
            const float darkenBrightness = -0.5f;
            const float darkenContrast = -0.5f;
            const float darkenSaturation = -0.8f;
            
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                gameManager.passthroughLayer.SetBrightnessContrastSaturation(
                    brightness: Mathf.Lerp(0, darkenBrightness, timer / duration),
                    contrast: Mathf.Lerp(0, darkenContrast, timer / duration),
                    saturation: Mathf.Lerp(0, darkenSaturation, timer / duration)
                );
                yield return null;
            }
        }

        private IEnumerator RotatingOpeningWall(AnimationCurve curve)
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

        private void OnValidate()
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}