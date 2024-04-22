using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Reless;

public class UI_Canvas : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform
    public float distanceFromCamera = 1f; // 카메라로부터의 거리

    // 플레이어
    public GameObject Player;
    PlayerControl _PlayerControl;
    PlayerState _PlayerState;
    ChapterControl _ChapterControl;

    // Chapter 01 UI RawImage
    public RawImage Ch01_Tutorial_1;
    public RawImage Ch01_Tutorial_2;
    public RawImage Ch01_Tutorial_Jump;     // Trigger로 작동
    public RawImage Ch01_Tutorial_Friend;   // Trigger로 작동

    // Chapter 02 UI RawImage
    public RawImage Ch02_Tutorial_1;
    public RawImage Ch02_Tutorial_2;
    public RawImage Ch02_Watch;             // Trigger로 작동
    public RawImage Ch02_Door;              // Trigger로 작동 

    // Chatper 03 Ui RawImage
    public RawImage Ch03_Tutorial_1;
    public RawImage Ch03_Suji;              // Trigger로 작동
    public RawImage Ch03_End;               // Trigger로 작동 

    // Can't Awake RawImage
    public RawImage CantAwake;

    // 열매 UI RawImage
    public RawImage fruit_0_1;
    public RawImage fruit_1_1;
    public RawImage fruit_0_2;
    public RawImage fruit_1_2;
    public RawImage fruit_2_2;

    // UI 컨트롤을 위한 bool 값
    bool JumpUIFin = false;
    bool FriendUIFin = false;
    bool WatchUIFin = false;
    bool DoorUIFin = false;
    bool Ch02Tuto2Fin = false;
    bool EndUIFin = false;
    
    // Chapter 02 Watch 
    public GameObject Watch;

    // Chapter 03 Monitor Button
    public GameObject MonitorButton;

    // Chapter 03 Suji
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;

    void Start()
    {
        _PlayerControl = Player.GetComponent<PlayerControl>();
        _ChapterControl = Player.GetComponent<ChapterControl>();
        
        // 전체 RawImage 비활성화
        UnableRawImage(Ch01_Tutorial_1);
        UnableRawImage(Ch01_Tutorial_2);
        UnableRawImage(Ch01_Tutorial_Jump);
        UnableRawImage(Ch01_Tutorial_Friend);

        UnableRawImage(Ch02_Tutorial_1);
        UnableRawImage(Ch02_Tutorial_2);
        UnableRawImage(Ch02_Watch);
        UnableRawImage(Ch02_Door);

        UnableRawImage(Ch03_Tutorial_1);
        UnableRawImage(Ch03_Suji);
        UnableRawImage(Ch03_End);

        UnableRawImage(CantAwake);

        UnableRawImage(fruit_0_1);
        UnableRawImage(fruit_1_1);
        UnableRawImage(fruit_0_2);
        UnableRawImage(fruit_1_2);
        UnableRawImage(fruit_2_2);


        // 챕터 1 시작시 UI 창 
        if(_ChapterControl.Ch01)
        {
            _PlayerControl.speed = 0f;

            // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
            Invoke("EnableCh01Tutorial1", 0.2f);
            Invoke("UnableCh01Tutorial1", 3.2f);
            Invoke("EnableCh01Tutorial2", 3.2f);    // 플레이어 속도 초기화 (4f)
            Invoke("UnableCh01Tutorial2", 6.2f);    // 튜토리얼 UI 비활성화 & 열매 UI 활성화 
        }
        // 챕터 2 시작시 UI 창 
        if(_ChapterControl.Ch02)
        {
            // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
            Invoke("EnableCh02Tutorial1", 0.2f);
            Invoke("UnableCh02Tutorial1", 3.2f);    // 튜토리얼 UI 비활성화 & 열매 UI 활성화
        }
        // 챕터 3 시작시 UI 창 
        if(_ChapterControl.Ch03)
        {
            // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
            Invoke("EnableCh03Tutorial1", 0.2f);
            Invoke("UnableCh03Tutorial1", 3.2f);
        }
    }

    void Update()
    {
        if(Player != null)
        {
            _PlayerState = Player.GetComponent<PlayerState>();
            _ChapterControl = Player.GetComponent<ChapterControl>();
        }

        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        OnOffMonitor _OnOffMonitor = MonitorButton.GetComponent<OnOffMonitor>();

        // 카메라의 전방 벡터와 거리를 곱하여 원하는 위치를 계산합니다.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // 계산된 위치로 Canvas를 이동시킵니다.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;


        // 챕터 1 - 점프 튜토리얼 UI 
        if(_PlayerState.isJumpUI && !JumpUIFin)
        {
            EnableRawImage(Ch01_Tutorial_Jump);
            _PlayerControl.speed = 0f;
            // 3초 뒤에 UnableRawImage(Ch01_Tutorial_Jump) && _PlayerControl.speed = 4.0f
            Invoke("Ch01Jump_Speed", 3f);
        }

        // 챕터 1 - 열매 전달 튜토리얼 UI 
        if(_PlayerState.isFriendUI && !FriendUIFin)
        {
            EnableRawImage(Ch01_Tutorial_Friend);
            _PlayerControl.speed = 0f;
            // 2초 뒤에 UnableRawImage(Ch01_Tutorial_Friend) && _PlayerControl.speed = 4.0f
            Invoke("Ch01Jump_Friend", 2f);
        }

        // 챕터 1 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.Ch01 && _PlayerState.fruitCount == 1)
        {
            UnableRawImage(fruit_0_1);
            EnableRawImage(fruit_1_1);
        }

        // 챕터 2 - 리스폰 했을 때, 시계  UI 
        if(_ChapterControl.CH02_RespawnCount > 0 && !WatchUIFin)
        {
            EnableRawImage(Ch02_Watch);

            // 시계가 부숴졌을 때 UI 삭제 
            if(Watch == null)
            {
                UnableRawImage(Ch02_Watch);
                WatchUIFin = true;
            }
        }

        // 챕터 2 - 문 앞에 왔을 때, 문 UI
        if(_PlayerState.isDoorUI && !DoorUIFin)
        {
            EnableRawImage(Ch02_Door);
            _PlayerControl.speed = 0f;
            // 2초 뒤에 UnableRawImage(Ch02_Door) && _PlayerControl.speed = 4.0f
            Invoke("Ch02_Door_Speed", 2f);
        }
        
        // 챕터 2 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.Ch02)
        {
            if(_PlayerState.fruitCount == 1)
            {
                UnableRawImage(fruit_0_2);
                EnableRawImage(fruit_1_2);
            }
            if(_PlayerState.fruitCount == 2)
            {
                UnableRawImage(fruit_1_2);
                EnableRawImage(fruit_2_2);

                // 친구에게 가져다주자 UI Invoke 
                if(!Ch02Tuto2Fin)
                {
                    EnableRawImage(Ch02_Tutorial_2);
                    Invoke("UnableCh02Tutorial2", 3f);
                }
                
            }
        }

        // 챕터 3 - 수지가 움직일 때, 수지 UI
        if(_SujiEndingTest.canMove)
        {
            EnableRawImage(Ch03_Suji);
        }
        else
        {
            UnableRawImage(Ch03_Suji);
        }

        // 챕터 3 - 리스폰 됐을 때, 엔딩 UI 
        if(_PlayerState.isTeleport && !EndUIFin)
        {
            EnableRawImage(Ch03_End);
            Invoke("UnableCh03_End", 2f);
        }

        // 모든 챕터 - FruitCount < 0 일때 캐릭터와 충돌, disAwake 활성화 
        if(_PlayerState.fruitCount < 0 && _PlayerState.isDisawakeUI_Trigger)
        {
            EnableRawImage(CantAwake);
        }
        else
        {
            UnableRawImage(CantAwake);
        }
    }

    /**************************************************************************************************/

    void EnableRawImage(RawImage yourRawImage)
    {
        // 렌더러를 활성화합니다.
        yourRawImage.enabled = true;
    }

    void UnableRawImage(RawImage yourRawImage)
    {
        // 렌더러를 활성화합니다.
        yourRawImage.enabled = false;
    }

    /**************************************************************************************************/

    // 챕터 1 튜토리얼 UI Invoke 용
    void EnableCh01Tutorial1()
    {
        EnableRawImage(Ch01_Tutorial_1);
    }

    void UnableCh01Tutorial1()
    {
        UnableRawImage(Ch01_Tutorial_1);
    }

    void EnableCh01Tutorial2()
    {
        EnableRawImage(Ch01_Tutorial_2);
        _PlayerControl.speed = 4f;
    }

    void UnableCh01Tutorial2()
    {
        UnableRawImage(Ch01_Tutorial_2);

        // 열매 UI 활성화 
        EnableRawImage(fruit_0_1);
    }

    // 챕터 2 튜토리얼 UI Invoke 용
    void EnableCh02Tutorial1()
    {
        EnableRawImage(Ch02_Tutorial_1);
    }

    void UnableCh02Tutorial1()
    {
        UnableRawImage(Ch02_Tutorial_1);

        // 열매 UI 활성화 
        EnableRawImage(fruit_0_2);
    }

    void UnableCh02Tutorial2()
    {
        UnableRawImage(Ch02_Tutorial_2);
        Ch02Tuto2Fin = true;
    }

    // 챕터 3 튜토리얼 UI Invoke 용
    void EnableCh03Tutorial1()
    {
        EnableRawImage(Ch03_Tutorial_1);
    }

    void UnableCh03Tutorial1()
    {
        UnableRawImage(Ch03_Tutorial_1);
    }

    /**************************************************************************************************/

    void UnableCh03_End()
    {
        UnableRawImage(Ch03_End);
        EndUIFin = true;
    }

    /**************************************************************************************************/

    // 챕터1 점프 튜토리얼 UI 비활성화 & 플레이어 스피드 초기화 
    void Ch01Jump_Speed()
    {
        UnableRawImage(Ch01_Tutorial_Jump);
        _PlayerControl.speed = 4.0f;
        JumpUIFin = true;
    }

    // 챕터1 열매 전달 튜토리얼 UI 비활성화 & 플레이어 스피드 초기화 
    void Ch01Jump_Friend()
    {
        UnableRawImage(Ch01_Tutorial_Friend);
        _PlayerControl.speed = 4.0f;
        FriendUIFin = true;
    }

    // 챕터2 문 UI 비활성화 & 플레이어 스피드 초기화 
    void Ch02_Door_Speed()
    {
        UnableRawImage(Ch02_Door);
        _PlayerControl.speed = 4.0f;
        DoorUIFin = true;
    }


    
}
