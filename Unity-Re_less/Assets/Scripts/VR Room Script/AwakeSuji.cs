using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeSuji : MonoBehaviour
{   
    public Transform TargetPosition;
    public GameObject ChangePrefab;
    public GameObject Fruit;
    public GameObject KeyboardOBJ;
    private Keyboard KeyboardScript; // Keyboard ��ũ��Ʈ ����

    public bool isChange = false;

    private bool FruitDetect = false;
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    void Start()
    {
        // Start �޼��忡�� �ʱ�ȭ
        KeyboardScript = KeyboardOBJ.GetComponent<Keyboard>();
    }

    void Update()
    {
        if(FruitDetect && KeyboardScript.enterDown)
        {
            Destroy(gameObject);
            Destroy(Fruit);

            // ���ο� ������ ����
            GameObject newObject = Instantiate(ChangePrefab, gameObject.transform.position, gameObject.transform.rotation);
            isChange = true;

            // Ÿ�� ��ġ�� �̵���ŵ�ϴ�.
            newObject.transform.position = TargetPosition.transform.position + offset; // TargetPosition�� Ÿ�� ��ġ�� ����Ű�� ����
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
