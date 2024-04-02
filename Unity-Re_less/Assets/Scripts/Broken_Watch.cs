using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // 충돌 횟수를 저장할 변수
    public GameObject destroyPrefab;    // 삭제할 프리팹 
    public GameObject newPrefab;  // 변경할 프리팹
    public Vector3 RotationOffset;              // 변경할 프리팹 위치 
    public bool isBroken;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionCount++; // 충돌 횟수 증가
            Debug.Log("충돌 횟수: " + collisionCount);

            // 충돌 횟수가 3 이상이면 프리팹 변경
            if (collisionCount >= 3)
            {
                ChangeParentPrefab();
                isBroken = true;
            }
        }
    }

    // 프리팹 변경 함수
    private void ChangeParentPrefab()
    {
        // 이전 오브젝트 삭제
        Destroy(destroyPrefab);
        Destroy(gameObject);

        // 새로운 프리팹 생성
        GameObject newObject = Instantiate(newPrefab, newPrefab.transform.position, newPrefab.transform.rotation);
    }
}
