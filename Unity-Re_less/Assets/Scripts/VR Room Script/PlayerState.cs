using System.Collections;
using System.Linq;
using UnityEngine;
using Reless;
using Reless.VR;

public class PlayerState : MonoBehaviour
{   
    // Fruit 카운트 & 타 캐릭터 상호작용 관리 스크립트입니다. 
    public int FruitCount
    {
        get => _fruitCount;
        set
        {
            _fruitCount = value;
            if (value == 2) { foreach (GameObject arrow in Ch02Arrows) { arrow.SetActive(true); } }
        }
    }
    private int _fruitCount = -1; 
    
    public bool isTrigger = false;
    public bool isCharacter = false;
    
    // Ending 요건
    public GameObject Suji;
    SujiEndingTest _SujiEndingTest;

    public bool isTeleport = false;

    // 엔딩 포인트 trigger -> Destroy Door 
    public GameObject door;

    // UI 트리거 관리
    public bool isJumpUI = false;
    public bool isFriendUI = false;
    public bool isDoorUI = false;
    public bool isCh02JumpUI = false;
    public bool isDisawakeUI_Trigger = false;
    public bool isCh02UI = false;
    public bool isCh02MonitorUI = false;

    // Fade 트리거 관리
    public bool isFadeOut = false; 
    public bool isFadeIn = false;

    // 효과음 관리
    public AudioClip fruit_get;
    private AudioSource audioSource;

    // EndTrigger 머테리얼
    public Material EndMaterial;

    // 챕터 2 열매 다 먹었을 때 화살표 활성화
    public GameObject[] Ch02Arrows;

    private EndingBehaviour _endingBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 배열을 반복하여 작업 수행
        foreach (GameObject obj in Ch02Arrows)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        
        if(Suji != null)
        {
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        _endingBehaviour = FindAnyObjectByType<EndingBehaviour>();
    }

    void FixedUpdate()
    {
        if(_endingBehaviour._isEndUIFin)
        {
            _endingBehaviour.StartEnding();
        }
    }

    public GameObject[] fruits;

    // NOTE: 업데이트 함수에 있는 엔딩 진행 EndingBehaviour.StartEnding()으로 이동

    void OnTriggerEnter(Collider other)
    {
        if(Suji != null)
           _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();

        if(fruits.ToList().Contains(other.gameObject))
        {
            if(FruitCount == -1)
            {
                FruitCount = 1;
            }
            else
            {
                FruitCount++;
            }
            
            // 충돌한 오브젝트 삭제
            Destroy(other.gameObject);
            Debug.Log("Fruit detected");

            audioSource.PlayOneShot(fruit_get);
        }
        else if(other.CompareTag("EndTrigger") && _SujiEndingTest.IsReachedEndPoint)
        {
            Renderer otherRend = other.GetComponent<Renderer>();

            otherRend.material = EndMaterial;

            _endingBehaviour.StartEndChat();
        }

        if(other.gameObject.name == "UI_JumpTutorial_Trigger")
        {
            isJumpUI = true;
        }
        else if(other.gameObject.name == "UI_FriendTutorial_Trigger")
        {
            isFriendUI = true;
        }
        else if(other.gameObject.name == "UI_Chapter01Fin_Trigger")
        {
            isCh02UI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Door_Trigger")
        {
            isDoorUI = true;
        }
        else if(other.gameObject.name == "UI_Ch02Jump_Trigger")
        {
            isCh02JumpUI = true;
        }
        else if(other.gameObject.name == "UI_MonitorTrigger")
        {
            isCh02MonitorUI = true;
        }
    }

    // 콜라이더랑 충돌할때 
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Character") && FruitCount > 0)
        {
            isCharacter = true;
        }
    }
}
