using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_Watch : MonoBehaviour
{
    private int collisionCount = 0;     // �浹 Ƚ���� ������ ����
    public GameObject newPrefab1;  // ������ ������1
    public GameObject newPrefab2;  // ������ ������2
    public Vector3 RotationOffset;              // ������ ������ ��ġ 
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
            collisionCount++; // �浹 Ƚ�� ����
            Debug.Log("�浹 Ƚ��: " + collisionCount);

            // �浹 Ƚ�� 1�̸� ������ ���� 1
            if (collisionCount == 1)
            {
                //gameObject.SetActive(false);
                newObject_1 = Instantiate(newPrefab1, newPrefab1.transform.position, newPrefab1.transform.rotation);
            }
            // �浹 Ƚ�� 2�̸� ������ ���� 2
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

                // A ������Ʈ�� ��� �ڽ� ������Ʈ�� �����ϱ�
                for (int i = 0; i < CH02_Cars.transform.childCount; i++)
                {
                    // i��° �ڽ� ������Ʈ ��������
                    GameObject childObject = CH02_Cars.transform.GetChild(i).gameObject;

                    // �ڽ� ������Ʈ�� �����Ͽ� ��ũ��Ʈ ���� �Ǵ� �����ϱ�
                    if (childObject != null)
                    {
                        // ���� ���, �ڽ� ������Ʈ�� ��ũ��Ʈ�� �����ϰų� �����ϱ�
                        CH02obj _CH02obj = childObject.GetComponent<CH02obj>();


                        if (_CH02obj != null)
                        {
                            // ChildScript�� public �޼��峪 ������ �����Ͽ� ���ϴ� �۾� ����
                            _CH02obj.isMoving = false;
                            _CH02obj.Speed = 0f;
                        }
                    }
                }
            }
        }
    }
}
