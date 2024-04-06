using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffMonitor : MonoBehaviour
{   
    public Material newMaterial; // 변경할 새로운 머테리얼
    public GameObject OffScreen; 

    private Renderer ownRenderer; // 자기 자신의 렌더러

    private void Start()
    {
        ownRenderer = GetComponent<Renderer>(); // 자기 자신의 렌더러 가져오기
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            if (ownRenderer != null && newMaterial != null)
            {
                ownRenderer.material = newMaterial; // 자기 자신의 머테리얼 변경
                OffScreen.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Renderer or new material is not assigned.");
            }
        }
    }
}
