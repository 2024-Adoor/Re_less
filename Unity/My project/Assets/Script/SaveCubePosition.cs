using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCubePosition : MonoBehaviour
{
    private Vector3 lastPosition; // 이전에 저장된 큐브의 위치를 저장하는 변수

    void Start()
    {
        // 초기에 이전 위치를 현재 위치로 설정
        lastPosition = transform.position;
    }

    void Update()
    {
        // 현재 큐브의 위치와 이전 위치를 비교하여 변경되었는지 확인
        Vector3 currentPosition = transform.position;
        if (currentPosition != lastPosition)
        {
            // 변경된 위치를 PlayerPrefs에 저장
            PlayerPrefs.SetFloat("CubePosX", currentPosition.x);
            PlayerPrefs.SetFloat("CubePosY", currentPosition.y);
            PlayerPrefs.SetFloat("CubePosZ", currentPosition.z);
            PlayerPrefs.Save(); // 변경사항 저장

            // 변경된 위치를 이전 위치로 업데이트
            lastPosition = currentPosition;

            // 디버깅용으로 변경된 위치 출력
            Debug.Log("Cube Position Changed: " + currentPosition);
        }
    }
}
