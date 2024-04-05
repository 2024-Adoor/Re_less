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
        // 오브젝트의 초기 y 위치를 기억합니다.
        startY = transform.position.y;
    }

    void Update()
    {
        // 위아래로의 움직임 
        float newY = startY + Mathf.Sin(Time.time * upDownSpeed) * upDownRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
