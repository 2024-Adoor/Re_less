using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSuji : MonoBehaviour
{
    public GameObject KeyboardEnter;        // Keyboard
    public GameObject Ch03Fruit;
    public GameObject SleepOut_SujiPrefab;

    // ����� ����
    public AudioClip fruit_get;
    private AudioSource audioSource;

    public bool isSleepOut = false;
    public bool isDetected = false;

    // �ִϸ��̼� ����
    bool isChange = false;

    // Start is called before the first frame update
    void Start()
    {
        SleepOut_SujiPrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard _Keyboard = KeyboardEnter.GetComponent<Keyboard>();

        // ���� �ε������� SleepOut �ִϸ��̼� ���������� ��ȯ 
        if(isDetected && _Keyboard.enterDown)
        {
            SleepOut_SujiPrefab.SetActive(true);

            Destroy(gameObject);
            Destroy(Ch03Fruit);

            Debug.Log("Suji is SleepOut true - SleepingSuji");
        }
    }


    private void OnTriggerEnter(Collider other)
    {   
        // �������� Fruit �±� �浹�� isDetected = true
        if (other.gameObject.CompareTag("Fruit"))
        {   
            isDetected = true;
            audioSource.PlayOneShot(fruit_get);
        }
    }
}
