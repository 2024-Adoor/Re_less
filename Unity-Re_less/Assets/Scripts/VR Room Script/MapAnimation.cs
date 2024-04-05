using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float upDownSpeed = 1.0f;
    public float upDownRange = 1.0f;

    private float startY;

    void Start()
    {
        // ������Ʈ�� �ʱ� y ��ġ�� ����մϴ�.
        startY = transform.position.y;
    }

    void Update()
    {
        // ���Ʒ����� ������ 
        float newY = startY + Mathf.Sin(Time.time * upDownSpeed) * upDownRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
