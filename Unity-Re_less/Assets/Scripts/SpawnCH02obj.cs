using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    // public GameObject[] Prefabs; // 자동차 프리팹들을 배열로 받음

    [System.Serializable]
    public struct PrefabRotationPair
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public PrefabRotationPair[] prefabRotations; // 각 Prefab과 회전값을 연결하는 배열

    public float spawnInterval = 2f; // 프리팹 생성 간격
    public float moveSpeed = 5f; // 프리팹 이동 속도
    public float direction = 1f; // 프리팹 이동 방향
    public Vector3 offset;

    private float spawnTimer = 0f; // 프리팹 생성 타이머
    public bool isSpawn = true; // 프리팹 생성 플래그

    void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 일정 간격마다 자동차 생성
        if (isSpawn && spawnTimer >= spawnInterval)
        {
            SpawnOBJ();
            spawnTimer = 0f; // 타이머 초기화
        }
    }

    // 자동차 생성 메서드
    void SpawnOBJ()
    {
        // 랜덤한 자동차 프리팹 선택
        PrefabRotationPair pair = prefabRotations[Random.Range(0, prefabRotations.Length)];

        // 선택된 Prefab과 회전값을 사용하여 자동차 생성
        Vector3 spawnPosition = transform.position + offset;
        Quaternion spawnRotationQuaternion = Quaternion.Euler(pair.rotation);
        GameObject newOBJ = Instantiate(pair.prefab, spawnPosition, spawnRotationQuaternion);

        // 생성된 자동차에 이동 속도 적용
        CH02obj objMovement = newOBJ.GetComponent<CH02obj>();
        if (objMovement != null)
        {
            objMovement.Speed = moveSpeed;
            objMovement.Direction = direction;
            objMovement.isMoving = true;
        }
    }

    // 프리팹 생성 중지 메서드
    public void StopSpawn()
    {
        isSpawn = false;
    }
}
