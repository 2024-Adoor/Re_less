using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;
    public float minY = 0.0f; // y���� �ּҰ�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        // ���콺�� Ŭ������ �� ȣ���
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        rb.freezeRotation = true; // ȸ�� ����
    }

    void OnMouseUp()
    {
        // ���콺���� ���� ���� �� ȣ���
        isDragging = false;
        rb.freezeRotation = false; // ȸ�� ���
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = Mathf.Max(targetPos.y, minY); // y���� minY���� ���� ��� minY�� ����
            rb.MovePosition(targetPos);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        // ���콺 �������� ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
