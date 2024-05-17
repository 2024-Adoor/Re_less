using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Suji : MonoBehaviour
{
    public GameObject Player;
    public GameObject Suji;
    
    public Material Chat02;
    public Material Chat03;
    public Material Chat04;
    public Material Chat05;

    SujiEndingTest _SujiEndingTest;
    SujiManage _SujiManage;

    public bool isChatFin = false;

    private float elapsedTime = 0f;
    private float delayTime = 2f;
    private bool isDelayedActionStarted = false;
    
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {   
        if(Suji != null)
        {
            _SujiManage = Suji.GetComponent<SujiManage>();
            _SujiEndingTest = Suji.GetComponent<SujiEndingTest>();
        }
        
        // isRotateFin2 = true
        if (!isChatFin && _SujiManage.isRotateFin2)
        {   
            GetComponent<Renderer>().enabled = true;
            StartCoroutine(Delay_Change(1.5f));
            isChatFin = true; 
        }

        Renderer rend = GetComponent<Renderer>();
        if (rend.material.name.Contains("수지 5"))
        {   
            Debug.Log("Chat_Suji's material contains 수지 5 !!");

            if (!isDelayedActionStarted)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    Debug.Log("Suji can Move");
                    _SujiEndingTest.WakeUp();
                    
                    //Debug.Log("Delayed action after " + delayTime + " seconds.");
                    isDelayedActionStarted = true;
                }
            }
        }
    }

    IEnumerator Delay_Change(float delayTime)
    {   
        yield return new WaitForSeconds(delayTime); // 딜레이
        ChangeMaterial(Chat02);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat03);
        yield return new WaitForSeconds(delayTime); // 딜레이
        ChangeMaterial(Chat04);
        yield return new WaitForSeconds(delayTime); 
        ChangeMaterial(Chat05);
    }

    // 머테리얼 변경
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial; 
    }
}
