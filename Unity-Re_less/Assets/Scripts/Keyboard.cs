using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public float downwardAmount = 0.3f; // 내려가는 양
    public float downwardSpeed = 0.1f; // 내려가는 속도
    public bool enterDown = false; 

    private bool hasCollided = false; // 충돌 여부 체크
    private Vector3 initialPosition; // 초기 위치 저장

    private void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
    }

    private void Update()
    {
        if (hasCollided)
        {
            // 아래로 이동
            transform.position -= Vector3.up * downwardSpeed * Time.deltaTime;
            enterDown = true;

            // 특정 위치 아래로 이동 완료 시
            if (transform.position.y <= initialPosition.y - downwardAmount)
            {
                hasCollided = false; // 충돌 여부 초기화
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"인 경우
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            // 현재 위치에서 아래로 특정 양만큼만 이동
            //transform.position -= Vector3.up * downwardAmount;
            hasCollided = true; // 충돌 여부 설정
        }
    }
}
