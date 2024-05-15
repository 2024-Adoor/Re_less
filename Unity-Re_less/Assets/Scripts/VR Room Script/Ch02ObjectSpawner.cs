using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ch02ObjectSpawner : MonoBehaviour
{
    /// <summary>
    /// 스폰할 오브젝트 프리팹들
    /// </summary>
    [SerializeField]
    private CH02obj[] ch02ObjectPrefabs;

    /// <summary>
    /// 오브젝트 풀
    /// </summary>
    private readonly List<CH02obj> _ch02ObjectPool = new();

    /// <summary>
    /// 오브젝트를 담을 부모 오브젝트
    /// </summary>
    public Transform container;
    
    /// <summary>
    /// 오브젝트의 이동이 끝나는 트리거
    /// </summary>
    [SerializeField]
    private Collider endTrigger;
    
    public float spawnInterval = 2f; // 프리팹 생성 간격
    public float moveSpeed = 5f; // 프리팹 이동 속도
    public float direction = 1f; // 프리팹 이동 방향
    public Vector3 offset;

    private float spawnTimer = 0f; // 프리팹 생성 타이머

    // 프리팹 생성 종료후 활성화할 오브젝트
    public GameObject Ch02_Cars;

    private void OnEnable()
    {
        for (int i = 0 ; i < ch02ObjectPrefabs.Length * 3 ; i++)
        {
            var ch02Obj = Instantiate(ch02ObjectPrefabs[i % ch02ObjectPrefabs.Length], parent: container);
            ch02Obj.Speed = moveSpeed;
            ch02Obj.Direction = direction;
            ch02Obj.endTrigger = endTrigger;
            ch02Obj.EndTriggerEntered += obj =>
            {
                // 엔드 트리거에 충돌 시 비활성화
                obj.gameObject.SetActive(false);
                _ch02ObjectPool.Add(obj);
            };
            ch02Obj.gameObject.SetActive(false);
            _ch02ObjectPool.Add(ch02Obj);
        }
        
        if (Ch02_Cars != null)
        {
            Ch02_Cars.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        StopSpawn();
    }
    
    private void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 일정 간격마다 프리팹 생성
        if (spawnTimer >= spawnInterval)
        {
            SpawnObj();
            spawnTimer = 0f; // 타이머 초기화
        }
    }

    private void SpawnObj()
    {
        // 풀에서 랜덤 오브젝트 가져오기
        var ch02Obj = _ch02ObjectPool.ElementAt(Random.Range(0, _ch02ObjectPool.Count));
        ch02Obj.gameObject.SetActive(true);
        _ch02ObjectPool.Remove(ch02Obj);
        
        // 오브젝트 위치를 스포너(이 오브젝트)의 위치로 재설정
        ch02Obj.transform.position = this.transform.position + offset;
    }

    // 프리팹 생성 중지 메서드
    public void StopSpawn()
    {
        Destroy(container.gameObject);

        if(Ch02_Cars != null)
        {
            Ch02_Cars.SetActive(true);
        }
    }
}
