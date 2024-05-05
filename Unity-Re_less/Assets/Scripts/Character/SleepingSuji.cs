using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSuji : MonoBehaviour
{
    public Animation animationComponent;    // Animation 컴포넌트 참조

    public GameObject KeyboardEnter;        // Keyboard
    public GameObject Ch03Fruit;
    public GameObject SujiPrefab;

    public AnimationClip SleepOutAni;       // SleepOut Animation Clip

    public AnimationClip JumpInAni;         // JumpIn Animation Clip
    public AnimationClip JumpOutAni;        // JumpOut Animation Clip

    public bool isSleepOut = false;
    public bool isDetected = false;

    // 애니메이션 제어
    bool isChange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard _Keyboard = KeyboardEnter.GetComponent<Keyboard>();
        SujiManage _SujiManage = SujiPrefab.GetComponent<SujiManage>();

        // 열매 부딪혔을때 SleepOut 애니메이션 클립으로 전환 
        if(isDetected && _Keyboard.enterDown)
        {
            // SleepOut 애니메이션 클립으로 전환
            if (animationComponent != null && SleepOutAni != null && !isChange)
            {
                Debug.Log("Suji SleepOut");
                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();

                isChange = true;
                isSleepOut = false;
            }

            // _SujiManage.isSleepOut = true;
            // Destroy(gameObject);
            Destroy(Ch03Fruit);

            Debug.Log("Suji is SleepOut true - SleepingSuji");
        }

        // 잠에서 깨는 애니메이션 끝났을 때 점프


    }


    private void OnTriggerEnter(Collider other)
    {   
        // 수지한테 Fruit 태그 충돌시 isDetected = true
        if (other.gameObject.CompareTag("Fruit"))
        {   
            isDetected = true;
        }
    }
}
