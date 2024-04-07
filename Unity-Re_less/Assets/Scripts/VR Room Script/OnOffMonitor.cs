using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffMonitor : MonoBehaviour
{   
    public Material newMaterial; // ������ ���ο� ���׸���
    public GameObject OffScreen; 
    public GameObject OnScreen; 

    private Renderer ownRenderer; // �ڱ� �ڽ��� ������

    private void Start()
    {
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
                OffScreen.SetActive(false);
                OnScreen.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Renderer or new material is not assigned.");
            }
        }
    }
}
