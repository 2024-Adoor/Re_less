using System;
using NaughtyAttributes;
using Reless;
using Reless.VR;
using Unity.VisualScripting;
using UnityEngine;
using static Reless.Chapter;

public class ChapterControl : MonoBehaviour
{   
    /// <summary>
    /// 에디터에서 테스트용으로 챕터를 설정합니다.
    /// </summary>
    [SerializeField, OnValueChanged(nameof(OnSetChapterChanged))]
    private Chapter setChapterTo;
    
    // 챕터별 스폰포인트 
    [Header("Chapter Spawn Points")]
    public Transform SpawnPoint01;
    public Transform SpawnPoint02;
    public Transform SpawnPoint03;
    public Vector3 offset;

    [Header("Chapter02 Spawn Objects")] 
    public Ch02ObjectSpawner[] ch02ObjectSpawners;

    [Header("?")]
    // UI 트리거용 
    [NonSerialized]
    public int CH02_RespawnCount = 0;
    
    // 챕터별로 해당 챕터에서만 나와야 하는 오브젝트
    [Header("Chapter Objects")]
    public GameObject[] Ch01_Objects; 
    public GameObject[] Ch02_Objects; 
    public GameObject[] Ch03_Objects;
    
    private const float Ch01SpawnDirection = -40;
    private const float Ch02SpawnDirection = 120;
    private const float Ch03SpawnDirection = 90;
    
    [Header("References")]
    [SerializeField]
    private RoomLighting roomLighting;

    /// <summary>
    /// 현재 챕터
    /// </summary>
    public Chapter CurrentChapter
    {
        get => _currentChapter;
        set
        {
            _currentChapter = value;
            GameManager.CurrentPhase = (GamePhase)value;
            switch (_currentChapter)
            {
                case Chapter1: SetupChapter01(); break;
                case Chapter2: SetupChapter02(); break;
                case Chapter3: SetupChapter03(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
    private Chapter _currentChapter;

    private void Awake()
    {
        CurrentChapter = GameManager.CurrentChapter ?? Chapter1;
        
#if UNITY_EDITOR
        if (setChapterTo != CurrentChapter)
        {
            OnSetChapterChanged();
        }
#endif
    }

    private void SetupChapter01()
    {
        SpawnPlayer(SpawnPoint01, Ch01SpawnDirection);

        // 앰비언트 라이팅 조정
        roomLighting.ApplyAmbientColorByChapter(Chapter.Chapter1);
        
        // 챕터 1 오브젝트가 아닌 오브젝트 비활성화 
        SetActiveFalse(Ch02_Objects);
        SetActiveFalse(Ch03_Objects);
        
        SetActiveTrue(Ch01_Objects);
    }
    
    private void SetupChapter02()
    {
        SpawnPlayer(SpawnPoint02, Ch02SpawnDirection);

        // 앰비언트 라이팅 조정
        roomLighting.ApplyAmbientColorByChapter(Chapter.Chapter2);

        // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
        foreach (var spawner in ch02ObjectSpawners) { spawner.StartSpawn(); }
        
        // 열매 카운트 초기화
        PlayerState _PlayerState = GetComponent<PlayerState>();
        _PlayerState.FruitCount = -1;

        // 챕터 2 오브젝트가 아닌 오브젝트 비활성화 
        SetActiveFalse(Ch01_Objects);
        SetActiveFalse(Ch03_Objects);
        
        SetActiveTrue(Ch02_Objects);
    }
    
    private void SetupChapter03()
    {
        SpawnPlayer(SpawnPoint03, Ch03SpawnDirection);

        // 앰비언트 라이팅 조정
        roomLighting.ApplyAmbientColorByChapter(Chapter.Chapter3);

        // OBJspawn's SpawnCH02obj.cs -> isSpawn False 
        foreach (var spawner in ch02ObjectSpawners) { spawner.StopSpawn(); }

        // 챕터 3 오브젝트가 아닌 오브젝트 비활성화 
        SetActiveFalse(Ch01_Objects);
        SetActiveFalse(Ch02_Objects);
        
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

    /// <summary>
    /// 꿈에서 나갑니다.
    /// </summary>
    public void ExitDream()
    {
        // 단계 변경
        GameManager.CurrentPhase = CurrentChapter switch
        {
            Chapter1 => GamePhase.Chapter2,
            Chapter2 => GamePhase.Chapter3,
            Chapter3 => GamePhase.Ending, //참고: 3챕터는 정상 진행 시 이 함수가 호출되기 전에 Ending으로 이미 변경됩니다.
            _ => throw new ArgumentOutOfRangeException()
        };
        
        GameManager.LoadMainScene();
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

    private void OnValidate()
    {
        if (roomLighting.IsUnityNull()) { roomLighting = FindAnyObjectByType<RoomLighting>(); }
    }
    
    /// <summary>
    /// <see cref="setChapterTo"/>가 인스펙터에서 변경될 때 호출됩니다.
    /// </summary>
    private void OnSetChapterChanged()
    {
        Debug.Log($"{nameof(ChapterControl)}: Changing chapter to <b>{setChapterTo}</b>");
        CurrentChapter = setChapterTo;
        
        // 챕터 변경시 플레이어 위치 변경
        // NOTE: 게임 자체는 챕터 변경시 플레이어 위치를 변경하지 않도록 기획이 수정되었으므로 이는 테스트용입니다.
        var spawnParams = setChapterTo switch
        {
            Chapter.Chapter1 => (point :SpawnPoint01, direction: Ch01SpawnDirection),
            Chapter.Chapter2 => (point :SpawnPoint02, direction: Ch02SpawnDirection),
            Chapter.Chapter3 => (point :SpawnPoint03, direction: Ch03SpawnDirection),
            _ => throw new ArgumentOutOfRangeException()
        };
        SpawnPlayer(spawnParams.point, spawnParams.direction);
        
        // 이 다음 코드는 에디터 모드에서만 실행됩니다. (플레이 모드에서는 게임 코드가 이미 처리하고 있음)
        if (Application.isPlaying) return;
        
        // 챕터 변경시 앰비언트 라이팅 변경
        roomLighting.ApplyAmbientColorByChapter(setChapterTo);
    }
}