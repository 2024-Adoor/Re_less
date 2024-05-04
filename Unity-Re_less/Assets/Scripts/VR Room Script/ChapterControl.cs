using System;
using System.Collections;
using System.Collections.Generic;
using Reless;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Assertions;

public class ChapterControl : MonoBehaviour
{   
#if UNITY_EDITOR
    /// <summary>
    /// 에디터에서 테스트용으로 챕터를 설정합니다.
    /// </summary>
    [SerializeField]
    private Chapter setChapterTo;
#endif
    
    // 챕터별 스폰포인트 
    public Transform SpawnPoint01;
    public Transform SpawnPoint02;
    public Transform SpawnPoint03;
    public Vector3 offset; 

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

    // 챕터별 라이트 조절
    public Light spotLight; 
    
    // UI 트리거용 
    public int CH02_RespawnCount = 0;

    public Volume volume;

    private bool _temp_UseStartControlLogic = false;
    
    // 챕터별로 해당 챕터에서만 나와야 하는 오브젝트
    public GameObject[] Ch01_Objects; 
    public GameObject[] Ch02_Objects; 
    public GameObject[] Ch03_Objects;

    /// <summary>
    /// 현재 챕터
    /// </summary>
    public Chapter CurrentChapter
    {
        get => _currentChapter;
        private set
        {
            _currentChapter = value;
            switch (_currentChapter)
            {
                case Chapter.Chapter1: SetupChapter01(); break;
                case Chapter.Chapter2: SetupChapter02(); break;
                case Chapter.Chapter3: SetupChapter03(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
    private Chapter _currentChapter;

    private void Awake()
    {
        if (GameManager.Instance.CurrentChapter is Chapter chapter)
        {
            CurrentChapter = chapter;
        }
#if UNITY_EDITOR
        else
        {
            if (setChapterTo != CurrentChapter)
            {
                CurrentChapter = setChapterTo;
            }
        }
#endif
    }

    void Start()
    {
        
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        if (setChapterTo != CurrentChapter)
        {
            Debug.Log($"Changing chapter to {setChapterTo}");
            CurrentChapter = setChapterTo;
        }
#endif
    }
    private void SetupChapter01()
    {
        SpawnPlayer(SpawnPoint01, -40);

        // 챕터 1 Spot Light Intensity 조절
        spotLight.intensity = 100f;
        
        // 챕터 1 오브젝트가 아닌 오브젝트 비활성화 
        SetActiveFalse(Ch02_Objects);
        SetActiveFalse(Ch03_Objects);
        
        SetActiveTrue(Ch01_Objects);
    }
    
    public void SetupChapter02()
    {
        //SpawnPlayer(SpawnPoint02, 120);

        // 챕터 2 Spot Light Intensity 조절
        spotLight.intensity = 300f;

        // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
        SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
        SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();
        spawnCH02Obj1.isSpawn = true;
        spawnCH02Obj2.isSpawn = true;
        
        // 챕터 2 오브젝트가 아닌 오브젝트 비활성화 
        //SetActiveFalse(Ch01_Objects);
        //SetActiveFalse(Ch03_Objects);
        
        SetActiveTrue(Ch02_Objects);
    }
    
    public void SetupChapter03()
    {
        //SpawnPlayer(SpawnPoint03, 150);
        
        // 챕터 3 Spot Light Intensity 조절
        spotLight.intensity = 1500f;

        // 챕터 3 오브젝트가 아닌 오브젝트 비활성화 
        //SetActiveFalse(Ch01_Objects);
        //SetActiveFalse(Ch02_Objects);
        
        SetActiveTrue(Ch03_Objects);
    }

    void SetActiveFalse(GameObject[] ChapterObjects)
    {
        foreach(GameObject gameObject in ChapterObjects) // Material 배열 순회
        {
            if(gameObject != null) // GameObject가 null이 아닌 경우에만 활성화
            {
                gameObject.SetActive(false);
            }
        }
    }
    
    void SetActiveTrue(GameObject[] ChapterObjects)
    {
        foreach(GameObject gameObject in ChapterObjects) 
        {
            if(gameObject != null) // GameObject가 null이 아닌 경우에만 활성화
            {
                gameObject.SetActive(true);
            }
        }
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
            CH02_RespawnCount++;
            Debug.Log("Respawned at Chapter 02");
        }
    }

}