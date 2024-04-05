using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOBJ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±װ� "CH02_OBJ"�� ���
        if (other.gameObject.CompareTag("CH02_OBJ"))
        {
            // �浹�� ������Ʈ ����
            Destroy(other.gameObject);
            // Debug.Log("Trigger detected");
        }
    }
}
