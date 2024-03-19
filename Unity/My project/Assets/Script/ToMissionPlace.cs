using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMissionPlace : MonoBehaviour
{
    public Transform targetPlace; // 이동할 장소의 Transform 참조 변수

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 "Player" 태그를 가지고 있다면
        if (other.CompareTag("Player"))
        {
            // 충돌이 발생했다는 로그 출력
            Debug.Log("Player collided!");

            // 플레이어를 지정된 장소로 이동
            other.transform.position = targetPlace.position;
        }
    }
}
