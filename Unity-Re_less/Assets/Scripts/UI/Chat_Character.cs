using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Character : MonoBehaviour
{
    public GameObject Player;
    public GameObject Character;
    
    public Material[] chatMaterials; // Material �迭
    public bool isChatFin = false;

    public AnimationClip doorAni;
    int Chat = 0;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {   
        AniManage _AniManage = Character.GetComponent<AniManage>();

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

    IEnumerator Delay_Change(float delayTime)
    {   
        foreach(Material material in chatMaterials) // Material �迭 ��ȸ
        {
            yield return new WaitForSeconds(delayTime); // ������
            ChangeMaterial(material); // ���� Material�� ����
            Chat++;
        }
    }

    // ���׸��� ����
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
