using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public float downwardAmount = 0.3f; // �������� ��
    public float downwardSpeed = 0.1f; // �������� �ӵ�
    public bool enterDown = false; 

    private bool hasCollided = false; // �浹 ���� üũ
    private Vector3 initialPosition; // �ʱ� ��ġ ����

    private void Start()
    {
        initialPosition = transform.position; // �ʱ� ��ġ ����
    }

    private void Update()
    {
        if (hasCollided)
        {
            // �Ʒ��� �̵�
            transform.position -= Vector3.up * downwardSpeed * Time.deltaTime;
            enterDown = true;

            // Ư�� ��ġ �Ʒ��� �̵� �Ϸ� ��
            if (transform.position.y <= initialPosition.y - downwardAmount)
            {
                hasCollided = false; // �浹 ���� �ʱ�ȭ
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"�� ���
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            // ���� ��ġ���� �Ʒ��� Ư�� �縸ŭ�� �̵�
            //transform.position -= Vector3.up * downwardAmount;
            hasCollided = true; // �浹 ���� ����
        }
    }
}
