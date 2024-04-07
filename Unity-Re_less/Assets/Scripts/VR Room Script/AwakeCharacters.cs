using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCharacters : MonoBehaviour
{
    public GameObject ChangePrefab;          // 전환될 애니메이션을 가지고 있는 오브젝트
    public GameObject Player;
    private PlayerState PlayerStateScript;  // PlayerState 스크립트 변수
    private bool isDetect = false;
    public bool isChange = false;

    void Update()
    {
        PlayerStateScript = Player.GetComponent<PlayerState>();

        ChagneAni(PlayerStateScript.fruitCount);
    }

    private void ChagneAni(int isFruit)
    { 
        if(isFruit > 0 && isDetect)
        {
            Destroy(gameObject);

            // 새로운 프리팹 생성
            GameObject newObject = Instantiate(ChangePrefab, gameObject.transform.position, gameObject.transform.rotation);
            isChange = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDetect = true;
        }
    }
}
