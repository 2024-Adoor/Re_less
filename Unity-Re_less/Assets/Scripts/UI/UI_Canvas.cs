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
    public GameObject Player;

    public Texture2D StartTexture; // RawImage에 사용할 텍스처
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

    private Vector2 position = new Vector2(0f, 0f); // RawImage의 위치
    private Vector3 scale = new Vector3(1f, 1f, 1f); // RawImage의 스케일

    private bool isTrigger01 = true;
    private bool isTrigger02 = true;
    private bool isTrigger03 = true;
    private bool isTrigger04 = true;

    void Start()
    {   
        ChapterControl ChControlScript = Player.GetComponent<ChapterControl>();

        // VR 시작 화면
        AddRawImage(StartTexture, position, scale, "VR_UI_Start");

        // 생성한 RawImage 오브젝트 찾기
        GameObject StartRawImage = GameObject.Find("VR_UI_Start");

        if(ChControlScript.Ch01)
        {   
            // Ch01인 경우 Help UI 생성 
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

        // 카메라의 전방 벡터와 거리를 곱하여 원하는 위치를 계산합니다.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // 계산된 위치로 Canvas를 이동시킵니다.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;
        
        // Ch01 & Ch02 Hand Trigger -> 손으로 짚고 올라가라는 UI 생성
        if(PlayerStateScript.isTrigger && isTrigger01)
        {
            AddRawImage(HandTexture, position, scale, "VR_UI_Hand");

            // 생성한 RawImage 오브젝트 찾기
            GameObject HandRawImage = GameObject.Find("VR_UI_Hand");
            StartCoroutine(StartFadeOutAndDestroy(HandRawImage, 0.5f, 3f));

            isTrigger01 = false;
        }

        // 열매카운트 없이 캐릭터와 상호작용 하려고 할 때
        if(PlayerStateScript.fruitCount == 0 && PlayerStateScript.isCharacter && isTrigger02)
        {
            AddRawImage(CharacterTexture, position, scale, "VR_UI_Chracter");

            // 생성한 RawImage 오브젝트 찾기
            GameObject CharacterRawImage = GameObject.Find("VR_UI_Chracter");
            StartCoroutine(StartFadeOutAndDestroy(CharacterRawImage, 0.5f, 3f));

            isTrigger02 = false;
        }

        // X 클릭 & 캐릭터 위치 
        if(PlayerCtrlScript.isXdown && ChControlScript.Ch01 && isTrigger03)
        {
            AddRawImage(FriendHint1, position, scale, "FriendHint");

            // 생성한 RawImage 오브젝트 찾기
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

            // 생성한 RawImage 오브젝트 찾기
            GameObject HintRawImage = GameObject.Find("FriendHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger03 = false;
        }

        // Y 클릭 & 캐릭터 위치 
        if(PlayerCtrlScript.isYdown && ChControlScript.Ch01 && isTrigger04)
        {
            AddRawImage(FruitHint1, position, scale, "FruitHint");

            // 생성한 RawImage 오브젝트 찾기
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

            // 생성한 RawImage 오브젝트 찾기
            GameObject HintRawImage = GameObject.Find("FruitHint");
            StartCoroutine(StartFadeOutAndDestroy(HintRawImage, 0.5f, 3f));

            isTrigger04 = false;
        }
    }

    // RawImage 추가 함수 - 텍스쳐, 위치, 스케일, 오브젝트이름
    void AddRawImage(Texture2D texture, Vector2 poisiton, Vector3 scale, String ObjectName)
    {
        // RawImage 생성
        GameObject rawImageObject = new GameObject(ObjectName);
        rawImageObject.transform.SetParent(transform, false); // Canvas에 배치

        // RawImage 컴포넌트 추가
        RawImage rawImageComponent = rawImageObject.AddComponent<RawImage>();
        rawImageComponent.texture = texture; // 텍스처 할당

        // RawImage의 RectTransform 가져오기
        RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();

        // RawImage의 위치 설정
        rectTransform.anchoredPosition = position;

        // 원본 이미지 크기로 설정
        rawImageComponent.SetNativeSize();

        // 스케일 조정
        rawImageObject.transform.localScale = scale;
    }

    // RawImage 삭제 함수 - 오브젝트, 페이드 시간, 딜레이 시간
    IEnumerator StartFadeOutAndDestroy(GameObject obj, float fadeTime, float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // 지연 실행

        // RawImage 컴포넌트 가져오기
        RawImage rawImageComponent = obj.GetComponent<RawImage>();

        // 투명도를 서서히 줄이는 애니메이션
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeTime;
            rawImageComponent.color = new Color(rawImageComponent.color.r, rawImageComponent.color.g, rawImageComponent.color.b, alpha);
            yield return null;
        }

        // 오브젝트 파괴
        Destroy(obj);
    }

    // Ch01 ) Start UI 이후 Help UI 띄우기  
    IEnumerator DestroyStartShowHelp(GameObject startRawImage, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Destroy(startRawImage);

        // VR Help 화면
        AddRawImage(HelpTexture, position, scale, "VR_UI_Help");

        // 생성한 RawImage 오브젝트 찾기
        GameObject HelpRawImage = GameObject.Find("VR_UI_Help");

        yield return StartCoroutine(StartFadeOutAndDestroy(HelpRawImage, 0.5f, 3f));
    }

    // Ch02 ) Hint UI 2개 연달아 띄우기  
    IEnumerator TwoHints(Texture2D texture1, Texture2D texture2)
    {   
        // 첫번째 힌트 
        AddRawImage(texture1, position, scale, "Hint1");
        // 생성한 RawImage 오브젝트 찾기
        GameObject Hint1RawImage = GameObject.Find("Hint1");
        yield return new WaitForSeconds(3f);
        Destroy(Hint1RawImage);

        // 첫번째 힌트 
        AddRawImage(texture2, position, scale, "Hint2");
        // 생성한 RawImage 오브젝트 찾기
        GameObject Hint2RawImage = GameObject.Find("Hint2");
        yield return StartCoroutine(StartFadeOutAndDestroy(Hint2RawImage, 0.5f, 3f));
    }
}
