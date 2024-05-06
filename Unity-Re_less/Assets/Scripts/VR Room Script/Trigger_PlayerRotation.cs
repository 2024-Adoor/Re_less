using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class Trigger_PlayerRotation : MonoBehaviour
    {
        public GameObject Player;
        public float rotationSpeed = 5f; // 회전 속도

        bool isRotate = false;
        Quaternion targetRotation;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(isRotate)
            {
                RotatePlayer();
            }
        }

        // 캐릭터 충돌시 캐릭터가 타겟을 향하게 회전시키기
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                isRotate = true;

                // 플레이어를 y축으로 90도 회전하기 위한 목표 회전 각도 계산
                targetRotation = Quaternion.Euler(0, -60, 0) * Player.transform.rotation;
            }
        }

        void RotatePlayer()
        {
            // 부드럽게 회전하기
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
            // 목표 회전 각도에 도달하면 회전 종료
            if(Quaternion.Angle(Player.transform.rotation, targetRotation) < 1.0f)
            {
                isRotate = false;
            }
        }

    }


}
