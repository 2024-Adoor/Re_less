using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // �浹 Ƚ���� ������ ����
    public GameObject destroyPrefab;    // ������ ������ 
    public GameObject newPrefab;  // ������ ������
    public Vector3 RotationOffset;              // ������ ������ ��ġ 
    public bool isBroken;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionCount++; // �浹 Ƚ�� ����
            Debug.Log("�浹 Ƚ��: " + collisionCount);

            // �浹 Ƚ���� 3 �̻��̸� ������ ����
            if (collisionCount >= 3)
            {
                ChangeParentPrefab();
                isBroken = true;
            }
        }
    }

    // ������ ���� �Լ�
    private void ChangeParentPrefab()
    {
        // ���� ������Ʈ ����
        Destroy(destroyPrefab);
        Destroy(gameObject);

        // ���ο� ������ ����
        GameObject newObject = Instantiate(newPrefab, newPrefab.transform.position, newPrefab.transform.rotation);
    }
}
