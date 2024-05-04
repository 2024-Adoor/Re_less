using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiEndingTest : MonoBehaviour
{
    public float moveSpeed = 8.0f; // �̵� �ӵ�
    public GameObject SujiChat; 
    public GameObject EndingCharacters;

    // �̵��ϱ� ���� ���� 
    public Transform A;
    public Transform B;
    public Transform C;

    public bool canMove = false;

    private Vector3 Aposition;
    private Vector3 Bposition;
    private Vector3 Cposition;

    private bool ApointRotate = false;
    private bool ApointMove = false;
    private bool AFin = false;

    private bool BpointRotate = false;
    private bool BpointMove = false;
    private bool BFin = false;

    private bool CpointRotate = false;
    private bool CpointMove = false;
    private bool CFin = false;

    public bool RotateFin = false;              // Suji �̵� ���� ���� 
    private bool isCharacters = false;

    // �ִϸ��̼�
    public Animation animationComponent;    // Animation ������Ʈ ����
    public AnimationClip RunAni;       // SleepOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 
    bool isRun = false;
    bool isIDle = false;

    // ���� Ʈ���� ��� 
    public GameObject EndTrigger;
    Renderer EndTriggerRender;

    void Start()
    {
        Aposition = A.position;
        Bposition = B.position;
        Cposition = C.position;

        EndTriggerRender = EndTrigger.GetComponent<Renderer>();
        EndTriggerRender.enabled = false;
    }

    // Update �Լ��� �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {   
        // Chat_Suji _Chat_Suji = SujiChat.GetComponent<Chat_Suji>();
        SujiManage _SujiManage = GetComponent<SujiManage>();

        if(canMove)
        {
            _SujiManage.isSleepOut = false;
            Debug.Log("Suji isSleepOut is false - SujiEndingTest");

            ChangeAnimation();
            animationComponent.Play();

            // A point���� �̵� 
            if(!ApointRotate)
            {
                transform.Rotate(0, -90, 0);
                ApointRotate = true;
                ApointMove = true;
                animationComponent.Play();
            }
            else if(ApointMove)
            {
                MoveToTarget(A);
                ChangeAnimation();
                animationComponent.Play();
            }
            if (Mathf.Approximately(transform.position.x, Aposition.x) && Mathf.Approximately(transform.position.z, Aposition.z))
            {
                AFin = true;
                ApointMove = false;
                if(!isCharacters)
                {
                    CreatePrefabInstance(EndingCharacters);
                    isCharacters = true;
                }
            }

            // B point���� �̵� 
            if(AFin)
            {
                if(!BpointRotate)
                {
                    transform.Rotate(0, 90, 0);
                    BpointRotate = true;
                    BpointMove = true;
                    ChangeAnimation();
                    animationComponent.Play();
                }
                else if(BpointMove)
                {
                    MoveToTarget(B);
                    ChangeAnimation();
                    animationComponent.Play();
                }
            }
            if (Mathf.Approximately(transform.position.x, Bposition.x) && Mathf.Approximately(transform.position.z, Bposition.z))
            {
                BFin = true;
                BpointMove = false;
            }

            // C point���� �̵� 
            if(BFin)
            {
                if(!CpointRotate)
                {
                    transform.Rotate(0, 90, 0);
                    CpointRotate = true;
                    CpointMove = true;
                    ChangeAnimation();
                    animationComponent.Play();
                }
                else if(CpointMove)
                {
                    MoveToTarget(C);
                    ChangeAnimation();
                    animationComponent.Play();
                }
            }
            if (Mathf.Approximately(transform.position.x, Cposition.x) && Mathf.Approximately(transform.position.z, Cposition.z))
            {
                CFin = true;
                CpointMove = false;
            }
        }

        // ������ Turn
        if(CFin && !RotateFin)
        {
            canMove = false;
            transform.Rotate(0, -90, 0);
            RotateFin = true;

            // End Trigger Point Render enable
            EndTriggerRender.enabled = true;
        }

        

        if(RotateFin && !isIDle)
        {
            animationComponent.Stop();
            animationComponent.clip = IdleAni;
            isIDle = true;
        }

        if(isIDle)
        {
            animationComponent.Play();
        }

    }
    
    // Ÿ����ġ���� �̵� 
    void MoveToTarget(Transform target)
    {   
        // ��ǥ ���������� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(target.position.x, 0f, target.position.z));

        // �̵��ϴµ� �ʿ��� �ð� ���
        float timeToReachTarget = distanceToTarget / moveSpeed;

        // ���� ��ġ�� y ���� �����ϸ鼭 ��ǥ �������� ������ �ӵ��� �̵�
        Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    // ������ ���� 
    void CreatePrefabInstance(GameObject prefabToCreate)
    {
        // �������� ��ġ�� ȸ�� ������ �����ɴϴ�.
        Vector3 prefabPosition = prefabToCreate.transform.position;
        Quaternion prefabRotation = prefabToCreate.transform.rotation;

        // �������� �ش� ��ġ�� ȸ������ �����մϴ�.
        Instantiate(prefabToCreate, prefabPosition, prefabRotation);
    }

    void ChangeAnimation()
    {
        if (animationComponent != null && RunAni != null && !isRun)
        {
            animationComponent.Stop();
            animationComponent.clip = RunAni;
            isRun = true;
            Debug.Log("Suji is Running");
        }
    }
}
