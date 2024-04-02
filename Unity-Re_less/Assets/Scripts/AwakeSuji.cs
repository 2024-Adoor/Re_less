using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeSuji : MonoBehaviour
{
    public Texture newTexture; // 변경할 텍스처
    public GameObject KeyboardOBJ;
    private Keyboard KeyboardScript; // Keyboard 스크립트 변수

    private bool FruitDetect = false;

    void Start()
    {
        // Start 메서드에서 초기화
        KeyboardScript = KeyboardOBJ.GetComponent<Keyboard>();
    }

    void Update()
    {
        if(FruitDetect && KeyboardScript.enterDown)
        {
            // 본인의 텍스처를 변경할 텍스처로 설정
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && newTexture != null)
                {
                    renderer.material.mainTexture = newTexture;
                }
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
