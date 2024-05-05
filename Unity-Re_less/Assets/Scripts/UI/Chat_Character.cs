using System.Collections;
using Reless;
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

<<<<<<< HEAD
    
    // é�� Ŭ����� 
    public GameObject Obstacle;
=======
    public GameObject Obstacle;
    
    private Renderer _renderer;

    // 챕터 클리어용 
>>>>>>> a3a251dd35a8654f70b70db7ed57debbf921917b
    bool isClear = false;
    public Canvas  UI;
    UI_Canvas _UI_Canvas;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
    }

    void Start()
    {
        _ChapterControl = Player.GetComponent<ChapterControl>();
<<<<<<< HEAD
        _UI_Canvas = UI.GetComponent<UI_Canvas>();
=======
        
        if(Character.name == "Character_Cat")
        {
            _Cat_AniManage = Character.GetComponent<Cat_AniManage>();
        }
        else
        {
            _AniManage = Character.GetComponent<AniManage>();
        }
>>>>>>> a3a251dd35a8654f70b70db7ed57debbf921917b
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
            if(isChatFin && Chat >  chatMaterials.Length && !isClear)
            {
                // 길을 막는 오브젝트 비활성화
                Obstacle.SetActive(false);

<<<<<<< HEAD
                // é�� 2 ����
                _ChapterControl.SetupChapter02();
                _UI_Canvas.Chapter02_StartUI();
=======
                // 챕터 2 시작
                _ChapterControl.CurrentChapter = Chapter.Chapter2;
>>>>>>> a3a251dd35a8654f70b70db7ed57debbf921917b
                isClear =  true;
            }
        }
        else if(Character.name == "Character_Cactus")
        {
            if(isChatFin && Chat >  chatMaterials.Length && !isClear)
            {
                // 길을 막는 오브젝트 비활성화
                Obstacle.SetActive(false);

<<<<<<< HEAD
                // é�� 3 ����
                _ChapterControl.SetupChapter03();
                _UI_Canvas.Chapter03_StartUI();
=======
                // 챕터 3 시작
                _ChapterControl.CurrentChapter = Chapter.Chapter3;
>>>>>>> a3a251dd35a8654f70b70db7ed57debbf921917b
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
