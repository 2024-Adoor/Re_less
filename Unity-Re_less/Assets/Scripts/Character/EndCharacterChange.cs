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

    // ������ ���� 
    void CreatePrefabInstance(GameObject prefabToCreate)
    {
        // �������� ��ġ ������ �����ɴϴ�.
        Vector3 prefabPosition = prefabToCreate.transform.position;

        // �������� �ش� ��ġ�� �����մϴ�.
        Instantiate(prefabToCreate, prefabPosition, Quaternion.identity);
    }
}
