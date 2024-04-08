using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation ������Ʈ ����
    public GameObject Player;               // Player

    public AnimationClip SleepOutAni;       // SleepOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 
    public bool isChange = false;

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {   
        PlayerState _PlayerState = Player.GetComponent<PlayerState>();

        if (collision.gameObject.CompareTag("Player"))
        {   
            // Animation ������Ʈ�� ���� Animation�� �����ϰ� �� Animation Clip���� ����
            if (animationComponent != null && SleepOutAni != null && _PlayerState.fruitCount > 0)
            {
                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();
                isChange = true;
                _PlayerState.fruitCount--;

                StartCoroutine(WaitForSleepOutAnimation());
            }
        }
    }

    private IEnumerator WaitForSleepOutAnimation()
    {
        yield return new WaitForSeconds(animationComponent.clip.length); // SleepOutAni �ִϸ��̼��� ���̸�ŭ ���
    
        // Animation �̺�Ʈ�� ���� ȣ��� �Լ� ȣ��
        OnSleepOutAnimationComplete();
    }
    
    // Animation �̺�Ʈ���� ȣ��� �Լ�
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
