using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // 충돌 횟수를 저장할 변수
    public GameObject newPrefab1;  // 변경할 프리팹1
    public GameObject newPrefab2;  // 변경할 프리팹2
    public Vector3 RotationOffset;              // 변경할 프리팹 위치 
    public bool isBroken = false;

    public Ch02ObjectSpawner[] ch02ObjectSpawners;

    GameObject newObject_1;

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionCount++; // 충돌 횟수 증가
            Debug.Log("충돌 횟수: " + collisionCount);

            // 충돌 횟수 1이면 프리팹 변경 1
            if (collisionCount == 1)
            {
                //gameObject.SetActive(false);
                newObject_1 = Instantiate(newPrefab1, newPrefab1.transform.position, newPrefab1.transform.rotation);
            }
            // 충돌 횟수 2이면 프리팹 변경 2
            if (collisionCount == 2)
            {
                Destroy(gameObject);
                Destroy(newObject_1);
                GameObject newObject_2 = Instantiate(newPrefab2, newPrefab2.transform.position, newPrefab2.transform.rotation);

                isBroken = true;

                foreach (var spawner in ch02ObjectSpawners) { spawner.StopSpawn(); }
            }
        }
    }
}
