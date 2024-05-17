using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Reless;
using Reless.VR;

public class Cat_AniManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation 컴포넌트 참조
    public GameObject Player;               // Player
    ChapterControl _ChapterControl;
    public bool isSleepOut = false;

    // VFX
    public ParticleSystem SleepingEffect;
    public ParticleSystem SleepOutEffect;

    public AnimationClip SleepOutAni;       // SleepOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 
    public bool isChange = false;

    // Sound
    public AudioClip HealingSound;
    private AudioSource HealingAudioSource;

    // eyes
    public GameObject eyes1;
    public GameObject eyes2;
    
    private PlayerState _playerState;

    void Start()
    {
        HealingAudioSource = GetComponent<AudioSource>();
        _ChapterControl = Player.GetComponent<ChapterControl>();

        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            SleepingEffect.Play();
        }
        SleepOutEffect.Stop();

        eyes1.SetActive(false);
        eyes2.SetActive(false);
        
        _playerState = Player.GetComponent<PlayerState>();
    }

    void Update()
    {

        if(isSleepOut)
        {
            // Animation 컴포넌트의 현재 Animation을 중지하고 새 Animation Clip으로 변경
            if (animationComponent != null && SleepOutAni != null && _playerState.FruitCount > 0 && !isChange)
            {
                SleepingEffect.Stop();
                SleepOutEffect.Play();

                HealingAudioSource.PlayOneShot(HealingSound);

                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();
                eyes1.SetActive(true);
                eyes2.SetActive(true);
                isChange = true;

                StartCoroutine(WaitForSleepOutAnimation());

                _playerState.FruitCount--;
            }
        }
    }

    private IEnumerator WaitForSleepOutAnimation()
    {
        yield return new WaitForSeconds(animationComponent.clip.length); // SleepOutAni 애니메이션의 길이만큼 대기
    
        // Animation 이벤트를 통해 호출될 함수 호출
        OnSleepOutAnimationComplete();
    }
    
    // Animation 이벤트에서 호출될 함수
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
