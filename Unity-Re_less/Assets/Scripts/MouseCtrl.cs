using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour
{
    public Transform Cursor; // Cursor Transform
    public Transform Fruit; // Fruit Transform
    private Vector3 lastPositionA; // A ������Ʈ�� ������ ��ġ

    void Start()
    {
        lastPositionA = transform.position; // �ʱ� ��ġ ����
    }

    void Update()
    {
        Vector3 currentPositionA = transform.position; // A ������Ʈ�� ���� ��ġ

        // A ������Ʈ�� �̵��� ���
        Vector3 displacement = currentPositionA - lastPositionA;

        // B ������Ʈ�� ��ġ�� �̵����� ���� ���� (Z ���� ����)
        Vector3 newPositionB = Cursor.position;
        
        newPositionB.z += displacement.z;
        newPositionB.y += displacement.x; // Y ���� Z ��ȭ�� ����
        Cursor.position = newPositionB;

        // B ������Ʈ�� ��ġ�� �̵����� ���� ���� (Z ���� ����)
        Vector3 newPositionC = Fruit.position;
        
        newPositionC.z += displacement.z;
        newPositionC.y += displacement.x; // Y ���� Z ��ȭ�� ����
        Fruit.position = newPositionC;

        lastPositionA = currentPositionA; // ���� ��ġ ����
    }
}
