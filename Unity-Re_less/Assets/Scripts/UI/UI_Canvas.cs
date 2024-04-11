using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Reless;

public class UI_Canvas : MonoBehaviour
{
    public Transform cameraTransform; // ī�޶��� Transform
    public float distanceFromCamera = 1f; // ī�޶�κ����� �Ÿ�

    // �÷��̾�
    public GameObject Player;
    PlayerControl _PlayerControl;
    PlayerState _PlayerState;
    ChapterControl _ChapterControl;

    // Tutorial RawImage
    public RawImage Tutorial_1;
    public RawImage Tutorial_2;
    public RawImage Tutorial_3;
    public RawImage Tutorial_4;
    public RawImage Tutorial_5;
    public RawImage Tutorial_6;

    // Can't Awake RawImage
    public RawImage disAwake;

    // Chapter 02 UI RawImage
    public RawImage Ch02_Watch;
    public RawImage Ch02_Door;
    public RawImage Ch02_Jump;

    // Chatper 03 Ui RawImage
    public RawImage Ch03_FirstUI;
    public RawImage Ch03_Mouse;
    public RawImage Ch03_Suji;
    public RawImage Ch03_End;

    // UI ��Ʈ���� ���� bool ��
    bool JumpUIFin = false;
    bool FriendUIFin = false;
    bool WatchUIFin = false;
    bool DoorUIFin = false;
    bool Ch02JumpUIFin = false;
    bool MouseUIFin = false;
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
        
        UnableRawImage(Tutorial_1);
        UnableRawImage(Tutorial_2);
        UnableRawImage(Tutorial_3);
        UnableRawImage(Tutorial_4);
        UnableRawImage(Tutorial_5);
        UnableRawImage(Tutorial_6);
        UnableRawImage(disAwake);
        UnableRawImage(Ch02_Watch);
        UnableRawImage(Ch02_Door);
        UnableRawImage(Ch02_Jump);
        UnableRawImage(Ch03_FirstUI);
        UnableRawImage(Ch03_Mouse);
        UnableRawImage(Ch03_Suji);
        UnableRawImage(Ch03_End);

