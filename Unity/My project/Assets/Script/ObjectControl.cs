using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;
    public float minY = 0.0f; // y값의 최소값

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        // 마우스로 클릭했을 때 호출됨
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        rb.freezeRotation = true; // 회전 방지
    }

    void OnMouseUp()
    {
        // 마우스에서 손을 뗐을 때 호출됨
        isDragging = false;
        rb.freezeRotation = false; // 회전 허용
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            targetPos.y = Mathf.Max(targetPos.y, minY); // y값이 minY보다 작을 경우 minY로 고정
            rb.MovePosition(targetPos);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        // 마우스 포인터의 위치를 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
