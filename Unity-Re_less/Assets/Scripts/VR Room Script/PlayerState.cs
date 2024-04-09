using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Delay 관리
    private float elapsedTime = 0f;
    private float delayTime = 5f;
    private bool isDelayedActionStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

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

        if(isYUp)
        {
            // 현재 위치에서 목표 y 값 위치까지 일정한 속도로 이동
            transform.position += Vector3.up * upwardSpeed * Time.deltaTime;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(other.CompareTag("Fruit"))
        {
            fruitCount++;

            // 충돌한 오브젝트 삭제
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");
        }
        else if(other.CompareTag("HandTrigger"))
        {
            isTrigger = true;
        }
        else if(other.CompareTag("EndTrigger") && _SujiEndingTest.RotateFin)
        {
            canEnd = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && fruitCount > 0)
        {
            isCharacter = true;
        }
    }
}
