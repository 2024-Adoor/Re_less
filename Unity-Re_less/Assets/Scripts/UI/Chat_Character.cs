using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Character : MonoBehaviour
{
    public GameObject Player;
    public GameObject Character;
    
    public Material[] chatMaterials; // Material 배열
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
        foreach(Material material in chatMaterials) // Material 배열 순회
        {
            yield return new WaitForSeconds(delayTime); // 딜레이
            ChangeMaterial(material); // 다음 Material로 변경
        }
    }

    // 머테리얼 변경
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
