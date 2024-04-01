using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    // public GameObject[] Prefabs; // �ڵ��� �����յ��� �迭�� ����

    [System.Serializable]
    public struct PrefabRotationPair
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public PrefabRotationPair[] prefabRotations; // �� Prefab�� ȸ������ �����ϴ� �迭

    public float spawnInterval = 2f; // ������ ���� ����
    public float moveSpeed = 5f; // ������ �̵� �ӵ�
    public float direction = 1f; // ������ �̵� ����
    public Vector3 offset;

    private float spawnTimer = 0f; // ������ ���� Ÿ�̸�
    public bool isSpawn = true; // ������ ���� �÷���

    void Update()
    {
        // Ÿ�̸� ������Ʈ
        spawnTimer += Time.deltaTime;

        // ���� ���ݸ��� �ڵ��� ����
        if (isSpawn && spawnTimer >= spawnInterval)
        {
            SpawnOBJ();
            spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }

    // �ڵ��� ���� �޼���
    void SpawnOBJ()
    {
        // ������ �ڵ��� ������ ����
        PrefabRotationPair pair = prefabRotations[Random.Range(0, prefabRotations.Length)];

        // ���õ� Prefab�� ȸ������ ����Ͽ� �ڵ��� ����
        Vector3 spawnPosition = transform.position + offset;
        Quaternion spawnRotationQuaternion = Quaternion.Euler(pair.rotation);
        GameObject newOBJ = Instantiate(pair.prefab, spawnPosition, spawnRotationQuaternion);

        // ������ �ڵ����� �̵� �ӵ� ����
        CH02obj objMovement = newOBJ.GetComponent<CH02obj>();
        if (objMovement != null)
        {
            objMovement.Speed = moveSpeed;
            objMovement.Direction = direction;
            objMovement.isMoving = true;
        }
    }

    // ������ ���� ���� �޼���
    public void StopSpawn()
    {
        isSpawn = false;
    }
}
