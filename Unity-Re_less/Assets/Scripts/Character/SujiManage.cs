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
            
            // SleepOut �ִϸ��̼� Ŭ������ ��ȯ
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


    // SleepOut ���̸�ŭ ��� -> IDLE ��� 
    private IEnumerator WaitForSleepOutAnimation()
    {
        yield return new WaitForSeconds(animationComponent.clip.length); // SleepOutAni �ִϸ��̼��� ���̸�ŭ ���
    
        // Animation �̺�Ʈ�� ���� ȣ��� �Լ� ȣ��
        OnSleepOutAnimationComplete();
    }
    
    // IDLE Ŭ������ ��ȯ 
    private void OnSleepOutAnimationComplete()
    {
        // IDLE �ִϸ��̼� Ŭ������ ��ȯ
        if (animationComponent != null && IdleAni != null)
        {
            animationComponent.Stop();
            animationComponent.clip = IdleAni;
            animationComponent.Play();
        }
    }
}
