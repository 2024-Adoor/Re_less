using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Clock : MonoBehaviour
{
    public GameObject Player;
    public GameObject Clock;
    
    public Material Chat02;
    public Material Chat03;
    public Material Chat04;
    public Material Chat05;
    public Material Chat06;
    public Material Chat07;

    bool isChatFin = false;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {   
        AwakeCharacters _AwakeCharacters = Clock.GetComponent<AwakeCharacters>();

        if (_AwakeCharacters.isChange && !isChatFin)
        {   
            GetComponent<Renderer>().enabled = true;
            StartCoroutine(Delay_Change(1.5f));
            isChatFin = true;
        }
        
    }

    IEnumerator Delay_Change(float delayTime)
    {   
        yield return new WaitForSeconds(delayTime); // 딜레이 
        ChangeMaterial(Chat02);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat03);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat04);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat05);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat06);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat07);
        
    }

    // 머테리얼 변경
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
