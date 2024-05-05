using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSuji : MonoBehaviour
{
    public Animation animationComponent;    // Animation ������Ʈ ����

    public GameObject KeyboardEnter;        // Keyboard
    public GameObject Ch03Fruit;
    public GameObject SujiPrefab;

    public AnimationClip SleepOutAni;       // SleepOut Animation Clip

    public AnimationClip JumpInAni;         // JumpIn Animation Clip
    public AnimationClip JumpOutAni;        // JumpOut Animation Clip

    public bool isSleepOut = false;
    public bool isDetected = false;

    // �ִϸ��̼� ����
    bool isChange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard _Keyboard = KeyboardEnter.GetComponent<Keyboard>();
        SujiManage _SujiManage = SujiPrefab.GetComponent<SujiManage>();

        // ���� �ε������� SleepOut �ִϸ��̼� Ŭ������ ��ȯ 
        if(isDetected && _Keyboard.enterDown)
        {
            // SleepOut �ִϸ��̼� Ŭ������ ��ȯ
            if (animationComponent != null && SleepOutAni != null && !isChange)
            {
                Debug.Log("Suji SleepOut");
                animationComponent.Stop();
                animationComponent.clip = SleepOutAni;
                animationComponent.Play();

                isChange = true;
                isSleepOut = false;
            }

            // _SujiManage.isSleepOut = true;
            // Destroy(gameObject);
            Destroy(Ch03Fruit);

            Debug.Log("Suji is SleepOut true - SleepingSuji");
        }

        // �ῡ�� ���� �ִϸ��̼� ������ �� ����


    }


    private void OnTriggerEnter(Collider other)
    {   
        // �������� Fruit �±� �浹�� isDetected = true
        if (other.gameObject.CompareTag("Fruit"))
        {   
            isDetected = true;
        }
    }
}
