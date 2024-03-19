using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public float shipSpeed = 10f; // �̵� �ӵ�

    bool isMovingForward;
    bool isMovingBackward;

     void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ��Ŭ��
        {
            MoveForward(); // �̵� ���� �Լ� ȣ��
        }
        if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ��
        {
            MoveBackward();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) // ���콺 ��Ŭ���� ����
        {
            StopMoving(); // �̵� ���� �Լ� ȣ��
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

        if (isMovingForward) // �̵� ���� ��
        {
            transform.position += new Vector3(0f, 0f, zMove);
        }
        else if (isMovingBackward) // �̵� ���� ��
        {
            transform.position += new Vector3(0f, 0f, zMove * -1.0f);
        }
    }

    /*
    void MoveBackward()
    {
        // �ڷ� �̵�
        float zMove = shipSpeed * Time.deltaTime * -1.0f;
        transform.position += new Vector3(0f, 0f, zMove);
    }
    */
}
