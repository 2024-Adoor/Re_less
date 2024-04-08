using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{   
    // Fruit ī��Ʈ & Ÿ ĳ���� ��ȣ�ۿ� ���� ��ũ��Ʈ�Դϴ�. 
    public int fruitCount; 
    public bool isTrigger = false;
    public bool isCharacter = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Fruit"))
        {
            fruitCount++;

            // �浹�� ������Ʈ ����
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");
        }
        else if(other.gameObject.CompareTag("HandTrigger"))
        {
            isTrigger = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && fruitCount > 0)
        {
            isCharacter = true;
        }
    }
}
