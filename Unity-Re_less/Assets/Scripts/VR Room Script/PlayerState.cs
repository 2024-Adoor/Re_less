using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless;

public class PlayerState : MonoBehaviour
{   
    // Fruit 카운트 & 타 캐릭터 상호작용 관리 스크립트입니다. 
    public int fruitCount; 
    public bool isTrigger = false;
    public bool isCharacter = false;
    
    // Ending 요건
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;
    
    public float upwardSpeed = 1f;

    public bool canEnd = false;
    public bool isYUp = false;

    public GameObject DeleteCharacters;

    public GameObject Suji_Surprised;
    public GameObject Characters_Surprised;
    bool isSurprised = false;
    public bool isTeleport = false;
    
    // Ending RespawnTrigger & SpawnPoint 
    public Transform RespawnTrigger;
    public Transform EndSpawnPoint;
    public GameObject Camera;

    // Delay 관리
    private float elapsedTime = 0f;
    private float delayTime = 2f;
    private bool isDelayedActionStarted = false;

    // UI 트리거 관리
    public bool isJumpUI = false;
    public bool isFriendUI = false;
    public bool isDoorUI = false;
    public bool isCh02JumpUI = false;

    // 효과음 관리
    public AudioClip fruit_get;
    private AudioSource audioSource;

    // EndTrigger 머테리얼
    public Material EndMaterial;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        // 엔딩요건이 충족되면 Delay 후 isYUp true 
        if(canEnd)
        {
            if (!isDelayedActionStarted)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    // 딜레이가 종료되면 실행할 코드
                    Debug.Log("Delay Finish");
                    // Rigidbody의 Use Gravity를 false로 변경
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                        isYUp = true;
                    }
                    isDelayedActionStarted = true;
                }
            }
        }

        // isYUp -> 플레이어 y값 상승 & 캐릭터 애니메이션 변경 
        if(isYUp)
        {
            // 현재 위치에서 일정한 속도로 이동
            MoveToTargetY(RespawnTrigger);

            if(!isSurprised)
            {
                // 애들 프리팹 변경 (IDLE -> Surprised)
                Destroy(Suji);
                // Destroy(DeleteCharacters);

                // 새로운 프리팹 생성
                GameObject newSuji = Instantiate(Suji_Surprised, Suji_Surprised.transform.position, Suji_Surprised.transform.rotation);
                GameObject newCharacters = Instantiate(Characters_Surprised, Characters_Surprised.transform.position, Characters_Surprised.transform.rotation);

                isSurprised = true;
            }
        }

        if(Mathf.Approximately(transform.position.y, RespawnTrigger.position.y))
        {
            isYUp = false;
            canEnd = false;
            Debug.Log("Player on RespawnTrigger !!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }

            transform.position = EndSpawnPoint.position + new Vector3(0f, 23f, 0f);

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleCollider.radius = 5f;
            capsuleCollider.height = 60f;

            Camera.transform.position += new Vector3(0f, 20f, 0f);

            PlayerControl _PlayerControl = GetComponent<PlayerControl>();
            if(_PlayerControl != null)
            {
                _PlayerControl.speed = 12f;
            }

            isTeleport = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(Suji != null)
           _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(other.CompareTag("Fruit"))
        {
            fruitCount++;

            // 충돌한 오브젝트 삭제
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");

            audioSource.PlayOneShot(fruit_get);
        }
        else if(other.CompareTag("EndTrigger") && _SujiEndingTest.RotateFin)
        {
            Renderer otherRend = other.GetComponent<Renderer>();

            otherRend.material = EndMaterial;

            canEnd = true;
        }

        if(other.gameObject.name == "UI_JumpTutorial_Trigger")
        {
            isJumpUI = true;
        }
        else if(other.gameObject.name == "UI_FriendTutorial_Trigger")
        {
            isFriendUI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Door_Trigger (1)" || other.gameObject.name == "UI_Ch02Door_Trigger (2)")
        {
            isDoorUI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Jump_Trigger")
        {
            isCh02JumpUI = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && fruitCount > 0)
        {
            isCharacter = true;
        }
    }

    // 타겟위치까지 이동 
    void MoveToTargetY(Transform target)
    {   
        // y축 방향으로의 거리 계산
        float distanceToTargetY = Mathf.Abs(target.position.y - transform.position.y);
    
        // 이동하는데 필요한 시간 계산
        float timeToReachTargetY = distanceToTargetY / upwardSpeed;
    
        // 목표 지점까지 일정한 속도로 y축 이동
        float newY = Mathf.MoveTowards(transform.position.y, target.position.y, upwardSpeed * Time.deltaTime);
        
        // 현재 x와 z 위치를 유지한 채로 y값을 갱신하여 새로운 위치 설정
        Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
        
        // 새로운 위치로 이동
        transform.position = newPosition;
    }
}
