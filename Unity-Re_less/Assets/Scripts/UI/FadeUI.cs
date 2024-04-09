using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform
    public float distanceFromCamera = 1f; // 카메라로부터의 거리

    public Image WhitePanel;
    public Image BlackPanel;
    float time = 0f;
    float FadeTime = 1f;

    bool isFadeIn = false;

    void Start()
    {
        WhitePanel.gameObject.SetActive(false);
        BlackPanel.gameObject.SetActive(false);

        // WhiteFadeIn();
    }

    void Update()
    {
        // 카메라의 전방 벡터와 거리를 곱하여 원하는 위치를 계산합니다.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // 계산된 위치로 Canvas를 이동시킵니다.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;
    }

    // 흰색 1.0f -> 없어짐 0.0f
    public void WhiteFadeIn()
    {
        StartCoroutine(WhiteFadeInFlow());
    }
    IEnumerator WhiteFadeInFlow()
    {
        WhitePanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = WhitePanel.color;
        
        while(alpha.a <= 1f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            WhitePanel.color = alpha;
            yield return null;
        }
    }
    
    // 없음 0.0f -> 흰색 1.0f
    public void WhiteFadeOut()
    {
        StartCoroutine(WhiteFadeOutFlow());
    }
    IEnumerator WhiteFadeOutFlow()
    {
        WhitePanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = WhitePanel.color;
        
        while(alpha.a >= 0f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            WhitePanel.color = alpha;
            yield return null;
        }
    }

    // 검은색 1.0f -> 없어짐 0.0f
    public void BlackFadeIn()
    {
        StartCoroutine(BlackFadeInFlow());
    }
    IEnumerator BlackFadeInFlow()
    {
        BlackPanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = BlackPanel.color;
        
        while(alpha.a <= 1f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            BlackPanel.color = alpha;
            yield return null;
        }
    }
    
    // 없음 0.0f -> 검은색 1.0f
    public void BlackFadeOut()
    {
        StartCoroutine(BlackFadeOutFlow());
    }
    IEnumerator BlackFadeOutFlow()
    {
        BlackPanel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = BlackPanel.color;
        
        while(alpha.a >= 0f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            BlackPanel.color = alpha;
            yield return null;
        }
    }
}
