using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch03_FruitSnap : MonoBehaviour
{   
    public bool isDetected = false;
    public GameObject suji;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CH03_OBJ")) // �浹�� ������Ʈ�� CH03_OBJ �±׸� ���� ���
        {
            isDetected = true;

            Vector3 newPosition = transform.position; // ���� ��ġ
            newPosition.y = suji.transform.position.y; // Y ��ǥ�� suji�� Y ��ǥ�� ����
            newPosition.z = suji.transform.position.z; // Z ��ǥ�� suji�� Z ��ǥ�� ����
            transform.position = newPosition; // ����� ��ġ ����
        }
    }
}
