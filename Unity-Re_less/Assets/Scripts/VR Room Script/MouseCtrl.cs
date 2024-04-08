using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour
{
    public Transform ScreenObject; 

    public float maxY = 42.2f; // B ������Ʈ�� �ִ� Y ��ġ
    public float minY = 31.1f; // B ������Ʈ�� �ּ� Y ��ġ
    public float maxZ = -51.9f; // B ������Ʈ�� �ִ� Z ��ġ
    public float minZ = -71.0f; // B ������Ʈ�� �ּ� Z ��ġ

    private Vector3 lastPositionA; // A ������Ʈ�� ������ ��ġ
    private bool collisionDetected = false; // �浹 ���� ���θ� �����ϴ� ����

    public GameObject Ch03_Fruit;

    void Start()
    {
        lastPositionA = transform.position; // �ʱ� ��ġ ����
    }

    void Update()
    {
        if (Ch03_Fruit != null)
        {
            Ch03_FruitSnap Ch03FruitScript = Ch03_Fruit.GetComponent<Ch03_FruitSnap>();
            collisionDetected = Ch03FruitScript.isDetected;

            if (!collisionDetected) // �浹�� �������� ���� ��쿡�� ������ ó��
            {
                Vector3 currentPositionA = transform.position; // A ������Ʈ�� ���� ��ġ

                // A ������Ʈ�� �̵��� ���
                Vector3 displacement = currentPositionA - lastPositionA;

                // B ������Ʈ�� ��ġ�� �̵����� ���� ���� (Z ���� ����)
                Vector3 newPositionB = ScreenObject.position;
                
                newPositionB.z += displacement.z;
                newPositionB.y += displacement.x; // Y ���� Z ��ȭ�� ����
                newPositionB.y = Mathf.Clamp(newPositionB.y, minY, maxY); // Y ��ġ�� �ִ� �� �ּ� ������ ����
                newPositionB.z = Mathf.Clamp(newPositionB.z, minZ, maxZ); // Z ��ġ�� �ִ� �� �ּ� ������ ����
                ScreenObject.position = newPositionB;

                lastPositionA = currentPositionA; // ���� ��ġ ����
            } 
        }
        
    }
}