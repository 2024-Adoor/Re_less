using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

    public Volume volume;

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

    }
    
    public void Temp_SpawnPlayerCh02()
    {
        SpawnPlayer(SpawnPoint02);
    }
    
    public void Temp_SpawnPlayerCh03()
    {
        SpawnPlayer(SpawnPoint03);
    }
    
    void SpawnPlayer(Transform Point)
    {
        transform.position = Point.position + offset;
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

}