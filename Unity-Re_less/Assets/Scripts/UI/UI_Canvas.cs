using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    // TextMeshPro Text 
    public TMP_Text messageText;
    bool isTextChange = false;

    // BackGround RawImage
    public RawImage BackGround;

    // Chapter 01 UI RawImage
    public RawImage Ch01_Tutorial_1;
    public RawImage Ch01_Tutorial_2;
    public RawImage Ch01_Tutorial_Jump;     // Trigger로 작동

    // Chapter 02 UI RawImage
    public RawImage Ch02_Tutorial_1;
    public RawImage Ch02_Tutorial_2;

    // Chatper 03 Ui RawImage
    public RawImage Ch03_Tutorial_1;
    public RawImage Ch03_Tutorial_2;

    // Can't Awake RawImage
    public RawImage CantAwake;

    // 열매 UI RawImage
    public RawImage fruit_0_1;
    public RawImage fruit_1_1;
    public RawImage fruit_0_2;
    public RawImage fruit_1_2;
    public RawImage fruit_2_2;

    // 열매받은 캐릭터 UI RawImage
    public RawImage Ch01_Sleeping;
    public RawImage Ch01_SleepOut;
    public RawImage Ch02_Sleeping;
    public RawImage Ch02_SleepOut_1;
    public RawImage Ch02_SleepOut_2;
    public RawImage Ch03_Sleeping;
    public RawImage Ch03_SleepOut;

    // UI 컨트롤을 위한 bool 값
    bool JumpUIFin = false;
    bool FriendUIFin = false;
    bool WatchUIFin = false;
    bool DoorUIFin = false;
    bool Ch02Tuto2Fin = false;
    bool EndUIFin = false;
    
    // Chapter 01 Clock
    public GameObject Clock;
    AniManage Clock_AniManage;

    // Chapter 02 Watch 
    public GameObject Watch;
    public GameObject Cat;
    public GameObject Cactus;
    Cat_AniManage Cat_AniManage;
    AniManage Cactus_AniManage;

    // Chapter 03 Monitor Button
    public GameObject MonitorButton;
    OnOffMonitor _OnOffMonitor;

    // Chapter 03 Suji
    public GameObject SleepingSuji;
    SleepingSuji _SleepingSuji;
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;

    void Start()
    {
        _PlayerControl = Player.GetComponent<PlayerControl>();
        _ChapterControl = Player.GetComponent<ChapterControl>();
        
        // BackGround RawImage 비활성화
        BackGround.gameObject.SetActive(false);
        //UnableRawImage(BackGround);

        // 전체 RawImage 비활성화
        UnableRawImage(Ch01_Tutorial_1);
        UnableRawImage(Ch01_Tutorial_2);
        UnableRawImage(Ch01_Tutorial_Jump);

        UnableRawImage(Ch02_Tutorial_1);
        UnableRawImage(Ch02_Tutorial_2);

        UnableRawImage(Ch03_Tutorial_1);
        UnableRawImage(Ch03_Tutorial_2);

        UnableRawImage(CantAwake);

        UnableRawImage(fruit_0_1);
        UnableRawImage(fruit_1_1);
        UnableRawImage(fruit_0_2);
        UnableRawImage(fruit_1_2);
        UnableRawImage(fruit_2_2);

        UnableRawImage(Ch01_Sleeping);
        UnableRawImage(Ch01_SleepOut);
        UnableRawImage(Ch02_Sleeping);
        UnableRawImage(Ch02_SleepOut_1);
        UnableRawImage(Ch02_SleepOut_2);
        UnableRawImage(Ch03_Sleeping);
        UnableRawImage(Ch03_SleepOut);

        // 챕터 1 시작시 UI 창 
        if(_ChapterControl.CurrentChapter is Chapter.Chapter1)
        {
            Chapter01_StartUI();
        }
        // 챕터 2 시작시 UI 창 
        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            Chapter02_StartUI();
        }
        // 챕터 3 시작시 UI 창 
        // if(_ChapterControl.CurrentChapter is Chapter.Chapter3)
        // {
        //     Chapter03_StartUI();
        // }
    }

    void Update()
    {
        if(Player != null)
        {
            _PlayerState = Player.GetComponent<PlayerState>();
        }

        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        if(MonitorButton != null)
        {
            _OnOffMonitor = MonitorButton.GetComponent<OnOffMonitor>();
        }
        
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
            if(!isTextChange)
            {
                BackGround.gameObject.SetActive(true);
                ChangeMessage("친구에게 가까이 가서\n획득한 열매를 전해주자!");
                isTextChange = true;
            }

            _PlayerControl.speed = 0f;
            // 2초 뒤에 UnableRawImage(BackGround) && _PlayerControl.speed = 4.0f
            Invoke("UnableBack_Ch01Friend", 2f);
        }

        // 챕터 1 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.CurrentChapter is Chapter.Chapter1 && _PlayerState.FruitCount == 1)
        {
            UnableRawImage(fruit_0_1);
            EnableRawImage(fruit_1_1);
        }

        // 챕터 1 - 열매 전달해줬을 때 UI 변경 
        // Clock AniManage.cs isSleepOut == true
        Clock_AniManage = Clock.GetComponent<AniManage>();
        if(Clock_AniManage.isSleepOut)
        {
            UnableRawImage(fruit_1_1);
            UnableRawImage(Ch01_Sleeping);
            EnableRawImage(Ch01_SleepOut);
        }

        // 챕터 2 - 리스폰 했을 때, 시계  UI 
        if(_ChapterControl.CH02_RespawnCount > 0 && !WatchUIFin)
        {
            //Enable_BackText();
            ChangeMessage("시계를 부숴볼까?");

            // 시계가 부숴졌을 때 UI 삭제 
            if(Watch == null)
            {
                //Unable_BackText();
                WatchUIFin = true;
            }
        }

        // 챕터 2 - 문 앞에 왔을 때, 문 UI
        if(_PlayerState.isDoorUI && !DoorUIFin)
        {
            // BackGround 띄우고 텍스트 변경
            //Enable_BackText();
            ChangeMessage("세번째 손가락으로 버튼을 눌러\n문을 당겨보자!");
            _PlayerControl.speed = 0f;

            // 2초 뒤에 UnableRawImage(BackGround) && _PlayerControl.speed = 4.0f
            //Invoke("UnableBack_Speed", 2f);
        }
        
        // 챕터 2 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            if(_PlayerState.FruitCount == 1)
            {
                UnableRawImage(fruit_0_2);
                EnableRawImage(fruit_1_2);
            }
            if(_PlayerState.FruitCount == 2)
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

        // 챕터 2 - 열매 전달해줬을 때 UI 변경
        // Cat Cat_AniManage.cs isSleepOut == true
        // Cactus AniManage.cs isSleepOut == true
        Cat_AniManage = Cat.GetComponent<Cat_AniManage>();
        Cactus_AniManage = Cactus.GetComponent<AniManage>();
        if(Cat_AniManage.isSleepOut)
        {
            UnableRawImage(fruit_2_2);
            EnableRawImage(fruit_1_2);

            UnableRawImage(Ch02_Sleeping);
            EnableRawImage(Ch02_SleepOut_1);
        }
        if(Cactus_AniManage.isSleepOut)
        {
            UnableRawImage(fruit_1_2);

            UnableRawImage(Ch02_SleepOut_1);
            EnableRawImage(Ch02_SleepOut_2);
        }

        // 챕터 3 - 수지랑 열매 닿았을 때
        if(SleepingSuji != null)
        {
            _SleepingSuji = SleepingSuji.GetComponent<SleepingSuji>();
        }
        if(_SleepingSuji.isDetected)
        {
            UnableRawImage(Ch03_Sleeping);
            EnableRawImage(Ch03_SleepOut);
        }

        // 챕터 3 - 수지가 움직일 때, 수지 UI
        if(_SujiEndingTest.canMove)
        {
            UnableRawImage(Ch03_SleepOut);
            //Enable_BackText();
            ChangeMessage("수지를 따라가자!");
        }
        else
        {
            //Unable_BackText();
        }

        // 모든 챕터 - FruitCount < 0 일때 캐릭터와 충돌, disAwake 활성화 
        if(_PlayerState.FruitCount < 0 && _PlayerState.isDisawakeUI_Trigger)
        {
            EnableRawImage(CantAwake);
        }
        else
        {
            UnableRawImage(CantAwake);
        }
    }

    /**************************************************************************************************/
    // 챕터 시작시 나오는 UI 
    public void Chapter01_StartUI()
    {
        _PlayerControl.speed = 0f;
        // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
        Invoke("EnableCh01Tutorial1", 0.2f);
        Invoke("UnableCh01Tutorial1", 3.2f);
        Invoke("EnableCh01Tutorial2", 3.2f);    // 플레이어 속도 초기화 (4f)
        Invoke("UnableCh01Tutorial2", 6.2f);    // 튜토리얼 UI 비활성화 & 열매 UI 활성화 
    }

    public void Chapter02_StartUI()
    {
        UnableRawImage(Ch01_SleepOut);

        // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
        Invoke("EnableCh02Tutorial1", 2f);
        Invoke("UnableCh02Tutorial1", 5f);    // 튜토리얼 UI 비활성화 & 열매 UI 활성화
    }

    // 챕터 3 시작 = 모니터 켜졌을 때 
    public void Chapter03_StartUI()
    {
        UnableRawImage(Ch02_SleepOut_2);
        // Ch03_Tutorial_2 활성화 후 지우기 
        Invoke("EnableCh03Tutorial2", 0.01f);
        Invoke("UnableCh03Tutorial2", 2.5f);
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

    void UnableBack_Ch01Friend()
    {
        BackGround.gameObject.SetActive(false);
        _PlayerControl.speed = 4f;
        FriendUIFin = true;
    }

    void UnableCh01Tutorial2()
    {
        UnableRawImage(Ch01_Tutorial_2);

        // 열매 UI 활성화 
        EnableRawImage(fruit_0_1);
        
        // Sleeping UI 활성화
        EnableRawImage(Ch01_Sleeping);
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

        // Sleeping UI 활성화
        EnableRawImage(Ch02_Sleeping);
    }

    void UnableCh02Tutorial2()
    {
        UnableRawImage(Ch02_Tutorial_2);
        Ch02Tuto2Fin = true;
    }

    // 챕터 3 튜토리얼 UI Invoke 용
    void EnableCh03Tutorial2()
    {
        EnableRawImage(Ch03_Tutorial_2);
    }

    void UnableCh03Tutorial2()
    {
        UnableRawImage(Ch03_Tutorial_2);

        // Sleeping UI 활성화
        EnableRawImage(Ch03_Sleeping);
    }

    /**************************************************************************************************/

    // 챕터1 점프 튜토리얼 UI 비활성화 & 플레이어 스피드 초기화 
    void Ch01Jump_Speed()
    {
        UnableRawImage(Ch01_Tutorial_Jump);
        _PlayerControl.speed = 4.0f;
        JumpUIFin = true;
    }

    // BackGround 비활성화 & 플레이어 스피드 초기화
    // void UnableBack_Speed()
    // {
    //     Unable_BackText();
    //     _PlayerControl.speed = 4.0f;
    //     DoorUIFin = true;
    // }

    /**************************************************************************************************/

    // TextMeshPro 텍스트 바꾸는 함수 
    public void ChangeMessage(string newMessage)
    {
        messageText.text = newMessage;
    }
}
