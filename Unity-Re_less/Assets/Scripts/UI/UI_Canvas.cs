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

    // UI ��Ʈ���� ���� bool ��
    bool JumpUIFin = false;
    bool FriendUIFin = false;

    void Start()
    {
        _PlayerControl = Player.GetComponent<PlayerControl>();
        _ChapterControl = Player.GetComponent<ChapterControl>();

        if(_ChapterControl.Ch01)
        {
            _PlayerControl.speed = 0f;

            UnableRawImage(Tutorial_1);
            UnableRawImage(Tutorial_2);
            UnableRawImage(Tutorial_3);
            UnableRawImage(Tutorial_4);
            UnableRawImage(Tutorial_5);
            UnableRawImage(Tutorial_6);
            UnableRawImage(disAwake);

            // �����ϰ� 1�� �ڿ� Chapter_Start render Ȱ��ȭ -> 3�� �� render ��Ȱ��ȭ 
            Invoke("EnableTutorial1", 0.5f);
            Invoke("UnableTutorial1", 3.5f);
            Invoke("EnableTutorial3", 3.5f);
            Invoke("UnableTutorial3", 6.5f);
            Invoke("EnableTutorial4", 6.5f);
            Invoke("UnableTutorial4", 9.5f);
        }
    }

    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();

        // ī�޶��� ���� ���Ϳ� �Ÿ��� ���Ͽ� ���ϴ� ��ġ�� ����մϴ�.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // ���� ��ġ�� Canvas�� �̵���ŵ�ϴ�.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;

        if(_PlayerState.isJumpUI && !JumpUIFin)
        {
            EnableRawImage(Tutorial_5);
            _PlayerControl.speed = 0f;
            // 3�� �ڿ� UnableRawImage(Tutorial_5) && _PlayerControl.speed = 4.0f
            Invoke("Tutorial5_Speed", 3f);
        }

        if(_PlayerState.isFriendUI && !FriendUIFin)
        {
            EnableRawImage(Tutorial_6);
            _PlayerControl.speed = 0f;
            // 3�� �ڿ� UnableRawImage(Tutorial_6) && _PlayerControl.speed = 4.0f
            Invoke("Tutorial6_Speed", 2f);
        }
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
}
