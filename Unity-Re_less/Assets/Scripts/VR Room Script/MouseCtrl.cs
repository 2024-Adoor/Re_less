using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless;
using Unity.VisualScripting;

public class MouseCtrl : MonoBehaviour
{
    public Transform ScreenObject; 
    public GameObject OnPopup;
    public GameObject Player;
    ChapterControl _ChapterControl;

    public float maxY = 42.2f; // B 오브젝트의 최대 Y 위치
    public float minY = 31.1f; // B 오브젝트의 최소 Y 위치
    public float maxZ = -51.9f; // B 오브젝트의 최대 Z 위치
    public float minZ = -71.0f; // B 오브젝트의 최소 Z 위치

    private Vector3 lastPositionA; // A 오브젝트의 마지막 위치
    private bool collisionDetected = false; // 충돌 감지 여부를 저장하는 변수

    public GameObject Ch03_Fruit;
    
    private Ch03_FruitSnap Ch03FruitScript;

    void Start()
    {
        lastPositionA = transform.position; // 초기 위치 설정

        if (Ch03_Fruit != null)
        {
            Ch03FruitScript = Ch03_Fruit.GetComponent<Ch03_FruitSnap>();
        }
    }

    void Update()
    {
        if (!Ch03FruitScript.IsUnityNull())
        {
            collisionDetected = Ch03FruitScript.isDetected;

            if (!collisionDetected) // 충돌이 감지되지 않은 경우에만 움직임 처리
            {
                Vector3 currentPositionA = transform.position; // A 오브젝트의 현재 위치

                // A 오브젝트의 이동량 계산
                Vector3 displacement = currentPositionA - lastPositionA;

                // B 오브젝트의 위치를 이동량에 따라 조정 (Z 값은 고정)
                Vector3 newPositionB = ScreenObject.position;
                
                newPositionB.z += displacement.z;
                newPositionB.y += displacement.x; // Y 값에 Z 변화량 적용
                newPositionB.y = Mathf.Clamp(newPositionB.y, minY, maxY); // Y 위치를 최대 및 최소 값으로 제한
                newPositionB.z = Mathf.Clamp(newPositionB.z, minZ, maxZ); // Z 위치를 최대 및 최소 값으로 제한
                ScreenObject.position = newPositionB;

                lastPositionA = currentPositionA; // 이전 위치 갱신
            } 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌할 때만 isKinematic을  비활성화
            GetComponent<Rigidbody>().isKinematic = false;
            if(Player != null)
            {
                _ChapterControl = Player.GetComponent<ChapterControl>();
            }

            // 챕터 3일때만 팝업 비활성화
            if(_ChapterControl.CurrentChapter is Chapter.Chapter3)
            {
                OnPopup.SetActive(false);
            }
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌이 끝났을 때 isKinematic을 활성화
            GetComponent<Rigidbody>().isKinematic = true;
        }
}
}