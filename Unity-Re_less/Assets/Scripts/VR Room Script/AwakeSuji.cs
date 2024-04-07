using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeSuji : MonoBehaviour
{   
    public Transform TargetPosition;
    public GameObject ChangePrefab;
    public GameObject Fruit;
    public GameObject KeyboardOBJ;
    private Keyboard KeyboardScript; // Keyboard 스크립트 변수

    public bool isChange = false;

    private bool FruitDetect = false;
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    void Start()
    {
        // Start 메서드에서 초기화
        KeyboardScript = KeyboardOBJ.GetComponent<Keyboard>();
    }

    void Update()
    {
        if(FruitDetect && KeyboardScript.enterDown)
        {
            Destroy(gameObject);
            Destroy(Fruit);

            // 새로운 프리팹 생성
            GameObject newObject = Instantiate(ChangePrefab, gameObject.transform.position, gameObject.transform.rotation);
            isChange = true;

            // 타겟 위치로 이동시킵니다.
            newObject.transform.position = TargetPosition.transform.position + offset; // TargetPosition은 타겟 위치를 가리키는 변수
        }
    }

    // 충돌 감지
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 Fruit인지 확인
        if (other.CompareTag("Fruit"))
        {   
            Debug.Log("Fruit detected");
            FruitDetect = true;
        }
        else
        {
            Debug.Log("Collision with non-Fruit object");
        }
    }
}
