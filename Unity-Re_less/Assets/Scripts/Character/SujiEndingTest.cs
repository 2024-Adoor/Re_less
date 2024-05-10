using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class SujiEndingTest : MonoBehaviour
{
    public float moveSpeed = 8.0f; // 이동 속도
    public GameObject SujiChat; 
    public GameObject EndingCharacters;

    // 이동하기 위한 값들 
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

    // 애니메이션
    public Animation animationComponent;    // Animation 컴포넌트 참조
    public AnimationClip RunAni;       // SleepOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 
    bool isRun = false;
    bool isIDle = false;

    // 엔딩 트리거 장소 
    public GameObject EndTrigger;
    Renderer EndTriggerRender;
    
    private SujiManage _sujiManage;

    void Start()
    {
        Aposition = A.position;
        Bposition = B.position;
        Cposition = C.position;

        EndTriggerRender = EndTrigger.GetComponent<Renderer>();
        EndTriggerRender.enabled = false;
        
        _sujiManage = GetComponent<SujiManage>();
    }

    // NOTE: Update 함수 _ 붙여서 임시 비활성화 (WakeUp() 으로 대체 중)
    void _Update()
    {   
        // Chat_Suji _Chat_Suji = SujiChat.GetComponent<Chat_Suji>();

        if(canMove)
        {
            _sujiManage.isSleepOut = false;
            Debug.Log("Suji isSleepOut is false - SujiEndingTest");

            ChangeAnimation();
            animationComponent.Play();

            // A point까지 이동 
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
                    //CreatePrefabInstance(EndingCharacters);
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

            // C point까지 이동 
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

        // 마지막 Turn
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
    
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void WakeUp()
    {
        Reless.Logger.Log($"{nameof(SujiEndingTest)}: WakeUp");
        //
        canMove = true;
        
        ChangeAnimation();
        animationComponent.Play();
        
        StartCoroutine(WakeRoutine());
        
        IEnumerator WakeRoutine()
        {
            // A point까지 이동
            transform.Rotate(0, -90, 0);
            yield return MovingTo(A);
            
            // B point까지 이동
            transform.Rotate(0, 90, 0);
            yield return MovingTo(B);
            
            // C point까지 이동
            transform.Rotate(0, 90, 0);
            yield return MovingTo(C);
            
            // 마지막 Turn
            transform.Rotate(0, -90, 0);
            
            // End Trigger Point Render enable
            EndTriggerRender.enabled = true;
            
            animationComponent.Stop();
            animationComponent.clip = IdleAni;
            animationComponent.Play();
            
            
            RotateFin = true;
        }
    }
    
    private IEnumerator MovingTo(Transform target)
    {
        Reless.Logger.Log($"{nameof(SujiEndingTest)}: Move toward {target.name}...");
        
        for (var reached = false; !reached; reached = Vector3.Distance(transform.position, target.position) < Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        Reless.Logger.Log($"{nameof(SujiEndingTest)}: Reached target {target.name}");
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
        // 프리팹의 위치와 회전 정보를 가져옵니다.
        Vector3 prefabPosition = prefabToCreate.transform.position;
        Quaternion prefabRotation = prefabToCreate.transform.rotation;

        // 프리팹을 해당 위치와 회전으로 생성합니다.
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
