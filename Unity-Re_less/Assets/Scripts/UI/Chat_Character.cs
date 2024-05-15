using System.Collections;
using Reless;
using Reless.VR;
using UnityEngine;

public class Chat_Character : MonoBehaviour
{
    public GameObject Player;
    ChapterControl _ChapterControl;

    public GameObject Character;
    AniManage _AniManage;
    Cat_AniManage _Cat_AniManage;
    
    public Material[] chatMaterials; // Material 배열
    public bool isChatFin = false;

    public AnimationClip doorAni;
    int Chat = 0;
    
    // é�� Ŭ����� 
    public GameObject Obstacle;
    
    private Renderer _renderer;

    // 챕터 클리어용 
    public bool isClear = false;

    // 라이팅 변경용
    [Header("References")]
    [SerializeField]
    private RoomLighting roomLighting;

    [Header("Chapter02 Spawn Objects")] 
    public Ch02ObjectSpawner[] ch02ObjectSpawners;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
    }

    void Start()
    {
        _ChapterControl = Player.GetComponent<ChapterControl>();
        
        if(Character.name == "Character_Cat")
        {
            _Cat_AniManage = Character.GetComponent<Cat_AniManage>();
        }
        else
        {
            _AniManage = Character.GetComponent<AniManage>();
        }
    }

    void Update()
    {   
        if(Character.name == "Character_Cat")
        {
            if (_Cat_AniManage.isChange && !isChatFin)
            {   
                _renderer.enabled = true;
                StartCoroutine(Delay_Change(1.5f));

                isChatFin = true;
            }

            if(Chat == chatMaterials.Length)
            {
                _Cat_AniManage.animationComponent.Stop();
                _Cat_AniManage.animationComponent.clip = doorAni;
                _Cat_AniManage.animationComponent.Play();

                Chat++;
            }
        }
        else
        {
            if (_AniManage.isChange && !isChatFin)
            {   
                GetComponent<Renderer>().enabled = true;
                StartCoroutine(Delay_Change(1.5f));

                isChatFin = true;
            }
            if(Chat == chatMaterials.Length)
            {
                _AniManage.animationComponent.Stop();
                _AniManage.animationComponent.clip = doorAni;
                _AniManage.animationComponent.Play();

                Chat++;
            }
        }

        if(Character.name == "Character_Clock")
        {
            if(isChatFin && Chat > chatMaterials.Length && !isClear)
            {
                // 길을 막는 오브젝트 비활성화
                Obstacle.SetActive(false);

                // 앰비언트 라이팅 조정
                roomLighting.ApplyAmbientColorByChapter(Chapter.Chapter2);

                // 오브젝트 지나다니는거 활성화
                // OBJspawn's SpawnCH02obj.cs -> isSpawn True 
                foreach (var spawner in ch02ObjectSpawners) { spawner.StartSpawn(); }

                isClear =  true;
            }
        }
        else if(Character.name == "Character_Cactus")
        {
            if(isChatFin && Chat >  chatMaterials.Length && !isClear)
            {
                // 길을 막는 오브젝트 비활성화
                Obstacle.SetActive(false);

                // 앰비언트 라이팅 조정
                roomLighting.ApplyAmbientColorByChapter(Chapter.Chapter3);

                isClear =  true;
            }
        }
    }

    IEnumerator Delay_Change(float delayTime)
    {   
        foreach(Material material in chatMaterials) // Material 배열 순회
        {
            yield return new WaitForSeconds(delayTime); // 딜레이
            ChangeMaterial(material); // 다음 Material로 변경
            Chat++;
        }
    }

    // 머테리얼 변경
    public void ChangeMaterial(Material newMaterial)
    {
        _renderer.material = newMaterial; 
    }
}
