using System;
using System.Collections;
using System.Collections.Generic;
using Reless;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class ChapterControl : MonoBehaviour
{   
    // 챕터별 진행여부
    public bool Ch01;
    public bool Ch02;
    public bool Ch03;

    // 챕터별 스폰포인트 
    public Transform SpawnPoint01;
    public Transform SpawnPoint02;
    public Transform SpawnPoint03;
    public Vector3 offset; 

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

    public Volume volume;

    private bool _temp_UseStartControlLogic = false;
    
    private void Awake()
    {
        // 게임매니저가 없는 경우(MainScene에서 시작되지 않음) 리턴합니다. 대신 Start에 있던 기존의 로직이 작동하도록 합니다.
        _temp_UseStartControlLogic = GameManager.NotInThisScene;
        if (_temp_UseStartControlLogic) return;

        // MainScene에서 시작되고 적절하게 페이즈가 지정되었다면 아래 로직을 이어갑니다.
        switch (GameManager.Instance.CurrentPhase)
        {
            case GameManager.Phase.Chapter1: StartChapter01(); break;
            case GameManager.Phase.Chapter2: StartChapter02(); break;
            case GameManager.Phase.Chapter3: StartChapter03(); break;
            
            default: Debug.LogWarning($"Unexpected phase: {GameManager.Instance.CurrentPhase}"); break;
        }
    }

    void Start()
    {
        // 이 경우 Awake에서 로직을 처리했습니다 - Start에서는 아무것도 하지 않습니다.
        if (!_temp_UseStartControlLogic) return;
        
        if (Ch01)
        {
            StartChapter01();
        }
        else if (Ch02)
        {
            StartChapter02();
        }
        else if (Ch03)
        {
            StartChapter03();
        }
    }

    private void StartChapter01()
    {
        SpawnPlayer(SpawnPoint01, -40);
    }
    
    private void StartChapter02()
    {
        SpawnPlayer(SpawnPoint02, 90);

        // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
        SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
        SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();
        spawnCH02Obj1.isSpawn = true;
        spawnCH02Obj2.isSpawn = true;
    }
    
    private void StartChapter03()
    {
        SpawnPlayer(SpawnPoint03, 150);
    }

    void Update()
    {

    }
    
    void SpawnPlayer(Transform Point, float RotateY)
    {
        transform.position = Point.position + offset;

        // 새로운 회전값을 Quaternion.Euler를 사용하여 생성합니다.
        Quaternion newRotation = Quaternion.Euler(0f, RotateY, 0f);
        
        // 새로운 회전값을 적용합니다.
        transform.rotation = newRotation;
    }

    // CH02_OBJ와 충돌시 리스폰
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "CH02_OBJ"인 경우
        if (collision.gameObject.CompareTag("CH02_OBJ"))
        {
            // 리스폰 위치 설정
            transform.position = SpawnPoint02.position + offset;
            Debug.Log("Respawned at Chapter 02");
        }
    }

}