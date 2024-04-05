using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch03_FruitSnap : MonoBehaviour
{   
    public bool isDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CH03_OBJ")) // 충돌한 오브젝트가 CH03_OBJ 태그를 가진 경우
        {
            isDetected = true;

        }
    }
}
