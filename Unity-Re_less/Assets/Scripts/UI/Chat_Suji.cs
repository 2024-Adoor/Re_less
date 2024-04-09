using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat_Suji : MonoBehaviour
{
    public GameObject Player;
    public GameObject Suji;
    
    public Material Chat02;
    public Material Chat03;
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
        
        if (_SujiManage.isEffectStop && !isChatFin)
        {   
            GetComponent<Renderer>().enabled = true;
            StartCoroutine(Delay_Change(1.5f));
            isChatFin = true; 
        }

        Renderer rend = GetComponent<Renderer>();
        if (rend.material.name.Contains("Suji_3"))
        {   
            Debug.Log("Chat_Suji's material contains Suji_3 !!");

            if (!isDelayedActionStarted)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= delayTime)
                {
                    Debug.Log("Suji can Move");
                    // 딜레이가 종료되면 실행할 코드
                    _SujiEndingTest.canMove = true;
                    //Debug.Log("Delayed action after " + delayTime + " seconds.");
                    isDelayedActionStarted = true;
                }
            }
        }

        // 수지 이동할 때 enabled false 
        if(_SujiEndingTest.canMove)
        {
            GetComponent<Renderer>().enabled = false;
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
