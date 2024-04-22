using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffMonitor : MonoBehaviour
{   
    public Material newMaterial; // ������ ���ο� ���׸���
    public GameObject OffScreen; 
    public GameObject OnScreen; 

    public ParticleSystem PressEffect;

    // ����� �˾� 1 
    public GameObject Popup_1;
    bool PopupFin = false;

    // UI Trigger
    public bool isScreenOn = false;

    private Renderer ownRenderer; // �ڱ� �ڽ��� ������

    private void Start()
    {
        PressEffect.Play();
        ownRenderer = GetComponent<Renderer>(); // �ڱ� �ڽ��� ������ ��������
        OnScreen.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �÷��̾�� �浹�ߴ��� Ȯ��
        {
            if (ownRenderer != null && newMaterial != null)
            {
                ownRenderer.material = newMaterial; // �ڱ� �ڽ��� ���׸��� ����
                isScreenOn = true;

                OffScreen.SetActive(false);
                OnScreen.SetActive(true);
                PressEffect.Stop();

                // ����� �˾� 1 ���� �����
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


