using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation 컴포넌트 참조
    public GameObject SujiChat;

    public AnimationClip JumpInAni;         // JumpIn Animation Clip
    public AnimationClip JumpOutAni;        // JumpOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 

    // 점프 + 이동
    public Transform target;                // 이동 목적지
    public float rotationSpeed = 5f;        // 회전 속도
    public float moveSpeed = 5f;            // 이동 속도
    Quaternion targetRotation;

    public Transform JumpTarget;            // 점프 목적지? 

    public bool isChange = false;
    public bool isSleepOut = false;
    bool isDelay = true;
    bool isJump = false;
    bool isMove = false;
    bool isMoveFin = false;

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
        gameObject.SetActive(false);
        SleepOutEffect.Play();
    }

    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();

        // SleepOut 애니메이션 끝나면 이펙트 종료 & 키보드 앞으로 점프 + 이동
        if (animationComponent.IsPlaying("SleepOut") == false && !isChange)
        {
            if(isDelay)
            {
                // 딜레이 n초 진행 후 else 코드 실행 
                StartCoroutine(Wait()); // Wait() 코루틴을 시작합니다.
            }
            else
            {
                // 이펙트 중지
                SleepOutEffect.Stop();

                // 회전
                if (!isRotate)
                {
                    // 회전을 하나만 수행하고 이후에는 더 이상 회전하지 않도록 isRotate를 true로 설정합니다.
                    isRotate = true;
                
                    // 목표 회전 각도 설정
                    targetRotation = Quaternion.Euler(0, -120, 0);
                }

                // 회전 로직
                if (isRotate)
                {
                    // Lerp 함수를 사용하여 부드럽게 회전합니다.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    
                    // 현재 회전 각도와 목표 회전 각도가 거의 같으면 회전을 완료했다고 가정하고 isRotate를 false로 설정
                    if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                    {
                        isRotate = false;
                        isMove = true;
                    }
                }

                // 회전 끝남 & 점프와 이동 시작
                if(isJump)
                {
                    //MoveToTarget(JumpTarget);
                }

                if(isMove)
                {
                    MoveNPC(target.position, 2f);
                }
            }
        }
       
        // _PlayerState isTeleport true -> 회전 & 애니메이션 // 플레이어가 방 한가운데 스폰되었을 때
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

    // n초 딜레이
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        isDelay = false;
    }

    // 일정한 시간 동안 서서히 이동시키는 함수
    public void MoveNPC(Vector3 destination, float duration)
    {
        StartCoroutine(MoveCoroutine(destination, duration));
    }

    private System.Collections.IEnumerator MoveCoroutine(Vector3 destination, float duration)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination; // 정확한 목표 위치로 이동
    }
}
