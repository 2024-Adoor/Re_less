using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation 컴포넌트 참조
    public GameObject SujiChat;

    public Transform Target;                 // 깨어난 후 이동할 위치 
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public AnimationClip SleepOutAni;       // SleepOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 
    public bool isChange = false;
    public bool isSleepOut = false;

    bool isPosition = false;

    void Start()
    {

    }

    void Update()
    {
        if(isSleepOut)
        {
            if(!isPosition)
            {
                gameObject.transform.position = Target.position;
                isPosition = true;
            }
            
            // SleepOut 애니메이션 클립으로 전환
            if (animationComponent != null && SleepOutAni != null && !isChange)
            {
                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();
                isChange = true;
            }
            StartCoroutine(WaitForSleepOutAnimation());
        }
    }


    // SleepOut 길이만큼 대기 -> IDLE 재생 
    private IEnumerator WaitForSleepOutAnimation()
    {
        yield return new WaitForSeconds(animationComponent.clip.length); // SleepOutAni 애니메이션의 길이만큼 대기
    
        // Animation 이벤트를 통해 호출될 함수 호출
        OnSleepOutAnimationComplete();
    }
    
    // IDLE 클립으로 전환 
    private void OnSleepOutAnimationComplete()
    {
        // IDLE 애니메이션 클립으로 전환
        if (animationComponent != null && IdleAni != null)
        {
            animationComponent.Stop();
            animationComponent.clip = IdleAni;
            animationComponent.Play();
        }
    }
}
