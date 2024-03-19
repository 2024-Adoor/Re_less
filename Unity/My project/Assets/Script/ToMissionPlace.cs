using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMissionPlace : MonoBehaviour
{
    public Transform targetPlace; // �̵��� ����� Transform ���� ����

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� "Player" �±׸� ������ �ִٸ�
        if (other.CompareTag("Player"))
        {
            // �浹�� �߻��ߴٴ� �α� ���
            Debug.Log("Player collided!");

            // �÷��̾ ������ ��ҷ� �̵�
            other.transform.position = targetPlace.position;
        }
    }
}
