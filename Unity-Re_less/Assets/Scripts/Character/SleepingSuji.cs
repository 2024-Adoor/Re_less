using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSuji : MonoBehaviour
{
    public GameObject KeyboardEnter;        // Keyboard
    public GameObject Ch03Fruit;
    public GameObject SujiPrefab;

    public bool isSleepOut = false;
    public bool isDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard _Keyboard = KeyboardEnter.GetComponent<Keyboard>();
        SujiManage _SujiManage = SujiPrefab.GetComponent<SujiManage>();

        if(isDetected && _Keyboard.enterDown)
        {
            _SujiManage.isSleepOut = true;
            Destroy(gameObject);
            Destroy(Ch03Fruit);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        // 수지한테 Fruit 태그 충돌시 isDetected = true
        if (other.gameObject.CompareTag("Fruit"))
        {   
            isDetected = true;
        }
    }
}
