using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Reless.Opening
{
    /// <summary>
    /// 오프닝 애니메이션을 담당합니다.
    /// </summary>
    public class OpeningAnimator : MonoBehaviour
    {
        /// <summary>
        /// 오프닝 애니메이션의 각 장면들
        /// </summary>
        public List<OpeningScene> scenes;
        
        /// <summary>
        /// OpeningBehaviour 레퍼런스
        /// </summary>
        [CanBeNull] 
        private OpeningBehaviour _openingBehaviour;
        
        /// <summary>
        /// 벽이 열릴 때의 애니메이션의 커브
        /// </summary>
        [SerializeField] 
        private AnimationCurve wallOpening;

        /// <summary>
        /// 최초로 벽이 열릴 때의 애니메이션의 커브
        /// </summary>
        [SerializeField] 
        private AnimationCurve wallOpeningFirst;
        
        /// <summary>
        /// 벽이 닫힐 때의 애니메이션의 커브
        /// </summary>
        [SerializeField] 
        private AnimationCurve wallClosing;

        private void Awake()
        {
            // 오프닝 씬 목록에 null이 있어서는 안 됩니다.
            foreach (var scene in scenes) { Assert.IsNotNull(scene); }
        }

        private void Start()
        {
            _openingBehaviour = FindAnyObjectByType<OpeningBehaviour>();

            if (SceneManager.ActiveScene is not BuildScene.Opening)
            {
                Assert.IsNotNull(_openingBehaviour, message: "Opening 씬에서 직접 실행한 것이 아닌 OpeningBehaviour는 null이 아니어야 합니다.");
                
                // 씬 시작 시 오프닝 씬 비활성화 보장
                DeactivateAllScenes();
            }
        }
        
        /// <summary>
        /// 오프닝 애니메이션을 재생합니다.
        /// </summary>
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public IEnumerator Play()
        {
            // 애니메이션 재생 시 오프닝 씬 비활성화 보장
            DeactivateAllScenes();

            // 최초 벽 열림
            yield return _openingBehaviour?.RotatingOpeningWall(wallOpeningFirst);
            
            // 오프닝 씬 반복
            foreach (var scene in scenes)
            {
                // 현재 오프닝 씬 활성화
                SetActiveScene(scene);
                
                // 벽을 넘깁니다.
                yield return _openingBehaviour switch { not null => _openingBehaviour.RotatingOpeningWall(wallOpening),
                    
                    // OpeningBehaviour가 null이라면 테스트용으로 Opening 씬에서 직접 실행한 것입니다. 벽 넘기기 시간을 시뮬레이션합니다.
                    null => new WaitForSeconds(wallOpening.keys.Last().time)
                };
                    
                yield return new WaitForSeconds(scene.duration);
            }
            
            // 벽 닫힘
            yield return _openingBehaviour?.RotatingOpeningWall(wallClosing);
            DeactivateAllScenes();
        }

        /// <summary>
        /// 활성화 오프닝 씬을 설정합니다.
        /// </summary>
        /// <param name="targetScene">대상 씬</param>
        private void SetActiveScene(OpeningScene targetScene)
        {
            DeactivateAllScenes();
            targetScene.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// 모든 오프닝 씬을 비활성화합니다.
        /// </summary>
        private void DeactivateAllScenes() => scenes.ForEach(scene => scene.gameObject.SetActive(false));
    }
}