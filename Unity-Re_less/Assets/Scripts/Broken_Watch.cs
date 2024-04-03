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

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

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

                SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
                SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();

                spawnCH02Obj1.isSpawn = false;
                spawnCH02Obj2.isSpawn = false;

                // PrefabRotationPair 구조체의 prefab의 CH02obj 스크립트의 isMoving을 false로 변경 
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
