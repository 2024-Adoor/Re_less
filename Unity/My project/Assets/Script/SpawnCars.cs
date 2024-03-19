using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public GameObject[] carPrefabs; // 자동차 프리팹들을 배열로 받음
    public float spawnInterval = 2f; // 자동차 생성 간격
    public float moveSpeed = 5f; // 자동차 이동 속도
    public Vector3 spawnRotation; // 자동차 생성 시 회전값
    
    private float spawnTimer = 0f; // 자동차 생성 타이머
    private bool canSpawn = true; // 프리팹 생성 플래그

    void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 일정 간격마다 자동차 생성
        if (canSpawn && spawnTimer >= spawnInterval)
        {
            SpawnCar();
            spawnTimer = 0f; // 타이머 초기화
        }
    }

    // 자동차 생성 메서드
    void SpawnCar()
    {
        // 랜덤한 자동차 프리팹 선택
        GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // SpawnPoint의 위치를 사용하여 자동차 생성
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotationQuaternion = Quaternion.Euler(spawnRotation);
        GameObject newCar = Instantiate(carPrefab, spawnPosition, spawnRotationQuaternion);

        // 생성된 자동차에 이동 속도 적용
        Cars carMovement = newCar.GetComponent<Cars>();
        if (carMovement != null)
        {
            carMovement.CarSpeed = moveSpeed;
            carMovement.isMoving = true;
        }
    }

    // 프리팹 생성 중지 메서드
    public void StopSpawn()
    {
        canSpawn = false;
    }
}
