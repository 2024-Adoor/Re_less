using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCharacterChange : MonoBehaviour
{
    public GameObject Player;
    public GameObject NewPrefab;
    PlayerState _PlayerState;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // _PlayerState isTeleport true -> Destroy & New Prefab 
    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();

        if(_PlayerState.isTeleport)
        {
            Destroy(gameObject);
            CreatePrefabInstance(NewPrefab);
        }
    }

    // 프리팹 생성 
    void CreatePrefabInstance(GameObject prefabToCreate)
    {
        // 프리팹의 위치 정보를 가져옵니다.
        Vector3 prefabPosition = prefabToCreate.transform.position;

        // 프리팹을 해당 위치에 생성합니다.
        Instantiate(prefabToCreate, prefabPosition, Quaternion.identity);
    }
}
