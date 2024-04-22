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

    // Chapter 01 UI RawImage
    public RawImage Ch01_Tutorial_1;
    public RawImage Ch01_Tutorial_2;
    public RawImage Ch01_Tutorial_Jump;     // Trigger�� �۵�
    public RawImage Ch01_Tutorial_Friend;   // Trigger�� �۵�

    // Chapter 02 UI RawImage
    public RawImage Ch02_Tutorial_1;
    public RawImage Ch02_Tutorial_2;
    public RawImage Ch02_Watch;             // Trigger�� �۵�
    public RawImage Ch02_Door;              // Trigger�� �۵� 

    // Chatper 03 Ui RawImage
    public RawImage Ch03_Tutorial_1;
    public RawImage Ch03_Suji;              // Trigger�� �۵�
    public RawImage Ch03_End;               // Trigger�� �۵� 

    // Can't Awake RawImage
    public RawImage CantAwake;

    // ���� UI RawImage
    public RawImage fruit_0_1;
    public RawImage fruit_1_1;
    public RawImage fruit_0_2;
    public RawImage fruit_1_2;
    public RawImage fruit_2_2;

    // UI ��Ʈ���� ���� bool ��
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
        
        // ��ü RawImage ��Ȱ��ȭ
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


        // é�� 1 ���۽� UI â 
        if(_ChapterControl.Ch01)
        {
            _PlayerControl.speed = 0f;

            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableCh01Tutorial1", 0.2f);
            Invoke("UnableCh01Tutorial1", 3.2f);
            Invoke("EnableCh01Tutorial2", 3.2f);    // �÷��̾� �ӵ� �ʱ�ȭ (4f)
            Invoke("UnableCh01Tutorial2", 6.2f);    // Ʃ�丮�� UI ��Ȱ��ȭ & ���� UI Ȱ��ȭ 
        }
        // é�� 2 ���۽� UI â 
        if(_ChapterControl.Ch02)
        {
            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableCh02Tutorial1", 0.2f);
            Invoke("UnableCh02Tutorial1", 3.2f);    // Ʃ�丮�� UI ��Ȱ��ȭ & ���� UI Ȱ��ȭ
        }
        // é�� 3 ���۽� UI â 
        if(_ChapterControl.Ch03)
        {
            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
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

        // ī�޶��� ���� ���Ϳ� �Ÿ��� ���Ͽ� ���ϴ� ��ġ�� ����մϴ�.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // ���� ��ġ�� Canvas�� �̵���ŵ�ϴ�.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;


        // é�� 1 - ���� Ʃ�丮�� UI 
        if(_PlayerState.isJumpUI && !JumpUIFin)
        {
            EnableRawImage(Ch01_Tutorial_Jump);
            _PlayerControl.speed = 0f;
            // 3�� �ڿ� UnableRawImage(Ch01_Tutorial_Jump) && _PlayerControl.speed = 4.0f
            Invoke("Ch01Jump_Speed", 3f);
        }

        // é�� 1 - ���� ���� Ʃ�丮�� UI 
        if(_PlayerState.isFriendUI && !FriendUIFin)
        {
            EnableRawImage(Ch01_Tutorial_Friend);
            _PlayerControl.speed = 0f;
            // 2�� �ڿ� UnableRawImage(Ch01_Tutorial_Friend) && _PlayerControl.speed = 4.0f
            Invoke("Ch01Jump_Friend", 2f);
        }

        // é�� 1 - ���� �Ծ��� �� UI ����
        if(_ChapterControl.Ch01 && _PlayerState.fruitCount == 1)
        {
            UnableRawImage(fruit_0_1);
            EnableRawImage(fruit_1_1);
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
        
        // é�� 2 - ���� �Ծ��� �� UI ����
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

                // ģ������ ���������� UI Invoke 
                if(!Ch02Tuto2Fin)
                {
                    EnableRawImage(Ch02_Tutorial_2);
                    Invoke("UnableCh02Tutorial2", 3f);
                }
                
            }
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
        // �������� Ȱ��ȭ�մϴ�.
        yourRawImage.enabled = true;
    }

    void UnableRawImage(RawImage yourRawImage)
    {
        // �������� Ȱ��ȭ�մϴ�.
        yourRawImage.enabled = false;
    }

    /**************************************************************************************************/

    // é�� 1 Ʃ�丮�� UI Invoke ��
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

        // ���� UI Ȱ��ȭ 
        EnableRawImage(fruit_0_1);
    }

    // é�� 2 Ʃ�丮�� UI Invoke ��
    void EnableCh02Tutorial1()
    {
        EnableRawImage(Ch02_Tutorial_1);
    }

    void UnableCh02Tutorial1()
    {
        UnableRawImage(Ch02_Tutorial_1);

        // ���� UI Ȱ��ȭ 
        EnableRawImage(fruit_0_2);
    }

    void UnableCh02Tutorial2()
    {
        UnableRawImage(Ch02_Tutorial_2);
        Ch02Tuto2Fin = true;
    }

    // é�� 3 Ʃ�丮�� UI Invoke ��
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

    // é��1 ���� Ʃ�丮�� UI ��Ȱ��ȭ & �÷��̾� ���ǵ� �ʱ�ȭ 
    void Ch01Jump_Speed()
    {
        UnableRawImage(Ch01_Tutorial_Jump);
        _PlayerControl.speed = 4.0f;
        JumpUIFin = true;
    }

    // é��1 ���� ���� Ʃ�丮�� UI ��Ȱ��ȭ & �÷��̾� ���ǵ� �ʱ�ȭ 
    void Ch01Jump_Friend()
    {
        UnableRawImage(Ch01_Tutorial_Friend);
        _PlayerControl.speed = 4.0f;
        FriendUIFin = true;
    }

    // é��2 �� UI ��Ȱ��ȭ & �÷��̾� ���ǵ� �ʱ�ȭ 
    void Ch02_Door_Speed()
    {
        UnableRawImage(Ch02_Door);
        _PlayerControl.speed = 4.0f;
        DoorUIFin = true;
    }


    
}
