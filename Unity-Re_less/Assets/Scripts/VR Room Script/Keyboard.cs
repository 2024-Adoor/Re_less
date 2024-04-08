using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public float downwardAmount = 0.3f; // �������� ��
    public float downwardSpeed = 0.1f; // �������� �ӵ�
    public bool enterDown = false; 

    public GameObject Ch03Fruit;       // Fruit & Cursor 

    private bool hasCollided = false; // �浹 ���� üũ
    private Vector3 initialPosition; // �ʱ� ��ġ ����

    private void Start()
    {
        initialPosition = transform.position; // �ʱ� ��ġ ����
    }

    private void Update()
    {
        if (Ch03Fruit != null)
        {
            Ch03_FruitSnap _Ch03_FruitSnap = Ch03Fruit.GetComponent<Ch03_FruitSnap>();

            if (hasCollided && _Ch03_FruitSnap.isDetected)
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"�� ���
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true; // �浹 ���� ����
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"�� ���


        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = false; // �浹 ���� �� ���� �ʱ�ȭ
        }
    }
}
