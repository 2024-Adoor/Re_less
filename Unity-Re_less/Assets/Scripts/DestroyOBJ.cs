using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOBJ : MonoBehaviour
{
    // �浹 ó��
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Destroy"�� ���
        if (collision.gameObject.CompareTag("CH02_OBJ"))
        {
            // �浹�� ������Ʈ ����
            Destroy(collision.gameObject);
            Debug.Log("Collision detected");
        }
    }
}
