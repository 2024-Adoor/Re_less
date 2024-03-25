using System.Reflection;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

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

    private bool _inFadeOut;
    private float _opacity;
    private float _timer;
    private float _timerForTooClose;
    
    /// <summary>
    /// TooCloseSequence의 Step To Activate 중 MissingHandEither의 인덱스입니다.
    /// </summary>
    private const int MissingHandEitherStep = 1;
    
    private void OnEnable()
    {
        _inFadeOut = false;
        _opacity = 0f;
        _timer = 0f;
        _timerForTooClose = 0f;
    }

    private void Update()
    {
        if (_inFadeOut)
        {
            // tooCloseSequence에서 MissingHandEither 단계로 진행 중인 경우에는 페이드 관련 연산을 fadeDurationWhenTooClose로 수행합니다.
            if (tooCloseSequence.CurrentActivationStep == MissingHandEitherStep)
            {
                _timerForTooClose += Time.deltaTime;
                _opacity = Mathf.Clamp01(_timerForTooClose / fadeDurationWhenTooClose);
            }
            else
            {
                _timer += Time.deltaTime;
                _opacity = Mathf.Clamp01(_timer / fadeDuration);
            }
            
            // TODO: 페이드 아웃 효과에 opacity 값을 적용합니다.
        }
        
    }

    /// <summary>
    /// 페이드 아웃을 시작합니다.
    /// </summary>
    public void StartFadeOut()
    {
        _inFadeOut = true;
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
        _inFadeOut = false;
        _opacity = 0f;
        _timer = 0f;
        _timerForTooClose = 0f;
    }
    
    public void CloseEyes()
    {
        // TODO: 꿈 세계 (VR)로 전환 코드
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var normalSequence = transform.Find("NormalSequence").GetComponent<Sequence>();
        tooCloseSequence ??= transform.Find("TooCloseSequence").GetComponent<Sequence>();

        // 인스펙터에서 fadeDuration과 fadeDurationWhenTooClose가 바뀔 때 그에 맞추어 시퀀스의 Max Step Time을 조정합니다.
        // NOTE: Sequence.InjectOptionalStepsToActivate 메서드를 사용할 수도 있었지만 이상하게도 Sequence.ActivationStep 클래스는
        //       생성자를 통해 생성하면 Active State가 인스펙터에 표시되지 않아 부득이하게 Reflection을 사용합니다.
        {
            // normalSequence
            {
                FieldInfo stepToActivateInfo =
                    typeof(Sequence).GetField("_stepsToActivate", BindingFlags.NonPublic | BindingFlags.Instance);
                var stepToActivateValue = stepToActivateInfo?.GetValue(normalSequence) as Sequence.ActivationStep[];

                FieldInfo maxStepTimeInfo =
                    typeof(Sequence.ActivationStep).GetField("_maxStepTime",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                maxStepTimeInfo?.SetValue(stepToActivateValue?[0], fadeDuration);
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
