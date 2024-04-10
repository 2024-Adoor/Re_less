using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reless;

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

    public bool canEnd = false;
    public bool isYUp = false;

    public GameObject DeleteCharacters;

    public GameObject Suji_Surprised;
    public GameObject Characters_Surprised;
    bool isSurprised = false;
    public bool isTeleport = false;
    
    // Ending RespawnTrigger & SpawnPoint 
    public Transform RespawnTrigger;
    public Transform EndSpawnPoint;
    public GameObject Camera;

    // Delay ����
    private float elapsedTime = 0f;
    private float delayTime = 2f;
    private bool isDelayedActionStarted = false;

    // UI Ʈ���� ����
    public bool isJumpUI = false;
    public bool isFriendUI = false;
    public bool isDoorUI = false;
    public bool isCh02JumpUI = false;

    // ȿ���� ����
    public AudioClip fruit_get;
    private AudioSource audioSource;

    // EndTrigger ���׸���
    public Material EndMaterial;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        // ��������� �����Ǹ� Delay �� isYUp true 
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
                        isYUp = true;
                    }
                    isDelayedActionStarted = true;
                }
            }
        }

        // isYUp -> �÷��̾� y�� ��� & ĳ���� �ִϸ��̼� ���� 
        if(isYUp)
        {
            // ���� ��ġ���� ������ �ӵ��� �̵�
            MoveToTargetY(RespawnTrigger);

            if(!isSurprised)
            {
                // �ֵ� ������ ���� (IDLE -> Surprised)
                Destroy(Suji);
                // Destroy(DeleteCharacters);

                // ���ο� ������ ����
                GameObject newSuji = Instantiate(Suji_Surprised, Suji_Surprised.transform.position, Suji_Surprised.transform.rotation);
                GameObject newCharacters = Instantiate(Characters_Surprised, Characters_Surprised.transform.position, Characters_Surprised.transform.rotation);

                isSurprised = true;
            }
        }

        if(Mathf.Approximately(transform.position.y, RespawnTrigger.position.y))
        {
            isYUp = false;
            canEnd = false;
            Debug.Log("Player on RespawnTrigger !!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }

            transform.position = EndSpawnPoint.position + new Vector3(0f, 23f, 0f);

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleCollider.radius = 5f;
            capsuleCollider.height = 60f;

            Camera.transform.position += new Vector3(0f, 20f, 0f);

            PlayerControl _PlayerControl = GetComponent<PlayerControl>();
            if(_PlayerControl != null)
            {
                _PlayerControl.speed = 12f;
            }

            isTeleport = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(Suji != null)
           _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(other.CompareTag("Fruit"))
        {
            fruitCount++;

            // �浹�� ������Ʈ ����
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");

            audioSource.PlayOneShot(fruit_get);
        }
        else if(other.CompareTag("EndTrigger") && _SujiEndingTest.RotateFin)
        {
            Renderer otherRend = other.GetComponent<Renderer>();

            otherRend.material = EndMaterial;

            canEnd = true;
        }

        if(other.gameObject.name == "UI_JumpTutorial_Trigger")
        {
            isJumpUI = true;
        }
        else if(other.gameObject.name == "UI_FriendTutorial_Trigger")
        {
            isFriendUI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Door_Trigger (1)" || other.gameObject.name == "UI_Ch02Door_Trigger (2)")
        {
            isDoorUI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Jump_Trigger")
        {
            isCh02JumpUI = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && fruitCount > 0)
        {
            isCharacter = true;
        }
    }

    // Ÿ����ġ���� �̵� 
    void MoveToTargetY(Transform target)
    {   
        // y�� ���������� �Ÿ� ���
        float distanceToTargetY = Mathf.Abs(target.position.y - transform.position.y);
    
        // �̵��ϴµ� �ʿ��� �ð� ���
        float timeToReachTargetY = distanceToTargetY / upwardSpeed;
    
        // ��ǥ �������� ������ �ӵ��� y�� �̵�
        float newY = Mathf.MoveTowards(transform.position.y, target.position.y, upwardSpeed * Time.deltaTime);
        
        // ���� x�� z ��ġ�� ������ ä�� y���� �����Ͽ� ���ο� ��ġ ����
        Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
        
        // ���ο� ��ġ�� �̵�
        transform.position = newPosition;
    }
}
