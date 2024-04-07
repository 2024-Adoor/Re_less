using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeCharacters : MonoBehaviour
{
    public GameObject ChangePrefab;          // ��ȯ�� �ִϸ��̼��� ������ �ִ� ������Ʈ
    public GameObject Player;
    private PlayerState PlayerStateScript;  // PlayerState ��ũ��Ʈ ����
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

            // ���ο� ������ ����
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