        if(_ChapterControl.Ch01)
        {
            _PlayerControl.speed = 0f;

            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableTutorial1", 0.2f);
            Invoke("UnableTutorial1", 3.5f);
            Invoke("EnableTutorial3", 3.5f);
            Invoke("UnableTutorial3", 6.5f);
            Invoke("EnableTutorial4", 6.5f);
            Invoke("UnableTutorial4", 9.5f);
        }
        if(_ChapterControl.Ch02)
        {
            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableTutorial1", 0.2f);
            Invoke("UnableTutorial1", 3.2f);
        }
        if(_ChapterControl.Ch03)
        {
            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableCh03_FirstUI", 0.2f);
            Invoke("UnableCh03_FirstUI", 3.2f);
        }
    }

    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();
        _ChapterControl = Player.GetComponent<ChapterControl>();

        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        OnOffMonitor _OnOffMonitor = MonitorButton.GetComponent<OnOffMonitor>();

        // ī�޶��� ���� ���Ϳ� �Ÿ��� ���Ͽ� ���ϴ� ��ġ�� ����մϴ�.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // ���� ��ġ�� Canvas�� �̵���ŵ�ϴ�.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;

        // é�� 1 - ���� Ʃ�丮�� UI 
        if(_PlayerState.isJumpUI && !JumpUIFin)
        {
            EnableRawImage(Tutorial_5);
            _PlayerControl.speed = 0f;
            // 3�� �ڿ� UnableRawImage(Tutorial_5) && _PlayerControl.speed = 4.0f
            Invoke("Tutorial5_Speed", 3f);
        }

        // é�� 1 - ���� ���� Ʃ�丮�� UI 
        if(_PlayerState.isFriendUI && !FriendUIFin)
        {
            EnableRawImage(Tutorial_6);
            _PlayerControl.speed = 0f;
            // 3�� �ڿ� UnableRawImage(Tutorial_6) && _PlayerControl.speed = 4.0f
            Invoke("Tutorial6_Speed", 2f);
        }

        // é�� 2 - ������ ���� ��, �ð�  UI 
        if(_ChapterControl.CH02_RespawnCount > 0 && !WatchUIFin)
        {
            EnableRawImage(Ch02_Watch);

            // �ð谡 �ν����� �� UI ���� 
            if(Watch == null)
            {
                UnableRawImage(Ch02_Watch);
                WatchUIFin = true;
            }
        }

        // é�� 2 - �� �տ� ���� ��, �� UI
        if(_PlayerState.isDoorUI && !DoorUIFin)
        {
            EnableRawImage(Ch02_Door);
            _PlayerControl.speed = 0f;
            // 2�� �ڿ� UnableRawImage(Ch02_Door) && _PlayerControl.speed = 4.0f
            Invoke("Ch02_Door_Speed", 2f);
        }

        // é�� 2 - �� UI Fin && �ٸ� �ö��� �� 
        if(DoorUIFin && _PlayerState.isCh02JumpUI && !Ch02JumpUIFin)
        {
            EnableRawImage(Ch02_Jump);
            _PlayerControl.speed = 0f;
            // 2�� �ڿ� UnableRawImage(Ch02_Jump) && _PlayerControl.speed = 4.0f
            Invoke("Ch02_Jump_Speed", 2f);
        }

        // é�� 3 - ��ũ�� ������ ��, ���콺 UI 
        if(_OnOffMonitor.isScreenOn && !MouseUIFin)
        {
            EnableRawImage(Ch03_Mouse);
            Invoke("UnableCh03_Mouse", 2f);
        }

        // é�� 3 - ������ ������ ��, ���� UI
        if(_SujiEndingTest.canMove)
        {
            EnableRawImage(Ch03_Suji);
        }
        else
        {
            UnableRawImage(Ch03_Suji);
        }

        // é�� 3 - ������ ���� ��, ���� UI 
        if(_PlayerState.isTeleport && !EndUIFin)
        {
            EnableRawImage(Ch03_End);
            Invoke("UnableCh03_End", 2f);
        }

        // ��� é�� - FruitCount < 0 �϶� ĳ���Ϳ� �浹, disAwake Ȱ��ȭ 
        if(_PlayerState.fruitCount < 0 && _PlayerState.isDisawakeUI_Trigger)
        {
            EnableRawImage(disAwake);
        }
        else
        {
            UnableRawImage(disAwake);
        }
    }

    void EnableRawImage(RawImage yourRawImage)
    {
        // �������� Ȱ��ȭ�մϴ�.
        yourRawImage.enabled = true;
    }

    void UnableRawImage(RawImage yourRawImage)
    {
        // �������� Ȱ��ȭ�մϴ�.
        yourRawImage.enabled = false;
    }

    

    void EnableTutorial1()
    {
        EnableRawImage(Tutorial_1);
    }

    void UnableTutorial1()
    {
        UnableRawImage(Tutorial_1);
    }

    void EnableTutorial2()
    {
        EnableRawImage(Tutorial_2);
    }

    void UnableTutorial2()
    {
        UnableRawImage(Tutorial_2);
    }

    void EnableTutorial3()
    {
        EnableRawImage(Tutorial_3);
    }

    void UnableTutorial3()
    {
        UnableRawImage(Tutorial_3);
    }

    void EnableTutorial4()
    {
        EnableRawImage(Tutorial_4);
        _PlayerControl = Player.GetComponent<PlayerControl>();

        _PlayerControl.speed = 4.0f;
    }

    void UnableTutorial4()
    {
        UnableRawImage(Tutorial_4);
    }

    void EnableCh03_FirstUI()
    {
        EnableRawImage(Ch03_FirstUI);
    }

    void UnableCh03_FirstUI()
    {
        UnableRawImage(Ch03_FirstUI);
    }

    void UnableCh03_Mouse()
    {
        UnableRawImage(Ch03_Mouse);
        MouseUIFin = true;
    }
    
    void UnableCh03_End()
    {
        UnableRawImage(Ch03_End);
        EndUIFin = true;
    }

    // ******************************************************************************* // 

    void Tutorial5_Speed()
    {
        UnableRawImage(Tutorial_5);
        _PlayerControl.speed = 4.0f;
        JumpUIFin = true;
    }

    void Tutorial6_Speed()
    {
        UnableRawImage(Tutorial_6);
        _PlayerControl.speed = 4.0f;
        FriendUIFin = true;
    }

    void Ch02_Door_Speed()
    {
        UnableRawImage(Ch02_Door);
        _PlayerControl.speed = 4.0f;
        DoorUIFin = true;
    }

    void Ch02_Jump_Speed()
    {
        UnableRawImage(Ch02_Jump);
        _PlayerControl.speed = 4.0f;
        Ch02JumpUIFin = true;
    }

    
}
