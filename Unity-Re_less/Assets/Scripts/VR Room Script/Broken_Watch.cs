using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // 충돌 횟수를 저장할 변수
    public GameObject newPrefab1;  // 변경할 프리팹1
    public GameObject newPrefab2;  // 변경할 프리팹2
    public Vector3 RotationOffset;              // 변경할 프리팹 위치 
    public bool isBroken;

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

    public GameObject CH02_Cars;

    GameObject newObject_1;

    void Start()
    {
        CH02_Cars.SetActive(false);
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

                SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
                SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();

                spawnCH02Obj1.isSpawn = false;
                spawnCH02Obj2.isSpawn = false;

                CH02_Cars.SetActive(true);

                // A 오브젝트의 모든 자식 오브젝트에 접근하기
                for (int i = 0; i < CH02_Cars.transform.childCount; i++)
                {
                    // i번째 자식 오브젝트 가져오기
                    GameObject childObject = CH02_Cars.transform.GetChild(i).gameObject;

                    // 자식 오브젝트에 접근하여 스크립트 실행 또는 수정하기
                    if (childObject != null)
                    {
                        // 예를 들어, 자식 오브젝트의 스크립트를 수정하거나 실행하기
                        CH02obj _CH02obj = childObject.GetComponent<CH02obj>();


                        if (_CH02obj != null)
                        {
                            // ChildScript의 public 메서드나 변수에 접근하여 원하는 작업 수행
                            _CH02obj.isMoving = false;
                            _CH02obj.Speed = 0f;
                        }
                    }
                }
            }
        }
    }
}
