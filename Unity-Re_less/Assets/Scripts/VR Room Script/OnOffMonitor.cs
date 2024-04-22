using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffMonitor : MonoBehaviour
{   
    public Material newMaterial; // 변경할 새로운 머테리얼
    public GameObject OffScreen; 
    public GameObject OnScreen; 

    public ParticleSystem PressEffect;

    // 모니터 팝업 1 
    public GameObject Popup_1;
    bool PopupFin = false;

    // UI Trigger
    public bool isScreenOn = false;

    private Renderer ownRenderer; // 자기 자신의 렌더러

    private void Start()
    {
        PressEffect.Play();
        ownRenderer = GetComponent<Renderer>(); // 자기 자신의 렌더러 가져오기
        OnScreen.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            if (ownRenderer != null && newMaterial != null)
            {
                ownRenderer.material = newMaterial; // 자기 자신의 머테리얼 변경
                isScreenOn = true;

                OffScreen.SetActive(false);
                OnScreen.SetActive(true);
                PressEffect.Stop();

                // 모니터 팝업 1 띄우고 지우기
                EnablePopup();
                if(!PopupFin)
                {
                    Invoke("UnablePopup", 5f);
                }
            }
            else
            {
                Debug.LogWarning("Renderer or new material is not assigned.");
            }
        }
    }


    void EnablePopup()
    {
        Popup_1.SetActive(true);
    }

    void UnablePopup()
    {
        Popup_1.SetActive(false);
        PopupFin = true;
    }
}


