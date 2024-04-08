using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{   
    // Fruit ī��Ʈ & Ÿ ĳ���� ��ȣ�ۿ� ���� ��ũ��Ʈ�Դϴ�. 
    public int fruitCount; 
    public bool isTrigger = false;
    public bool isCharacter = false;
    

    // Ending ���
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;
    
    public float upwardSpeed = 1f;
    public float upwardPosition = 20f;

    public bool canEnd = false;
    
    // Delay ����
    private float elapsedTime = 0f;
    private float delayTime = 5f;
    private bool isDelayedActionStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(canEnd)
        {
            if (!isDelayedActionStarted)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    // �����̰� ����Ǹ� ������ �ڵ�
                    Debug.Log("Delay Finish");

                    // Rigidbody�� Use Gravity�� false�� ����
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                    }

                    // ����� y �� ��ġ
                    float targetY = upwardPosition;
                    
                    // ���� ��ġ���� ��ǥ y �� ��ġ���� ������ �ӵ��� �̵�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), upwardSpeed * Time.deltaTime);

                    isDelayedActionStarted = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(other.CompareTag("Fruit"))
        {
            fruitCount++;

            // �浹�� ������Ʈ ����
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");
        }
        else if(other.CompareTag("HandTrigger"))
        {
            isTrigger = true;
        }
        else if(other.CompareTag("EndTrigger") && _SujiEndingTest.RotateFin)
        {
            canEnd = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && fruitCount > 0)
        {
            isCharacter = true;
        }
    }
}
