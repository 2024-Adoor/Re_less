using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeSuji : MonoBehaviour
{
    public Texture newTexture; // ������ �ؽ�ó
    public GameObject KeyboardOBJ;
    private Keyboard KeyboardScript; // Keyboard ��ũ��Ʈ ����

    private bool FruitDetect = false;

    void Start()
    {
        // Start �޼��忡�� �ʱ�ȭ
        KeyboardScript = KeyboardOBJ.GetComponent<Keyboard>();
    }

    void Update()
    {
        if(FruitDetect && KeyboardScript.enterDown)
        {
            // ������ �ؽ�ó�� ������ �ؽ�ó�� ����
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && newTexture != null)
                {
                    renderer.material.mainTexture = newTexture;
                }
        }
    }

    // �浹 ����
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±װ� Fruit���� Ȯ��
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
