using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation ������Ʈ ����
    public GameObject SujiChat;

    public Transform Target;                 // ��� �� �̵��� ��ġ 
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

    // ��¥ ������ �κ� �λ��ϴ� �ִϸ��̼� ��ȯ
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
            
            // SleepOut �ִϸ��̼� Ŭ������ ��ȯ
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

        // _PlayerState isTeleport true -> ȸ�� & �ִϸ��̼� 
        if(_PlayerState.isTeleport)
        {
            if(!isRotate)
            {
                transform.Rotate(0, 180, 0);
                isRotate = true;
            }

            // Bye �ִϸ��̼� Ŭ������ ��ȯ
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


    // SleepOut ���̸�ŭ ��� -> IDLE ��� 
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
