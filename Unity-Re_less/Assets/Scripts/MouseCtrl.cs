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
        
        newPositionB.x += displacement.x;
        newPositionB.y += displacement.z; // Y ���� Z ��ȭ�� ����
        Cursor.position = newPositionB;

        // B ������Ʈ�� ��ġ�� �̵����� ���� ���� (Z ���� ����)
        Vector3 newPositionC = Fruit.position;
        
        newPositionC.x += displacement.x;
        newPositionC.y += displacement.z; // Y ���� Z ��ȭ�� ����
        Fruit.position = newPositionC;

        lastPositionA = currentPositionA; // ���� ��ġ ����
    }
}
