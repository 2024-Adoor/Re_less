using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCat : MonoBehaviour
{
    public Animation animationComponent; // Animation ������Ʈ ����
    public AnimationClip newAnimationClip; // ������ Animation Clip
    public bool isChange = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Animation ������Ʈ�� ���� Animation�� �����ϰ� �� Animation Clip���� ����
            if (animationComponent != null && newAnimationClip != null)
            {
                animationComponent.Stop();
                animationComponent.clip = newAnimationClip;
                animationComponent.Play();
                isChange = true;
            }
        }
    }
}
