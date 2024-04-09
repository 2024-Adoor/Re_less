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

    bool isIdle = false;
    bool isPosition = false;
    public bool isEffectStop = false;

    // VFX 
    public ParticleSystem SleepOutEffect;

    // 진짜 마지막 부분 인사하는 애니메이션 전환
    public GameObject Player;
    public AnimationClip ByeAni;           // IDLE Animation Clip 
    PlayerState _PlayerState;
    bool isBye = false;
    bool isRotate = false;

    void Start()
    {
        SleepOutEffect.Play();
    }

    void Update()
    {
        if(isSleepOut)
        {
            if(!isPosition)
            {
                Debug.Log("Suji teleportation");
                Vector3 newPosition = new Vector3(Target.position.x, 25.5f, Target.position.z);
                gameObject.transform.position = newPosition;
                isPosition = true;
            }
            
            // SleepOut 애니메이션 클립으로 전환
            if (animationComponent != null && SleepOutAni != null && !isChange)
            {
                Debug.Log("Suji SleepOut");
                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();

                isChange = true;
                isIdle = true;
                isSleepOut = false;
            }
        }
        
        if(isIdle & !isEffectStop) 
        {
            StartCoroutine(Wait());
            SleepOutEffect.Stop();
            isEffectStop = true;
        }

        // _PlayerState isTeleport true -> 회전 & 애니메이션 
        if(_PlayerState.isTeleport)
        {
            if(!isRotate)
            {
                transform.Rotate(0, 180, 0);
                isRotate = true;
            }

            // Bye 애니메이션 클립으로 전환
            if (animationComponent != null && ByeAni != null && !isBye)
            {
                Debug.Log("Suji Bye");
                animationComponent.Stop();
                animationComponent.clip = ByeAni;
                animationComponent.Play();

                isBye = true;
            }
        }
    }


    // SleepOut 길이만큼 대기 -> IDLE 재생 
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
