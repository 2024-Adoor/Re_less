using System.Collections;
using NaughtyAttributes;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

namespace Reless
{
    public class CloseEyesToSleepPose : MonoBehaviour
    {
        /// <summary>
        /// 페이드 지속 시간, 즉 포즈가 활성화되고 꿈 세계로 들어가기 전 까지 걸리는 시간입니다.
        /// </summary>
        [SerializeField] private float fadeDuration;

        /// <summary>
        /// TooCloseSequence의 두 번째 단계(MissingHandEither)로 진행되었을 때 꿈 세계로 들어가기 전 까지 남은 시간입니다.
        /// </summary>
        [SerializeField] private float fadeDurationWhenTooClose;

        /// <summary>
        /// TooCloseSequence의 Sequence 컴포넌트를 참조합니다.
        /// </summary>
        [SerializeField, HideInInspector] private Sequence tooCloseSequence;

        /// <summary>
        /// TooCloseSequence의 Step To Activate 중 MissingHandEither의 인덱스입니다.
        /// </summary>
        private const int MissingHandEitherStep = 1;

        /// <summary>
        /// 페이드 아웃을 시작합니다.
        /// </summary>
        [Button]
        public void StartFadeOut()
        {
            _fadeOut = StartCoroutine(FadeOut());
        }

        private Coroutine _fadeOut;

        /// <summary>
        /// 페이드 아웃을 진행합니다.
        /// </summary>
        private IEnumerator FadeOut()
        {
            float timer = 0f;
            float opacity = 0f;

            while (opacity < 1f)
            {
                timer += Time.deltaTime;
                opacity = Mathf.Clamp01(timer / fadeDuration);
                OVRScreenFade.instance.SetUIFade(opacity);

                yield return null;
            }
        }

        /// <summary>
        /// TooCloseSequence의 두 번째 단계(MissingHandEither)가 진행 중이라면 취소하지 않고, 그렇지 않다면 취소합니다.
        /// 취소하지 않는 이유는 StaticConditions이 비활성화된 이유를 손이 카메라에 너무 가깝기 때문으로 추정하며 이를 보상하기 위함입니다.
        /// </summary>
        public void CancelFadeOutIfNotTooClose()
        {
            if (tooCloseSequence.CurrentActivationStep == MissingHandEitherStep) return;
            CancelFadeOut();
        }

        /// <summary>
        /// 페이드 아웃을 취소합니다.
        /// </summary>
        public void CancelFadeOut()
        {
            // 페이드 아웃 코루틴을 중지합니다.
            if (_fadeOut != null)
            {
                StopCoroutine(_fadeOut);
            }

            // 페이드 값을 0으로 설정합니다.
            OVRScreenFade.instance.SetUIFade(0f);
        }

        [Button]
        public void CloseEyes()
        {


            if (_fadeOut != null)
            {
                StopCoroutine(_fadeOut);
            }

            StartCoroutine(LoadScene());

            IEnumerator LoadScene()
            {
                OVRScreenFade.instance.SetUIFade(1f);

                yield return null;

                var asyncLoad = GameManager.LoadVRScene();

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}