using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public Transform cameraTransform; // ī�޶��� Transform
    public float distanceFromCamera = 1f; // ī�޶�κ����� �Ÿ�

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
        // ī�޶��� ���� ���Ϳ� �Ÿ��� ���Ͽ� ���ϴ� ��ġ�� ����մϴ�.
        Vector3 desiredPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // ���� ��ġ�� Canvas�� �̵���ŵ�ϴ�.
        transform.position = desiredPosition;
        transform.rotation = cameraTransform.rotation;
    }

    // ��� 1.0f -> ������ 0.0f
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
    
    // ���� 0.0f -> ��� 1.0f
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

    // ������ 1.0f -> ������ 0.0f
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
    
    // ���� 0.0f -> ������ 1.0f
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
