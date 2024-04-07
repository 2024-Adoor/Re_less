﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Suji : MonoBehaviour
{
    public GameObject Player;
    public GameObject Suji;
    
    public Material Chat02;
    public Material Chat03;

    bool isChatFin = false;

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {   
        AwakeSuji _AwakeCharacters = Suji.GetComponent<AwakeSuji>();

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
        
    }

    // 머테리얼 변경
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
