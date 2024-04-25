using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Character : MonoBehaviour
{
    public GameObject Player;
    public GameObject Character;
    AniManage _AniManage;
    Cat_AniManage _Cat_AniManage;
    
    public Material[] chatMaterials; // Material 배열
    public bool isChatFin = false;

    public AnimationClip doorAni;
    int Chat = 0;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {   
        if(Character.name == "Character_Cat")
        {
            _Cat_AniManage = Character.GetComponent<Cat_AniManage>();

            if (_Cat_AniManage.isChange && !isChatFin)
            {   
                GetComponent<Renderer>().enabled = true;
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
            _AniManage = Character.GetComponent<AniManage>();

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
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
