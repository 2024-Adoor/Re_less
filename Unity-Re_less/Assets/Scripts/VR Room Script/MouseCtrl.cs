using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless;
using Unity.VisualScripting;

public class MouseCtrl : MonoBehaviour
{
    public Transform ScreenObject; 
    public GameObject OnPopup;
    public GameObject Player;
    ChapterControl _ChapterControl;

    public float maxY = 42.2f; // B ������Ʈ�� �ִ� Y ��ġ
    public float minY = 31.1f; // B ������Ʈ�� �ּ� Y ��ġ
    public float maxZ = -51.9f; // B ������Ʈ�� �ִ� Z ��ġ
    public float minZ = -71.0f; // B ������Ʈ�� �ּ� Z ��ġ

    private Vector3 lastPositionA; // A ������Ʈ�� ������ ��ġ
    private bool collisionDetected = false; // �浹 ���� ���θ� �����ϴ� ����

    public GameObject Ch03_Fruit;
    
    private Ch03_FruitSnap Ch03FruitScript;

    void Start()
    {
        lastPositionA = transform.position; // �ʱ� ��ġ ����

        if (Ch03_Fruit != null)
        {
            Ch03FruitScript = Ch03_Fruit.GetComponent<Ch03_FruitSnap>();
        }
    }

    void Update()
    {
        if (!Ch03FruitScript.IsUnityNull())
        {
            collisionDetected = Ch03FruitScript.isDetected;

            if (!collisionDetected) // �浹�� �������� ���� ��쿡�� ������ ó��
            {
                Vector3 currentPositionA = transform.position; // A ������Ʈ�� ���� ��ġ

                // A ������Ʈ�� �̵��� ���
                Vector3 displacement = currentPositionA - lastPositionA;

                // B ������Ʈ�� ��ġ�� �̵����� ���� ���� (Z ���� ����)
                Vector3 newPositionB = ScreenObject.position;
                
                newPositionB.z += displacement.z;
                newPositionB.y += displacement.x; // Y ���� Z ��ȭ�� ����
                newPositionB.y = Mathf.Clamp(newPositionB.y, minY, maxY); // Y ��ġ�� �ִ� �� �ּ� ������ ����
                newPositionB.z = Mathf.Clamp(newPositionB.z, minZ, maxZ); // Z ��ġ�� �ִ� �� �ּ� ������ ����
                ScreenObject.position = newPositionB;

                lastPositionA = currentPositionA; // ���� ��ġ ����
            } 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ���� isKinematic��  ��Ȱ��ȭ
            GetComponent<Rigidbody>().isKinematic = false;
            if(Player != null)
            {
                _ChapterControl = Player.GetComponent<ChapterControl>();
            }

            // é�� 3�϶��� �˾� ��Ȱ��ȭ
            if(_ChapterControl.CurrentChapter is Chapter.Chapter3)
            {
                OnPopup.SetActive(false);
            }
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ������ �� isKinematic�� Ȱ��ȭ
            GetComponent<Rigidbody>().isKinematic = true;
        }
}
}