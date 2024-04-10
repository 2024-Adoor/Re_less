using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch03_FruitSnap : MonoBehaviour
{   
    public bool isDetected = false;
    public GameObject suji;

    public ParticleSystem PressEffect;
    public GameObject EnterPopup;
    Renderer EnterPopupRender;

    void Start()
    {
        EnterPopupRender = EnterPopup.GetComponent<Renderer>();
        
        // 렌더 끄기 
        EnterPopupRender.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character")) // 충돌한 오브젝트가 Character 태그를 가진 경우
        {
            isDetected = true;
            PressEffect.Play();

            // 렌더 켜기 
            EnterPopupRender.enabled = true;

            Vector3 newPosition = transform.position; // 현재 위치
            newPosition.y = suji.transform.position.y; // Y 좌표를 suji의 Y 좌표로 변경
            newPosition.z = suji.transform.position.z; // Z 좌표를 suji의 Z 좌표로 변경
            transform.position = newPosition; // 변경된 위치 적용

        }
    }
}

