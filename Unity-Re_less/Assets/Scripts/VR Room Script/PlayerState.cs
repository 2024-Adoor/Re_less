using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{   
    // Fruit 카운트 & 타 캐릭터 상호작용 관리 스크립트입니다. 
    public int fruitCount; 

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

            // 충돌한 오브젝트 삭제
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");
        }
    }
}
