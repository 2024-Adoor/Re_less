using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffMonitor : MonoBehaviour
{   
    public Material newMaterial; // ������ ���ο� ���׸���
    public GameObject OffScreen; 
    public GameObject OnScreen; 

    public ParticleSystem PressEffect;

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
            }
            else
            {
                Debug.LogWarning("Renderer or new material is not assigned.");
            }
        }
    }
}
