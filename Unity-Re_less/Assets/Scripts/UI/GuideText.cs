using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Reless.Debug.Logger;

namespace Reless.UI
{
    /// <summary>
    /// MainScene에서 사용하는 플레이어 카메라 UI 가이드 텍스트입니다.
    /// </summary>
    public class GuideText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        private static GuideText instance;
        
        private static GuideText Instance 
        {
            get => instance ??= FindAnyObjectByType<GuideText>() ?? throw new MissingComponentException("GuideText is not found.");
            set => instance = value;
        }
        
        [CanBeNull]
        private Coroutine _currentCoroutine;
        
        private void Awake()
        {
            if (Instance != this)
            {
                Logger.LogError("GuideText is already exists.");
                return;
            }
            ClearText();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            Instance = null;
        }

        /// <summary>
        /// 텍스트를 설정합니다.
        /// </summary>
        /// <param name="text">설정할 텍스트</param>
        /// <param name="duration">텍스트가 유지될 시간</param>
        /// <param name="setAfter">이 시간이 지난 후 텍스트를 설정합니다.</param>
        public static void SetText(string text, float duration = float.PositiveInfinity, float? setAfter = null)
        {
            if (setAfter is not null)
            {
                Logger.Log($"Setting text after {setAfter.Value} seconds.");
                Instance.StartCoroutine(SetTextAfter(setAfter.Value));
                return;
            }
            
            Instance.text.enabled = true;
            Instance.text.text = text;
            
            if (!float.IsPositiveInfinity(duration))
            {
                if (Instance._currentCoroutine is not null)
                {
                    Instance.StopCoroutine(Instance._currentCoroutine);
                    ClearText();
                }
                
                Instance.text.enabled = true;
                Instance.text.text = text;
                Instance._currentCoroutine = Instance.StartCoroutine(Instance.FadeOutTextAfter(duration));
            }
            
            return;
            
            IEnumerator SetTextAfter(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                if (Instance._currentCoroutine is not null)
                {
                    Logger.LogWarning($"{nameof(GuideText)}: Coroutine is already running. Stopping the current coroutine.");
                    Instance.StopCoroutine(Instance._currentCoroutine);
                    ClearText();
                }
                
                SetText(text, duration);
            }
        }
        
        private IEnumerator FadeOutTextAfter(float duration)
        {
            yield return new WaitForSeconds(duration);
            yield return FadeOutText();
        }
        
        private IEnumerator FadeOutText()
        {
            const float fadeOutDuration = 1f;
            
            for (float time = 0; time < fadeOutDuration; time += Time.deltaTime)
            {
                Instance.text.alpha = AnimationCurve.EaseInOut(0, 1, fadeOutDuration, 0).Evaluate(time);
                yield return null;
            }
            
            ClearText();
        }

        public static void StartFadeOutText(float after = 0)
        {
            if (Instance._currentCoroutine is not null)
            {
                Instance.StopCoroutine(Instance._currentCoroutine);
                ClearText();
            }
                
            Instance._currentCoroutine = Instance.StartCoroutine(Instance.FadeOutTextAfter(after));
        }
        
        public static void ClearText()
        {
            Instance.text.text = string.Empty;
            Instance.text.enabled = false;
            Instance.text.alpha = 1;
            Instance._currentCoroutine = null;
        }
    }
}
