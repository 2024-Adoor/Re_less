using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Character : MonoBehaviour
{
    public GameObject Player;
    public GameObject Character;
    
    public Material[] chatMaterials; // Material �迭
    public bool isChatFin = false;

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
    }

    IEnumerator Delay_Change(float delayTime)
    {   
        foreach(Material material in chatMaterials) // Material �迭 ��ȸ
        {
            yield return new WaitForSeconds(delayTime); // ������
            ChangeMaterial(material); // ���� Material�� ����
        }
    }

    // ���׸��� ����
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
