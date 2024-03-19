using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public GameObject[] carPrefabs; // �ڵ��� �����յ��� �迭�� ����
    public float spawnInterval = 2f; // �ڵ��� ���� ����
    public float moveSpeed = 5f; // �ڵ��� �̵� �ӵ�
    public Vector3 spawnRotation; // �ڵ��� ���� �� ȸ����
    
    private float spawnTimer = 0f; // �ڵ��� ���� Ÿ�̸�
    private bool canSpawn = true; // ������ ���� �÷���

    void Update()
    {
        // Ÿ�̸� ������Ʈ
        spawnTimer += Time.deltaTime;

        // ���� ���ݸ��� �ڵ��� ����
        if (canSpawn && spawnTimer >= spawnInterval)
        {
            SpawnCar();
            spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }

    // �ڵ��� ���� �޼���
    void SpawnCar()
    {
        // ������ �ڵ��� ������ ����
        GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // SpawnPoint�� ��ġ�� ����Ͽ� �ڵ��� ����
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotationQuaternion = Quaternion.Euler(spawnRotation);
        GameObject newCar = Instantiate(carPrefab, spawnPosition, spawnRotationQuaternion);

        // ������ �ڵ����� �̵� �ӵ� ����
        Cars carMovement = newCar.GetComponent<Cars>();
        if (carMovement != null)
        {
            carMovement.CarSpeed = moveSpeed;
            carMovement.isMoving = true;
        }
    }

    // ������ ���� ���� �޼���
    public void StopSpawn()
    {
        canSpawn = false;
    }
}
