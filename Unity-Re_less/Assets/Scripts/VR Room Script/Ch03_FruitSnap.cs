using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch03_FruitSnap : MonoBehaviour
{   
    public bool isDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CH03_OBJ")) // �浹�� ������Ʈ�� CH03_OBJ �±׸� ���� ���
        {
            isDetected = true;

        }
    }
}
