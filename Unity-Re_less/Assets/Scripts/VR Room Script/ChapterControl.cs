using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterControl : MonoBehaviour
{   
    // 챕터별 진행여부
    public bool Ch01;
    public bool Ch02;
    public bool Ch03;

    // 챕터별 스폰포인트 
    public Transform SpawnPoint01;
    public Transform SpawnPoint02;
    public Transform SpawnPoint03;
    public Vector3 offset; 

    public GameObject CH02_OBJ_SpawnOBJ1;
    public GameObject CH02_OBJ_SpawnOBJ2;

    void Start()
    {
        if (Ch01)
        {
            SpawnPlayer(SpawnPoint01);
        }
        else if (Ch02)
        {
            SpawnPlayer(SpawnPoint02);

            // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
            SpawnCH02obj spawnCH02Obj1 = CH02_OBJ_SpawnOBJ1.GetComponent<SpawnCH02obj>();
            SpawnCH02obj spawnCH02Obj2 = CH02_OBJ_SpawnOBJ2.GetComponent<SpawnCH02obj>();
            spawnCH02Obj1.isSpawn = true;
            spawnCH02Obj2.isSpawn = true;
            
        }
        else if (Ch03)
        {
            SpawnPlayer(SpawnPoint03);
        }
    }

    void Update()
    {
        if (Ch01)
        {
            Chapter01();
        }
        else if (Ch02)
        {
            Chapter02();;
        }
        else if (Ch03)
        {
            Chapter03();
        }
    }

    void SpawnPlayer(Transform Point)
    {
        transform.position = Point.position + offset;
    }


    void Chapter01()
    {
        // Fruit 충돌시 카운트 ++ 
        // Fruit 카운트 1 일때, 시계토끼 접근시 시계토끼 깨우기 -> 시계토끼 스크립트에 애니메이션 추가 
        // 시계토끼와 대화 후 패스스루 씬으로 나가기 
    }


    void Chapter02()
    {   
        

        // Fruit 충돌시 카운트 ++
        // 시계 isBroken 일때 OBJSpawn isSpawn False && OBJprefab isMoving False && 플라스틱 박스 변화 (올라갈 수 있는 구조로)
        // Fruit 카운트 1 일때, 선인장 & 체셔캣 깨우기 (한명 깨우면 카운트--) -> 각 캐릭터 스크립트에 애니메이션 추가  
        // 선인장 & 체셔캣과 대화 후 패스스루 씬으로 나가기 
    }

    // CH02_OBJ와 충돌시 리스폰
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "CH02_OBJ"인 경우
        if (collision.gameObject.CompareTag("CH02_OBJ"))
        {
            // 리스폰 위치 설정
            transform.position = SpawnPoint02.position + offset;
            Debug.Log("Respawned at Chapter 02");
        }
    }

    void Chapter03()
    {
        // 모니터 전원을 켜서 스크린 속 자고 있는 앨리스 발견하기
        // 앨리스가 깨어나면 (AwakeSuji에 isWake 추가) 스크린 밖으로 나오는 앨리스 
        // 앨리스와 대화 -> 엔딩으로 연결(점점 커지면서 방 한가운데로 이동) -> 패스스루 씬으로 나가기 
    }
}