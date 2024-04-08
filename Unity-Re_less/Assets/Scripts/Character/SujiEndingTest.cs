using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiEndingTest : MonoBehaviour
{
    public float moveSpeed = 8.0f; // 이동 속도
    public GameObject SujiChat; 
    public GameObject EndingCharacters;

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

    public bool RotateFin = false;              // Suji 이동 종료 여부 
    private bool isCharacters = false;

    void Start()
    {
        Aposition = A.position;
        Bposition = B.position;
        Cposition = C.position;
    }

    // Update 함수는 매 프레임마다 호출됩니다.
    void Update()
    {   
        // Chat_Suji _Chat_Suji = SujiChat.GetComponent<Chat_Suji>();

        if(canMove)
        {
            // A point까지 이동 
            if(!ApointRotate)
            {
                transform.Rotate(0, -90, 0);
                ApointRotate = true;
                ApointMove = true;
            }
            else if(ApointMove)
            {
                MoveToTarget(A);
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

            // B point까지 이동 
            if(AFin)
            {
                if(!BpointRotate)
                {
                    transform.Rotate(0, 90, 0);
                    BpointRotate = true;
                    BpointMove = true;
                }
                else if(BpointMove)
                {
                    MoveToTarget(B);
                }
            }
            if (Mathf.Approximately(transform.position.x, Bposition.x) && Mathf.Approximately(transform.position.z, Bposition.z))
            {
                BFin = true;
                BpointMove = false;
            }

            // C point까지 이동 
            if(BFin)
            {
                if(!CpointRotate)
                {
                    transform.Rotate(0, 90, 0);
                    CpointRotate = true;
                    CpointMove = true;
                }
                else if(CpointMove)
                {
                    MoveToTarget(C);
                }
            }
            if (Mathf.Approximately(transform.position.x, Cposition.x) && Mathf.Approximately(transform.position.z, Cposition.z))
            {
                CFin = true;
                CpointMove = false;
            }

            // 마지막 Turn
            if(CFin && !RotateFin)
            {
                transform.Rotate(0, 90, 0);
                RotateFin = true;
            }
        }

    }
    
    // 타겟위치까지 이동 
    void MoveToTarget(Transform target)
    {   
        // 목표 지점까지의 거리 계산
        float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(target.position.x, 0f, target.position.z));

        // 이동하는데 필요한 시간 계산
        float timeToReachTarget = distanceToTarget / moveSpeed;

        // 현재 위치의 y 값을 유지하면서 목표 지점으로 일정한 속도로 이동
        Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    // 프리팹 생성 
    void CreatePrefabInstance(GameObject prefabToCreate)
    {
        // 프리팹의 위치 정보를 가져옵니다.
        Vector3 prefabPosition = prefabToCreate.transform.position;

        // 프리팹을 해당 위치에 생성합니다.
        Instantiate(prefabToCreate, prefabPosition, Quaternion.identity);
    }

}
