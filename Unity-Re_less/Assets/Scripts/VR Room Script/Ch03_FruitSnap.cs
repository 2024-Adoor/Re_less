using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch03_FruitSnap : MonoBehaviour
{   
    public bool isDetected = false;
    public GameObject suji;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CH03_OBJ")) // 충돌한 오브젝트가 CH03_OBJ 태그를 가진 경우
        {
            isDetected = true;

            Vector3 newPosition = transform.position; // 현재 위치
            newPosition.y = suji.transform.position.y; // Y 좌표를 suji의 Y 좌표로 변경
            newPosition.z = suji.transform.position.z; // Z 좌표를 suji의 Z 좌표로 변경
            transform.position = newPosition; // 변경된 위치 적용
        }
    }
}
