using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reless
{
    public class Trigger_PlayerRotation : MonoBehaviour
    {
        public GameObject Player;
        public float rotationSpeed = 5f; // ȸ�� �ӵ�

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

        // ĳ���� �浹�� ĳ���Ͱ� Ÿ���� ���ϰ� ȸ����Ű��
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                isRotate = true;

                // �÷��̾ y������ 90�� ȸ���ϱ� ���� ��ǥ ȸ�� ���� ���
                targetRotation = Quaternion.Euler(0, -60, 0) * Player.transform.rotation;
            }
        }

        void RotatePlayer()
        {
            // �ε巴�� ȸ���ϱ�
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
            // ��ǥ ȸ�� ������ �����ϸ� ȸ�� ����
            if(Quaternion.Angle(Player.transform.rotation, targetRotation) < 1.0f)
            {
                isRotate = false;
            }
        }

    }


}
