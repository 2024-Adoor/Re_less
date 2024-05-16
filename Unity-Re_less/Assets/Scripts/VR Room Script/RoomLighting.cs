using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Reless.VR
{
    /// <summary>
    /// VR Room 월드의 라이팅을 관리합니다.
    /// </summary>
    public class RoomLighting : MonoBehaviour
    {
        [Serializable]
        private struct AmbientColor
        {
            [ColorUsage(showAlpha: false, hdr: true)]
            public Color sky, equator, ground;
        
            public AmbientColor(Color sky, Color equator, Color ground)
            {
                this.sky = sky;
                this.equator = equator;
                this.ground = ground;
            }
        }
        
        [Header("Ambient Colors")]
        [SerializeField] 
        private AmbientColor ch01AmbientColor;

        [SerializeField]
        private AmbientColor ch02AmbientColor;
    
        [SerializeField]
        private AmbientColor ch03AmbientColor;
    
        [SerializeField]
        private AmbientColor endingAmbientColor;
        
        [Header("References")]
        [SerializeField]
        private Light endingLight;

        private void Start()
        {
            endingLight.gameObject.SetActive(false);
        }

        /// <summary>
        /// 현재 챕터에 대한 앰비언트 라이팅을 적용합니다.
        /// </summary>
        public void ApplyAmbientColorByChapter(Chapter chapter)
        {
            ApplyAmbientColor(chapter switch
                {
                    Chapter.Chapter1 => ch01AmbientColor,
                    Chapter.Chapter2 => ch02AmbientColor,
                    Chapter.Chapter3 => ch03AmbientColor,
                    _ => throw new ArgumentOutOfRangeException()
                }
            );
            endingLight.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 엔딩에 대한 앰비언트 라이팅을 적용합니다.
        /// </summary>
        public void ApplyEndingAmbientColor() 
        {
            ApplyAmbientColor(endingAmbientColor);
            endingLight.gameObject.SetActive(true);
        }
        
        private void ApplyAmbientColor(AmbientColor color)
        {
            RenderSettings.ambientSkyColor = color.sky;
            RenderSettings.ambientEquatorColor = color.equator;
            RenderSettings.ambientGroundColor = color.ground;
        }

        /// <summary>
        /// 현재 챕터를 받아 다음 챕터의 앰비언트 라이팅으로 전환합니다.
        /// </summary>
        /// <param name="current">현재 챕터</param>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="current"/>가 유효하지 않은 챕터입니다.</exception>
        public void TransitAmbientColorToNext(Chapter current)
        {
            (AmbientColor from, AmbientColor to) ambientPair = current switch
            {
                Chapter.Chapter1 => (ch01AmbientColor, ch02AmbientColor),
                Chapter.Chapter2 => (ch02AmbientColor, ch03AmbientColor),
                Chapter.Chapter3 => (ch03AmbientColor, endingAmbientColor),
                _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
            };

            StartCoroutine(TransitAmbientColor(ambientPair.from, ambientPair.to, 3f));
        }
        
        private IEnumerator TransitAmbientColor(AmbientColor from, AmbientColor to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                var t = elapsed / duration;
                ApplyAmbientColor(new AmbientColor(
                    Color.Lerp(from.sky, to.sky, t),
                    Color.Lerp(from.equator, to.equator, t),
                    Color.Lerp(from.ground, to.ground, t)
                ));
                elapsed += Time.deltaTime;
                yield return null;
            }
            ApplyAmbientColor(to);
        }
        
        
#if UNITY_EDITOR
        [Button]
        private void PreviewCh01() => ApplyAmbientColorByChapter(Chapter.Chapter1);
        
        [Button]
        private void PreviewCh02() => ApplyAmbientColorByChapter(Chapter.Chapter2);
        
        [Button]
        private void PreviewCh03() => ApplyAmbientColorByChapter(Chapter.Chapter3);
        
        [Button]
        private void PreviewEnding() => ApplyEndingAmbientColor();
#endif
    }
}
