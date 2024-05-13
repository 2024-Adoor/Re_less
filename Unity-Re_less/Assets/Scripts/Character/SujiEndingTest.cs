using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Reless.Debug.Logger;

public class SujiEndingTest : MonoBehaviour
{
    public float moveSpeed = 8.0f; // 이동 속도
    public GameObject SujiChat; 
    
    [SerializeField]
    public GameObject endingCharacters;

    [Header("Suji Move Points")]
    [SerializeField]
    private Transform aPoint;
    
    [SerializeField]
    private Transform bPoint;

    [SerializeField]
    private Transform cPoint;

    /// <summary>
    /// 수지 이동이 끝났는지 여부
    /// </summary>
    public bool IsReachedEndPoint { get; private set; }           

    // 애니메이션
    [Header("Animation")]
    [SerializeField]
    private AnimationClip runAnimationClip;  
    
    [SerializeField]
    private AnimationClip idleAnimationClip;
    
    [SerializeField, ReadOnly]
    private Animation animationComponent;    // Animation 컴포넌트 참조
    
    [Header("")]
    // 엔딩 트리거 장소 
    public GameObject EndTrigger;
    
    private void Awake()
    {
        endingCharacters.SetActive(false);
    }

    void Start()
    {
        
    }
    
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void WakeUp()
    {
        Logger.Log($"{nameof(SujiEndingTest)}: WakeUp");
        
        SujiChat.SetActive(false);
        
        // 애니메이션 실행
        animationComponent.Stop();
        animationComponent.clip = runAnimationClip;
        animationComponent.Play();
        
        StartCoroutine(WakeRoutine());
        
        IEnumerator WakeRoutine()
        {
            // A point까지 이동
            transform.Rotate(0, -90, 0);
            yield return MovingTo(aPoint);
            
            // 엔딩 캐릭터들 활성화
            endingCharacters.SetActive(true);
            
            // B point까지 이동
            transform.Rotate(0, 90, 0);
            yield return MovingTo(bPoint);
            
            // C point까지 이동
            transform.Rotate(0, 90, 0);
            yield return MovingTo(cPoint);
            
            // 마지막 Turn
            transform.Rotate(0, -90, 0);
            
            animationComponent.Stop();
            animationComponent.clip = idleAnimationClip;
            animationComponent.Play();
            
            
            IsReachedEndPoint = true;
            
            // SujiChat.SetActive(true);
        }
    }
    
    private IEnumerator MovingTo(Transform target)
    {
        Logger.Log($"{nameof(SujiEndingTest)}: Move toward {target.name}...");
        
        for (var reached = false; !reached; reached = Vector3.Distance(transform.position, target.position) < Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        Logger.Log($"{nameof(SujiEndingTest)}: Reached target {target.name}");
    }

    private void OnValidate()
    {
        if (animationComponent.IsUnityNull()) { animationComponent = GetComponent<Animation>(); }
    }
}
