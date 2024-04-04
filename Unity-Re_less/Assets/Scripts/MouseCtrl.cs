using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour
{
    public Transform Cursor; // Cursor Transform
    public Transform Fruit; // Fruit Transform
    private Vector3 lastPositionA; // A 오브젝트의 마지막 위치

    void Start()
    {
        lastPositionA = transform.position; // 초기 위치 설정
    }

    void Update()
    {
        Vector3 currentPositionA = transform.position; // A 오브젝트의 현재 위치

        // A 오브젝트의 이동량 계산
        Vector3 displacement = currentPositionA - lastPositionA;

        // B 오브젝트의 위치를 이동량에 따라 조정 (Z 값은 고정)
        Vector3 newPositionB = Cursor.position;
        
        newPositionB.z += displacement.z;
        newPositionB.y += displacement.x; // Y 값에 Z 변화량 적용
        Cursor.position = newPositionB;

        // B 오브젝트의 위치를 이동량에 따라 조정 (Z 값은 고정)
        Vector3 newPositionC = Fruit.position;
        
        newPositionC.z += displacement.z;
        newPositionC.y += displacement.x; // Y 값에 Z 변화량 적용
        Fruit.position = newPositionC;

        lastPositionA = currentPositionA; // 이전 위치 갱신
    }
}
