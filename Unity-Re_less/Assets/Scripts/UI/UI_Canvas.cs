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
    public TMP_Text messageText;        // 기본 text
    public TMP_Text fruitText;          // 열매 text

    // BackGround RawImage
    public RawImage BackGround;         // 기본 BackGround
    public RawImage BackGround_Fruit;   // 열매 BackGround

    // Chapter 01 UI RawImage
    public RawImage Ch01_Tutorial_Jump;     

    // 열매받은 캐릭터 UI RawImage
    public RawImage Ch01_Sleeping;
    public RawImage Ch01_SleepOut;
    public RawImage Ch02_Sleeping;
    public RawImage Ch02_SleepOut_1;
    public RawImage Ch02_SleepOut_2;
    public RawImage Ch03_Sleeping;
    public RawImage Ch03_SleepOut;

    // UI 컨트롤을 위한 bool 값
        // chatper 01
    bool JumpUIFin = false;
    bool FriendUIFin = false;
    bool SleepOutClock = false;
    bool WatchUIFin = false;
    bool CrossUIFin = false;

        // chatper 02
    bool DoorUIFin = false;
    bool Ch02Tuto2Fin = false;
    

    // 여러번 확인을 눌러야 할 때 bool 값 
    bool canWatchUiChange = false;
    int WatchBcount = 0;
    int StartBcount1 = 0;
    
    // 챕터 초반 UI 제어 bool 값
    bool Ch01Fin = false;

    // Chapter 01 Clock
    public GameObject Clock;
    public GameObject Clock_Chat;
    Chat_Character _Clock_Chat;
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
        _ChapterControl = Player.GetComponent<ChapterControl>();

        // 전체 RawImage 비활성화
        BackGround.gameObject.SetActive(false);
        BackGround_Fruit.gameObject.SetActive(false);
        Ch01_Tutorial_Jump.gameObject.SetActive(false);
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
            _PlayerControl = Player.GetComponent<PlayerControl>();
        }

        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        if(MonitorButton != null)
        {
            _OnOffMonitor = MonitorButton.GetComponent<OnOffMonitor>();
        }

        if(Clock_Chat != null)
        {
            _Clock_Chat = Clock_Chat.GetComponent<Chat_Character>();
        }
        
        // 카메라의 전방 벡터와 거리를 곱하여 원하는 위치를 계산합니다.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // 계산된 위치로 Canvas를 이동시킵니다.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;

        // 챕터 1 시작시 UI 창 
        if(_ChapterControl.CurrentChapter is Chapter.Chapter1 && !Ch01Fin)
        {
            Chapter01_StartUI();
        }

        // 챕터 1 - 점프 튜토리얼 UI (이미지로 처리)
        if(_PlayerState.isJumpUI && !JumpUIFin)
        {
            Ch01_Tutorial_Jump.gameObject.SetActive(true);
            _PlayerControl.speed = 0f;

            // 확인 누르면 실행으로 변경
            if(_PlayerControl.isBdown)
            {
                Ch01_Tutorial_Jump.gameObject.SetActive(false);
                _PlayerControl.speed = 4.0f;
                JumpUIFin = true;
            }
        }

        // 챕터 1 - 열매 전달 튜토리얼 UI (텍스트 변환으로 처리)
        if(_PlayerState.isFriendUI && !FriendUIFin)
        {
            BackGround.gameObject.SetActive(true);
            ChangeMessage(messageText, "친구에게 가까이 가서\n획득한 열매를 전해주자!");
            
            _PlayerControl.speed = 0f;

            // 확인 누르면 실행으로 변경 
            if(_PlayerControl.isBdown)
            {
                BackGround.gameObject.SetActive(false);
                _PlayerControl.speed = 4f;
                FriendUIFin = true;
            }
        }

        // 챕터 1 - 열매 먹었을 때 UI 변경 (텍스트 변환으로 처리)
        if(_ChapterControl.CurrentChapter is Chapter.Chapter1 && _PlayerState.FruitCount == 1)
        {
            ChangeMessage(fruitText, "1/1");
        }

        // 챕터 1 - 열매 전달해줬을 때 UI 변경 (이미지로 처리)
        // Clock AniManage.cs isSleepOut == true
        Clock_AniManage = Clock.GetComponent<AniManage>();
        if(Clock_AniManage.isSleepOut && !SleepOutClock)
        {
            BackGround_Fruit.gameObject.SetActive(false);
            UnableRawImage(Ch01_Sleeping);
            EnableRawImage(Ch01_SleepOut);

            // 챕터 1 - 시계토끼 대사 끝났을 때 SleepOut UI 비활성화
            if(_Clock_Chat.isClear)
            {
                UnableRawImage(Ch01_SleepOut);
                SleepOutClock = true;
            }
        }

        // 챕터 1 - 길 건너기 전 trigger 밟았을 때,
        if(_PlayerState.isCh02UI && !CrossUIFin)
        {
            BackGround.gameObject.SetActive(true);
            _PlayerControl.speed = 0f;
            ChangeMessage(messageText, "길을 건너 열매를 찾자!\n(시간을 멈추면 건너기 쉬워질지도?)");

            // 확인 누르면 실행
            if(_PlayerControl.isBdown)
            {
                BackGround.gameObject.SetActive(false);
                _PlayerControl.speed = 4f;

                BackGround_Fruit.gameObject.SetActive(true);
                ChangeMessage(fruitText, "0/2");
                EnableRawImage(Ch02_Sleeping);
                CrossUIFin = true;
            }
        }

        // 챕터 1 - 오브젝트에 부딪혀서 리스폰 했을 때, 시계  UI (텍스트 변환으로 처리)
        if(_ChapterControl.CH02_RespawnCount > 0 && !WatchUIFin)
        {
            BackGround.gameObject.SetActive(true);
            
            if(_PlayerControl.isBdown)
            {
                WatchBcount ++;
                _PlayerControl.isBdown = false;
            }

            Debug.Log(WatchBcount);

            if(WatchBcount == 0)
            {
                ChangeMessage(messageText, "시간을 멈추면 건널 수 있지 않을까?\n여기서 나가자 시계를 그려오자!");
            }
            else if(WatchBcount == 1)
            {
                ChangeMessage(messageText, "양쪽 조이스틱을 눌러 볼을 꼬집자!\n꿈에서 깰 수 있을지 몰라");
            }
            else if(WatchBcount > 2)
            {
                BackGround.gameObject.SetActive(false);
                WatchUIFin = true;
            }
        }



        // 챕터 1 - 패스스루로 돌아가지 않고 또 충돌했을 때 
        // if(_ChapterControl.CH02_RespawnCount > 1)
        // {
        //     BackGround.gameObject.SetActive(true);
        //     ChangeMessage(messageText, "양쪽 조이스틱을 눌러 볼을 꼬집자!\n꿈에서 깰 수 있을지 몰라");
        //     
        //     if(_PlayerControl.isBdown)
        //     {
        //         BackGround.gameObject.SetActive(false);
        //     }
        // }

        // 챕터 2 - 문 앞에 왔을 때, 문 UI (텍스트 변환으로 처리)
        if(_PlayerState.isDoorUI && !DoorUIFin)
        {
            BackGround.gameObject.SetActive(true);
            ChangeMessage(messageText, "세번째 손가락으로 버튼을 눌러\n문을 당겨보자!");
            _PlayerControl.speed = 0f;

            // 확인 누르면 실행으로 변경 
            // 2초 뒤에 UnableRawImage(BackGround) && _PlayerControl.speed = 4.0f
            Invoke("UnableBack_Ch02Door", 2f);
        }
        
        // 챕터 2 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            if(_PlayerState.FruitCount == 1)
            {
                // UnableRawImage(fruit_0_2);
                // EnableRawImage(fruit_1_2);
            }
            if(_PlayerState.FruitCount == 2)
            {
                // UnableRawImage(fruit_1_2);
                // EnableRawImage(fruit_2_2);

                // 친구에게 가져다주자 UI Invoke 
                if(!Ch02Tuto2Fin)
                {
                    // 텍스트 변환
                    BackGround.gameObject.SetActive(true);
                    ChangeMessage(messageText, "열매를 다 모았어!\n친구에게 가져다주자");

                    // 확인 누르면 실행으로 변환
                    Invoke("UnableBack_Ch02Friend", 3f);
                }
            }
        }

        // 챕터 2 - 열매 전달해줬을 때 UI 변경 (이미지처로 처리)
        // Cat Cat_AniManage.cs isSleepOut == true
        // Cactus AniManage.cs isSleepOut == true
        Cat_AniManage = Cat.GetComponent<Cat_AniManage>();
        Cactus_AniManage = Cactus.GetComponent<AniManage>();
        if(Cat_AniManage.isSleepOut)
        {
            // UnableRawImage(fruit_2_2);
            // EnableRawImage(fruit_1_2);

            UnableRawImage(Ch02_Sleeping);
            EnableRawImage(Ch02_SleepOut_1);
        }
        if(Cactus_AniManage.isSleepOut)
        {
            // UnableRawImage(fruit_1_2);

            UnableRawImage(Ch02_SleepOut_1);
            EnableRawImage(Ch02_SleepOut_2);
        }

        // 챕터 3 - 수지랑 열매 닿았을 때 (이미지로 처리)
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
        // if(_SujiEndingTest.canMove)
        // {
        //     UnableRawImage(Ch03_SleepOut);
        // 
        //     // 텍스트 변환
        //     BackGround.gameObject.SetActive(true);
        //     ChangeMessage("수지를 따라가자!");
        // }
        // else
        // {
        //     // BackGround.gameObject.SetActive(false);
        // }

        // 모든 챕터 - FruitCount < 0 일때 캐릭터와 충돌, disAwake 활성화 
        // if(_PlayerState.FruitCount < 0 && _PlayerState.isDisawakeUI_Trigger)
        // {
        //     // 텍스트 변환
        //     BackGround.gameObject.SetActive(true);
        //     ChangeMessage("열매 없이는\n친구를 깨울 수 없어~");
        // }
        // else
        // {
        //     
        // }
    }

    /**************************************************************************************************/
    // 챕터 시작시 나오는 UI 
    public void Chapter01_StartUI()
    {
        if(_PlayerControl.isBdown)
        {
            StartBcount1 ++;
            _PlayerControl.isBdown = false;
        }

        if(StartBcount1 == 0)
        {
            BackGround.gameObject.SetActive(true);
            ChangeMessage(messageText, "월드에서 열매를 찾아\n친구에게 가져다주자!");
            _PlayerControl.speed = 0f;
        }
        else if(StartBcount1 == 1)
        {
            ChangeMessage(messageText, "왼쪽 조이스틱으로 이동하자");
            _PlayerControl.speed = 0f;
        }
        else if(StartBcount1 == 2)
        {
            BackGround.gameObject.SetActive(false);
            _PlayerControl.speed = 4f;
            BackGround_Fruit.gameObject.SetActive(true);
            ChangeMessage(fruitText, "0/1");
            EnableRawImage(Ch01_Sleeping);
            Ch01Fin = true;
        }
    }

    public void Chapter02_StartUI()
    {
        // UnableRawImage(Ch01_SleepOut);

        // 시작하고 1초 뒤에 Chapter_Start render 활성화 -> 3초 뒤 render 비활성화 
    }

    // 챕터 3 시작 = 모니터 켜졌을 때 
    public void Chapter03_StartUI()
    {
        // UnableRawImage(Ch02_SleepOut_2);
        // Ch03_Tutorial_2 활성화 후 지우기 
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

    // TextMeshPro 텍스트 바꾸는 함수 
    public void ChangeMessage(TMP_Text TMP_Text, string newMessage)
    {
        TMP_Text.text = newMessage;
    }
}
