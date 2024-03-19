using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{   
    public GameObject[] cars; // 정지시킬 자동차 오브젝트들의 배열

    public SpawnCars[] spawnCarsScripts; // SpawnCars 스크립트 배열

    public GameObject objectToChangeMaterial_1; // 메테리얼을 변경할 오브젝트
    public GameObject objectToChangeMaterial_2;

    public Material newMaterial_1; // 적용할 새로운 메테리얼
    public Material newMaterial_2;

    public float buttonPressDepth = 0.5f; // 버튼이 아래로 내려갈 거리
    public float movementSpeed = 1.0f; // 내려가는 속도

    private bool isPressed = false; // 버튼이 눌렸는지 여부
    private Vector3 initialPosition; // 초기 위치

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;

            // 버튼이 목표 위치에 도달하면 isPressed를 false로 설정하여 더 이상 내려가지 않도록 함
            if (transform.position.y <= initialPosition.y - buttonPressDepth)
            {
                isPressed = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 버튼과 "Player" 오브젝트가 충돌했을 때만 버튼이 작동
        if(collision.gameObject.tag == "Player")
        {
            // 버튼을 누르고, 메테리얼을 변경
            isPressed = true;
            objectToChangeMaterial_1.GetComponent<Renderer>().material = newMaterial_1;
            objectToChangeMaterial_2.GetComponent<Renderer>().material = newMaterial_2;

            // 모든 자동차 오브젝트의 이동을 정지
            foreach(GameObject car in cars)
            {
                Cars carScript = car.GetComponent<Cars>();
                if(carScript != null)
                {
                    carScript.isMoving = false;
                }
            }

            // 모든 SpawnCars 스크립트에 대해 프리팹 생성 중지 메서드 호출
            foreach(SpawnCars spawnCarsScript in spawnCarsScripts)
            {
                spawnCarsScript.StopSpawn();
            }
        }
    }
}
