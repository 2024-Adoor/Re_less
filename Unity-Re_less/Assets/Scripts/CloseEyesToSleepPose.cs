using System.Collections;
using System.Reflection;
using NaughtyAttributes;
using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseEyesToSleepPose : MonoBehaviour
{
    /// <summary>
    /// 페이드 지속 시간, 즉 포즈가 활성화되고 꿈 세계로 들어가기 전 까지 걸리는 시간입니다.
    /// </summary>
    [SerializeField]
    private float fadeDuration;
    
    /// <summary>
    /// TooCloseSequence의 두 번째 단계(MissingHandEither)로 진행되었을 때 꿈 세계로 들어가기 전 까지 남은 시간입니다.
    /// </summary>
    [SerializeField]
    private float fadeDurationWhenTooClose;

    /// <summary>
    /// TooCloseSequence의 Sequence 컴포넌트를 참조합니다.
    /// </summary>
    [SerializeField, HideInInspector]
    private Sequence tooCloseSequence;

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
        StopCoroutine(_fadeOut);
        
        // 페이드 값을 0으로 설정합니다.
        OVRScreenFade.instance.SetUIFade(0f);
    }
    
    [Button]
    public void CloseEyes()
    {
        StopCoroutine(_fadeOut);
        StartCoroutine(LoadScene());
        
        IEnumerator LoadScene()
        {
            OVRScreenFade.instance.SetUIFade(1f);

            yield return null;

            var asyncLoad =  SceneManager.LoadSceneAsync("VR Room");

            while (!asyncLoad.isDone)
            {
               yield return null;
            }
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var normalSequence = transform.Find("NormalSequence").GetComponent<Sequence>();
        tooCloseSequence ??= transform.Find("TooCloseSequence").GetComponent<Sequence>();

        // 인스펙터에서 fadeDuration과 fadeDurationWhenTooClose가 바뀔 때 그에 맞추어 시퀀스를 조정합니다.
        // NOTE: Sequence.InjectOptionalStepsToActivate 메서드를 사용할 수도 있었지만 이상하게도 Sequence.ActivationStep 클래스는
        //       생성자를 통해 생성하면 Active State가 인스펙터에 표시되지 않아 부득이하게 Reflection을 사용합니다.
        {
            // normalSequence
            {
                FieldInfo stepToActivateInfo =
                    typeof(Sequence).GetField("_stepsToActivate", BindingFlags.NonPublic | BindingFlags.Instance);
                var stepToActivateValue = stepToActivateInfo?.GetValue(normalSequence) as Sequence.ActivationStep[];

                FieldInfo minActiveTime =
                    typeof(Sequence.ActivationStep).GetField("_minStepTime",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                minActiveTime?.SetValue(stepToActivateValue?[0], fadeDuration);
            }

            // tooCloseSequence
            {
                FieldInfo stepToActivateInfo =
                    typeof(Sequence).GetField("_stepsToActivate", BindingFlags.NonPublic | BindingFlags.Instance);
                var stepToActivateValue = stepToActivateInfo?.GetValue(tooCloseSequence) as Sequence.ActivationStep[];

                FieldInfo maxStepTimeInfo =
                    typeof(Sequence.ActivationStep).GetField("_maxStepTime",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                maxStepTimeInfo?.SetValue(stepToActivateValue?[0], fadeDuration);
                maxStepTimeInfo?.SetValue(stepToActivateValue?[1], fadeDurationWhenTooClose);
            }
        }
    }
#endif
}