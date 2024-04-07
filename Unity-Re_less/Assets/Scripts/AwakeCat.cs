using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCat : MonoBehaviour
{
    public Animation animationComponent; // Animation 컴포넌트 참조
    public AnimationClip newAnimationClip; // 변경할 Animation Clip
    public bool isChange = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Animation 컴포넌트의 현재 Animation을 중지하고 새 Animation Clip으로 변경
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
