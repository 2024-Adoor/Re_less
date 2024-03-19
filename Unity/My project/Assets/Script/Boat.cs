using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public float shipSpeed = 10f; // 이동 속도

    bool isMovingForward;
    bool isMovingBackward;

     void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭
        {
            MoveForward(); // 이동 시작 함수 호출
        }
        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭
        {
            MoveBackward();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) // 마우스 좌클릭을 떼면
        {
            StopMoving(); // 이동 중지 함수 호출
        }
    }

    void MoveForward()
    {
        isMovingForward = true; 
        isMovingBackward = false; 
    }

    void MoveBackward()
    {
        isMovingForward = false; 
        isMovingBackward = true; 
    }

    void StopMoving()
    {
        isMovingForward = false; 
        isMovingBackward = false;
    }

    void FixedUpdate()
    {   
        float zMove = shipSpeed * Time.deltaTime;

        if (isMovingForward) // 이동 중일 때
        {
            transform.position += new Vector3(0f, 0f, zMove);
        }
        else if (isMovingBackward) // 이동 중일 때
        {
            transform.position += new Vector3(0f, 0f, zMove * -1.0f);
        }
    }

    /*
    void MoveBackward()
    {
        // 뒤로 이동
        float zMove = shipSpeed * Time.deltaTime * -1.0f;
        transform.position += new Vector3(0f, 0f, zMove);
    }
    */
}
