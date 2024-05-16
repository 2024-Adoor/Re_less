using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Reless;
using Reless.VR;
using UnityEngine.Assertions;
using Logger = Reless.Debug.Logger;

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
    public RawImage BackGround_Confirm; // 확인 BackGround

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
    bool SleepOutCat = false;
    bool SleepOutCactus = false;
    bool ChFruit01 = false;
    bool ChFruit02 = false;
    public bool MonitorOn = false;
    bool MonitorUIFin = false;
    bool MonitorCheckFin = false;
        // chapter 03
    bool canSleepOutSuji = false;
    bool isEnterUIFin = false;
    bool isSujiUIFin = false;
    bool isEndFin = false;

    // 여러번 확인을 눌러야 할 때 bool 값 
    bool canWatchUiChange = false;
    int WatchBcount = 0;
    int StartBcount1 = 0;
    int MonitorBcount = 0;
    int EndBcount = 0;
    
    // 챕터 초반 UI 제어 bool 값
    bool Ch01Fin = false;
    bool Ch02Fin = false;
    bool Ch03Fin = false;

    // Chapter 01 
    public GameObject Clock;
    public GameObject Clock_Chat;
    Chat_Character _Clock_Chat;
    AniManage Clock_AniManage;

    // Chapter 02  
    public GameObject Watch;
    public GameObject Cat;
    public GameObject Cactus;
    public GameObject Cactus_Chat;
    Chat_Character _Cactus_Chat;
    Cat_AniManage Cat_AniManage;
    AniManage Cactus_AniManage;

    // Chapter 03 Suji
    public GameObject SleepingSuji;
    SleepingSuji _SleepingSuji;
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;

    // Ending
    [SerializeField]
    EndingBehaviour _EndingBehaviour;

    void Start()
    {
        Assert.IsNotNull(Player);
        _ChapterControl = Player.GetComponent<ChapterControl>();
        _PlayerState = Player.GetComponent<PlayerState>();
        _PlayerControl = Player.GetComponent<PlayerControl>();
        
        if(Suji != null) { _SujiEndingTest = Suji.GetComponent<SujiEndingTest>(); }
        
        if(Clock_Chat != null)
        {
            _Clock_Chat = Clock_Chat.GetComponent<Chat_Character>();
        }
        
        if(Cactus_Chat != null)
        {
            _Cactus_Chat = Cactus_Chat.GetComponent<Chat_Character>();
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        
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

        // 챕터 2 시작시 UI 창 
        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            Chapter02_StartUI();
        }
    }

    void Update()
    {
        ////REVIEW: 이미 UI는 카메라를 따라가고 있는데 아래 코드가 필요한지 확인 필요
        // 카메라의 전방 벡터와 거리를 곱하여 원하는 위치를 계산합니다.
        /*Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // 계산된 위치로 Canvas를 이동시킵니다.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;*/

        // 챕터 1 시작시 UI 창 
        if(_ChapterControl.CurrentChapter is Chapter.Chapter1 && !Ch01Fin)
        {
            Chapter01_StartUI();
        }
        else if(_ChapterControl.CurrentChapter is Chapter.Chapter2 && !Ch02Fin)
        {
            Chapter02_StartUI();
        }
        else if(_ChapterControl.CurrentChapter is Chapter.Chapter3 && !Ch03Fin)
        {
            Chapter03_StartUI();
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

            Logger.Log(WatchBcount);

            if(_ChapterControl.CurrentChapter is Chapter.Chapter1)
            {
                // 챕터 1일 때만 실행
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
                    
                    // 패스스루로 나갈 수 있게 활성화
                    _PlayerControl.EnableExitAction();
                }
            }
            else
            {
                // 챕터 2로 넘어와서 충돌했을 경우
                if(WatchBcount == 0)
                {
                    ChangeMessage(messageText, "시계를 부숴보자!");
                }
                else if(WatchBcount == 1)
                {
                    BackGround.gameObject.SetActive(false);
                    WatchUIFin = true;
                }
            }
        }

        // 챕터 2 - 문 앞에 왔을 때, 문 UI (텍스트 변환으로 처리)
        if(_PlayerState.isDoorUI && !DoorUIFin)
        {
            BackGround.gameObject.SetActive(true);
            ChangeMessage(messageText, "세번째 손가락으로 버튼을 눌러\n문을 당겨보자!");
            _PlayerControl.speed = 0f;

            // 확인 누르면 실행으로 변경 
            if(_PlayerControl.isBdown)
            {
                BackGround.gameObject.SetActive(false);
                _PlayerControl.speed = 4f;
                DoorUIFin = true;
            }
        }
        
        // 챕터 2 - 열매 먹었을 때 UI 변경
        if(_ChapterControl.CurrentChapter is Chapter.Chapter2)
        {
            if(_PlayerState.FruitCount == 1 && !ChFruit01)
            {
                ChangeMessage(fruitText, "1/2");
                ChFruit01 = true;
            }
            if(_PlayerState.FruitCount == 2 && !ChFruit02)
            {
                ChangeMessage(fruitText, "2/2");

                // 친구에게 가져다주자 UI
                if(!Ch02Tuto2Fin)
                {
                    // 텍스트 변환
                    BackGround.gameObject.SetActive(true);
                    ChangeMessage(messageText, "열매를 다 모았어!\n친구에게 가져다주자");

                    if(_PlayerControl.isBdown)
                    {
                        BackGround.gameObject.SetActive(false);
                        Ch02Tuto2Fin = true;
                        ChFruit02 = true;
                    }
                }
            }
        }

        // 챕터 2 - 열매 전달해줬을 때 UI 변경 
        // Cat Cat_AniManage.cs isSleepOut == true
        // Cactus AniManage.cs isSleepOut == true
        Cat_AniManage = Cat.GetComponent<Cat_AniManage>();
        Cactus_AniManage = Cactus.GetComponent<AniManage>();
        if(Cat_AniManage.isSleepOut && !SleepOutCat)
        {
            ChangeMessage(fruitText, "1/1");

            UnableRawImage(Ch02_Sleeping);
            EnableRawImage(Ch02_SleepOut_1);

            SleepOutCat = true;
        }
        if(Cactus_AniManage.isSleepOut && !SleepOutCactus)
        {
            BackGround_Fruit.gameObject.SetActive(false);

            UnableRawImage(Ch02_SleepOut_1);
            EnableRawImage(Ch02_SleepOut_2);

            // 챕터 2 - 선인장 대사 끝났을 때 SleepOut UI 비활성화
            if(_Cactus_Chat.isClear)
            {
                UnableRawImage(Ch02_SleepOut_2);
                SleepOutCactus = true;
            }
        }

        // 챕터 2 - 모니터 켜지는 trigger
        if(MonitorOn && !MonitorUIFin)
        {
            BackGround.gameObject.SetActive(true);
            _PlayerControl.speed = 0f;
            ChangeMessage(messageText, "모니터 안을 확인해보자!");

            if(_PlayerControl.isBdown)
            {
                BackGround.gameObject.SetActive(false);
                _PlayerControl.speed = 4f;
                MonitorUIFin = true;
            }
        }

        // 챕터 2 - 모니터 확인 & 마우스가 없다! trigger
        if(_PlayerState.isCh02MonitorUI && !MonitorCheckFin)
        {
            if(_PlayerControl.isBdown)
            {
                MonitorBcount ++;
                _PlayerControl.isBdown = false;
            }

            if(MonitorBcount == 0)
            {
                BackGround.gameObject.SetActive(true);
                ChangeMessage(messageText, "마우스로 밀어 열매를 드래그해보자!");
                _PlayerControl.speed = 0f;
            }
            else if(MonitorBcount == 1)
            {
                ChangeMessage(messageText, "어라? 마우스가 없어!\n여기서 나가서 마우스를 그려오자!");
                _PlayerControl.speed = 0f;
            }
            else if(MonitorBcount == 2)
            {
                ChangeMessage(messageText, "양쪽 조이스틱을 눌러 볼을 꼬집자!\n꿈에서 깰 수 있을지 몰라");
                _PlayerControl.speed = 0f;
            }
            else if(MonitorBcount == 3)
            {
                BackGround.gameObject.SetActive(false);
                _PlayerControl.speed = 4f;
                MonitorCheckFin = true;

                EnableRawImage(Ch03_Sleeping);

                // 패스스루로 나갈 수 있게 활성화
                _PlayerControl.EnableExitAction();
            }
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

            // 2초 딜레이 후 canSleepOutSuji true
            StartCoroutine(SetCanSleepOutSujiAfterDelay(2.0f));
        }
        if(canSleepOutSuji && !isEnterUIFin)
        {
            BackGround.gameObject.SetActive(true);
            ChangeMessage(messageText, "수지에게 열매를 주려면\nenter키를 밟아보자!");
            
            if(_PlayerControl.isBdown)
            {
                BackGround.gameObject.SetActive(false);
                isEnterUIFin = true;
            }
        }

        // 챕터 3 - 수지가 움직일 때, 수지를 따라가자 활성화 & 깨운 캐릭터 UI 비활성화
        if(_SujiEndingTest.canMove)
        {
            BackGround.gameObject.SetActive(true);
            BackGround_Confirm.gameObject.SetActive(false);
            UnableRawImage(Ch03_SleepOut);

            ChangeMessage(messageText, "수지를 따라가자!");
        }
        if(_SujiEndingTest.IsReachedEndPoint)
        {
            BackGround.gameObject.SetActive(false);
            BackGround_Confirm.gameObject.SetActive(true);
            UnableRawImage(Ch03_SleepOut);
        }

        // 모든 챕터 - SleepOut trigger 충돌했을 때, FruitCount가 없으면 열매없이는 못 깨운다는 UI 활성화 & 트리거 색상 복구
        // if(_ChapterControl.CurrentChapter is Chapter.Chapter1 && !Clock_AniManage.isSleepOut)
        // {
        //     // 챕터 1 - 시계토끼 아직 깨어나지 않음
        // 
        // }

        // 엔딩 캐릭터 대사 이후 나오는 UI 
        if(_EndingBehaviour._isEndChatFin && !isEndFin)
        {
            if(_PlayerControl.isBdown)
            {
                EndBcount ++;
                _PlayerControl.isBdown = false;
            }

            if(EndBcount == 0)
            {
                BackGround.gameObject.SetActive(true);
                ChangeMessage(messageText, "무사히 친구들을 깨웠어!");
            }
            else if(EndBcount == 1)
            {
                BackGround.gameObject.SetActive(true);
                ChangeMessage(messageText, "그 까만 아이는\n왜 친구들을 재운걸까?");
            }
            else if(EndBcount == 2)
            {
                BackGround.gameObject.SetActive(true);
                ChangeMessage(messageText, "사이좋게 지내면 좋을텐데");
            }
            else if(EndBcount == 3)
            {
                StartCoroutine(EndingFinalUI(1.5f));
            }
            
        }
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
        BackGround.gameObject.SetActive(true);
        _PlayerControl.speed = 0f;
        ChangeMessage(messageText, "길을 건너 열매를 찾자!\n(시간을 멈추면 건너기 쉬워질지도?)");

        // 확인 눌렀을 때 비활성화 
        if(_PlayerControl.isBdown)
        {
            BackGround.gameObject.SetActive(false);
            _PlayerControl.speed = 4f;

            BackGround_Fruit.gameObject.SetActive(true);
            ChangeMessage(fruitText, "0/2");
            EnableRawImage(Ch02_Sleeping);
            Ch02Fin = true;
        }
    }

    public void Chapter03_StartUI()
    {
        BackGround.gameObject.SetActive(true);
        _PlayerControl.speed = 0f;
        ChangeMessage(messageText, "마우스로 밀어 열매를 드래그해보자!");

        // 확인 눌렀을 때 비활성화 
        if(_PlayerControl.isBdown)
        {
            BackGround.gameObject.SetActive(false);
            _PlayerControl.speed = 4f;

            EnableRawImage(Ch03_Sleeping);
            Ch03Fin = true;
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

    // TextMeshPro 텍스트 바꾸는 함수 
    public void ChangeMessage(TMP_Text TMP_Text, string newMessage)
    {
        TMP_Text.text = newMessage;
    }

    private IEnumerator SetCanSleepOutSujiAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSleepOutSuji = true;
    }

    private IEnumerator EndingFinalUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        _EndingBehaviour._isEndUIFin = true;
        BackGround.gameObject.SetActive(true);
        BackGround_Confirm.gameObject.SetActive(false);
        ChangeMessage(messageText, "어라?\n몸이 커지고 있어!");

        yield return new WaitForSeconds(delay);

        BackGround.gameObject.SetActive(false);
        isEndFin = true;
    }
}
