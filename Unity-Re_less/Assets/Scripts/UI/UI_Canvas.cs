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
    public GameObject Player;

    public Texture2D StartTexture; // RawImage�� ����� �ؽ�ó
    public Texture2D HelpTexture;
    public Texture2D HandTexture;
    public Texture2D CharacterTexture;

    // Friend Hint 1~4 
    public Texture2D FriendHint1;
    public Texture2D FriendHint2;
    public Texture2D FriendHint3;
    public Texture2D FriendHint4;

    // Fruit Hint 1~4  
    public Texture2D FruitHint1;
    public Texture2D FruitHint2;
    public Texture2D FruitHint3;
    public Texture2D FruitHint4;

    private Vector2 position = new Vector2(0f, 0f); // RawImage�� ��ġ
    private Vector3 scale = new Vector3(1f, 1f, 1f); // RawImage�� ������

    private bool isTrigger01 = true;
    private bool isTrigger02 = true;
    private bool isTrigger03 = true;
    private bool isTrigger04 = true;

    void Start()
    {   
        ChapterControl ChControlScript = Player.GetComponent<ChapterControl>();

        // VR ���� ȭ��
        AddRawImage(StartTexture, position, scale, "VR_UI_Start");

        // ������ RawImage ������Ʈ ã��
        GameObject StartRawImage = GameObject.Find("VR_UI_Start");

        if(ChControlScript.Ch01)
        {   
            // Ch01�� ��� Help UI ���� 
            StartCoroutine(DestroyStartShowHelp(StartRawImage, 3f));
        }
        else if(ChControlScript.Ch02 || ChControlScript.Ch03)
        {   
            StartCoroutine(StartFadeOutAndDestroy(StartRawImage, 0.5f, 3f));
        }
        
    }

    void Update()
    {   
        ChapterControl ChControlScript = Player.GetComponent<ChapterControl>();
        PlayerState PlayerStateScript = Player.GetComponent<PlayerState>();
        PlayerControl PlayerCtrlScript = Player.GetComponent<PlayerControl>();

        // ī�޶��� ���� ���Ϳ� �Ÿ��� ���Ͽ� ���ϴ� ��ġ�� ����մϴ�.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // ���� ��ġ�� Canvas�� �̵���ŵ�ϴ�.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;
        
        // Ch01 & Ch02 Hand Trigger -> ������ ¤�� �ö󰡶�� UI ����
        if(PlayerStateScript.isTrigger && isTrigger01)
        {
            AddRawImage(HandTexture, position, scale, "VR_UI_Hand");

            // ������ RawImage ������Ʈ ã��
            GameObject HandRawImage = GameObject.Find("VR_UI_Hand");
            StartCoroutine(StartFadeOutAndDestroy(HandRawImage, 0.5f, 3f));

            isTrigger01 = false;
        }

        // ����ī��Ʈ ���� ĳ���Ϳ� ��ȣ�ۿ� �Ϸ��� �� ��
        if(PlayerStateScript.fruitCount == 0 && PlayerStateScript.isCharacter && isTrigger02)
        {
            AddRawImage(CharacterTexture, position, scale, "VR_UI_Chracter");

            // ������ RawImage ������Ʈ ã��
            GameObject CharacterRawImage = GameObject.Find("VR_UI_Chracter");
            StartCoroutine(StartFadeOutAndDestroy(CharacterRawImage, 0.5f, 3f));

            isTrigger02 = false;
        }

        // X Ŭ�� & ĳ���� ��ġ 
        if(PlayerCtrlScript.isXdown && ChControlScript.Ch01 && isTrigger03)
        {
            AddRawImage(FriendHint1, position, scale, "FriendHint");

            // ������ RawImage ������Ʈ ã��
            GameObject HintRawImage = GameObject.Find("FriendHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger03 = false;
        }
        else if(PlayerCtrlScript.isXdown && ChControlScript.Ch02 && isTrigger03)
        {
            StartCoroutine(TwoHints(FriendHint2, FriendHint3));
            isTrigger03 = false;
        }
        else if(PlayerCtrlScript.isXdown && ChControlScript.Ch03 && isTrigger03)
        {
            AddRawImage(FriendHint4, position, scale, "FriendHint");

            // ������ RawImage ������Ʈ ã��
            GameObject HintRawImage = GameObject.Find("FriendHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger03 = false;
        }

        // Y Ŭ�� & ĳ���� ��ġ 
        if(PlayerCtrlScript.isYdown && ChControlScript.Ch01 && isTrigger04)
        {
            AddRawImage(FruitHint1, position, scale, "FruitHint");

            // ������ RawImage ������Ʈ ã��
            GameObject HintRawImage = GameObject.Find("FruitHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger04 = false;
        }
        else if(PlayerCtrlScript.isYdown && ChControlScript.Ch02 && isTrigger04)
        {
            StartCoroutine(TwoHints(FruitHint2, FruitHint3));
            isTrigger04 = false;
        }
        else if(PlayerCtrlScript.isYdown && ChControlScript.Ch03 && isTrigger04)
        {
            AddRawImage(FruitHint4, position, scale, "FruitHint");

            // ������ RawImage ������Ʈ ã��
            GameObject HintRawImage = GameObject.Find("FruitHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger04 = false;
        }
    }

    // RawImage �߰� �Լ� - �ؽ���, ��ġ, ������, ������Ʈ�̸�
    void AddRawImage(Texture2D texture, Vector2 poisiton, Vector3 scale, String ObjectName)
    {
        // RawImage ����
        GameObject rawImageObject = new GameObject(ObjectName);
        rawImageObject.transform.SetParent(transform, false); // Canvas�� ��ġ

        // RawImage ������Ʈ �߰�
        RawImage rawImageComponent = rawImageObject.AddComponent<RawImage>();
        rawImageComponent.texture = texture; // �ؽ�ó �Ҵ�

        // RawImage�� RectTransform ��������
        RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();

        // RawImage�� ��ġ ����
        rectTransform.anchoredPosition = position;

        // ���� �̹��� ũ��� ����
        rawImageComponent.SetNativeSize();

        // ������ ����
        rawImageObject.transform.localScale = scale;
    }

    // RawImage ���� �Լ� - ������Ʈ, ���̵� �ð�, ������ �ð�
    IEnumerator StartFadeOutAndDestroy(GameObject obj, float fadeTime, float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // ���� ����

        // RawImage ������Ʈ ��������
        RawImage rawImageComponent = obj.GetComponent<RawImage>();

        // ������ ������ ���̴� �ִϸ��̼�
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeTime;
            rawImageComponent.color = new Color(rawImageComponent.color.r, rawImageComponent.color.g, rawImageComponent.color.b, alpha);
            yield return null;
        }

        // ������Ʈ �ı�
        Destroy(obj);
    }

    // Ch01 ) Start UI ���� Help UI ����  
    IEnumerator DestroyStartShowHelp(GameObject startRawImage, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Destroy(startRawImage);

        // VR Help ȭ��
        AddRawImage(HelpTexture, position, scale, "VR_UI_Help");

        // ������ RawImage ������Ʈ ã��
        GameObject HelpRawImage = GameObject.Find("VR_UI_Help");

        yield return StartCoroutine(StartFadeOutAndDestroy(HelpRawImage, 0.5f, 3f));
    }

    // Ch02 ) Hint UI 2�� ���޾� ����  
    IEnumerator TwoHints(Texture2D texture1, Texture2D texture2)
    {   
        // ù��° ��Ʈ 
        AddRawImage(texture1, position, scale, "Hint1");
        // ������ RawImage ������Ʈ ã��
        GameObject Hint1RawImage = GameObject.Find("Hint1");
        yield return new WaitForSeconds(3f);
        Destroy(Hint1RawImage);

        // ù��° ��Ʈ 
        AddRawImage(texture2, position, scale, "Hint2");
        // ������ RawImage ������Ʈ ã��
        GameObject Hint2RawImage = GameObject.Find("Hint2");
        yield return StartCoroutine(StartFadeOutAndDestroy(Hint2RawImage, 0.5f, 3f));
    }
}
