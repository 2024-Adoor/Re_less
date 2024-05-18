using System;
using UnityEngine;

public class SleepingSuji : MonoBehaviour
{
    public GameObject KeyboardEnter;        // Keyboard
    public GameObject Ch03Fruit;
    public GameObject SleepOut_SujiPrefab;

    // 오디오 관리
    public AudioClip fruit_get;
    private AudioSource audioSource;

    public bool isSleepOut = false;
    public bool isDetected = false;

    public event Action TriggerFruitEntered;

    // 애니메이션 제어
    bool isChange = false;
    
    private Keyboard _Keyboard;

    // Start is called before the first frame update
    void Start()
    {
        SleepOut_SujiPrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        _Keyboard = KeyboardEnter.GetComponent<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        // 열매 부딪혔을때 SleepOut 애니메이션 프리팹으로 전환 
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
        // 수지한테 Fruit 태그 충돌시 isDetected = true
        if (other.gameObject.CompareTag("Fruit"))
        {   
            TriggerFruitEntered?.Invoke();
            isDetected = true;
            audioSource.PlayOneShot(fruit_get);
        }
    }
}
