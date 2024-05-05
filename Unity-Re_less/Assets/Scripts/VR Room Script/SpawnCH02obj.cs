using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCH02obj : MonoBehaviour
{
    [System.Serializable]
    public struct PrefabRotationPair
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public PrefabRotationPair[] prefabRotations; // 각 프리팹과 회전값을 연결하는 배열

    public float spawnInterval = 2f; // 프리팹 생성 간격
    public float moveSpeed = 5f; // 프리팹 이동 속도
    public float direction = 1f; // 프리팹 이동 방향
    public Vector3 offset;

    private float spawnTimer = 0f; // 프리팹 생성 타이머
    public bool isSpawn = true; // 프리팹 생성 플래그

    // 프리팹 생성 종료후 활성화할 오브젝트
    public GameObject Ch02_Cars;

    void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 일정 간격마다 프리팹 생성
        if (isSpawn && spawnTimer >= spawnInterval)
        {
            SpawnOBJ();
            spawnTimer = 0f; // 타이머 초기화
        }
        else
        {
            
        }
    }

    // 프리팹 생성 메서드
    void SpawnOBJ()
    {
        if(Ch02_Cars != null)
        {
            Ch02_Cars.SetActive(false);
        }

        // 랜덤한 프리팹 선택
        PrefabRotationPair pair = prefabRotations[Random.Range(0, prefabRotations.Length)];

        // 선택된 Prefab과 회전값을 사용하여 프리팹 생성
        Vector3 spawnPosition = transform.position + offset;
        Quaternion spawnRotationQuaternion = Quaternion.Euler(pair.rotation);
        GameObject newOBJ = Instantiate(pair.prefab, spawnPosition, spawnRotationQuaternion);

        // 생성된 프리팹에 이동 속도 적용
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

        if(Ch02_Cars != null)
        {
            Ch02_Cars.SetActive(true);
        }
    }
}
