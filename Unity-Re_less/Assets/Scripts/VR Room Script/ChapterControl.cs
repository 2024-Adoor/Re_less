using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterControl : MonoBehaviour
{   
    // é�ͺ� ���࿩��
    public bool Ch01;
    public bool Ch02;
    public bool Ch03;

    // é�ͺ� ��������Ʈ 
    public Transform SpawnPoint01;
    public Transform SpawnPoint02;
    public Transform SpawnPoint03;
    public Vector3 offset; 

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

    void Start()
    {
        if (Ch01)
        {
            SpawnPlayer(SpawnPoint01);
        }
        else if (Ch02)
        {
            SpawnPlayer(SpawnPoint02);

            // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
            SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
            SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();
            spawnCH02Obj1.isSpawn = true;
            spawnCH02Obj2.isSpawn = true;
            
        }
        else if (Ch03)
        {
            SpawnPlayer(SpawnPoint03);
        }
    }

    void Update()
    {
        if (Ch01)
        {
            Chapter01();
        }
        else if (Ch02)
        {
            Chapter02();;
        }
        else if (Ch03)
        {
            Chapter03();
        }
    }

    void SpawnPlayer(Transform Point)
    {
        transform.position = Point.position + offset;
    }


    void Chapter01()
    {
        // Fruit �浹�� ī��Ʈ ++ 
        // Fruit ī��Ʈ 1 �϶�, �ð��䳢 ���ٽ� �ð��䳢 ����� -> �ð��䳢 ��ũ��Ʈ�� �ִϸ��̼� �߰� 
        // �ð��䳢�� ��ȭ �� �н����� ������ ������ 
    }


    void Chapter02()
    {   
        

        // Fruit �浹�� ī��Ʈ ++
        // �ð� isBroken �϶� OBJSpawn isSpawn False && OBJprefab isMoving False && �ö�ƽ �ڽ� ��ȭ (�ö� �� �ִ� ������)
        // Fruit ī��Ʈ 1 �϶�, ������ & ü��Ĺ ����� (�Ѹ� ����� ī��Ʈ--) -> �� ĳ���� ��ũ��Ʈ�� �ִϸ��̼� �߰�  
        // ������ & ü��Ĺ�� ��ȭ �� �н����� ������ ������ 
    }

    // CH02_OBJ�� �浹�� ������
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "CH02_OBJ"�� ���
        if (collision.gameObject.CompareTag("CH02_OBJ"))
        {
            // ������ ��ġ ����
            transform.position = SpawnPoint02.position + offset;
            Debug.Log("Respawned at Chapter 02");
        }
    }

    void Chapter03()
    {
        // ����� ������ �Ѽ� ��ũ�� �� �ڰ� �ִ� �ٸ��� �߰��ϱ�
        // �ٸ����� ����� (AwakeSuji�� isWake �߰�) ��ũ�� ������ ������ �ٸ��� 
        // �ٸ����� ��ȭ -> �������� ����(���� Ŀ���鼭 �� �Ѱ���� �̵�) -> �н����� ������ ������ 
    }
}