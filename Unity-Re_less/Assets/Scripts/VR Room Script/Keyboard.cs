using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public float downwardAmount = 0.3f; // 내려가는 양
    public float downwardSpeed = 0.1f; // 내려가는 속도
    public bool enterDown = false; 

    public GameObject Ch03Fruit;       // Fruit & Cursor 
    Ch03_FruitSnap _Ch03_FruitSnap;

    private bool hasCollided = false; // 충돌 여부 체크
    private Vector3 initialPosition; // 초기 위치 저장

    public GameObject EnterPopup;
    Renderer EnterPopupRender;

    public GameObject Mouse;
    Renderer MouseRend;

    // Sound
    public AudioClip EnterSound;
    private AudioSource AudioSource;

    // VFX 
    public ParticleSystem PressEffect;

    private void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
        PressEffect.Stop();

        AudioSource = GetComponent<AudioSource>(); // AudioSource를 가져옴
        EnterPopupRender = EnterPopup.GetComponent<Renderer>();
        MouseRend = Mouse.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (hasCollided)
        {
            // 아래로 이동
            transform.position -= Vector3.up * downwardSpeed * Time.deltaTime;
            enterDown = true;

            // 특정 위치 아래로 이동 완료 시
            if (transform.position.y <= initialPosition.y - downwardAmount)
            {
                hasCollided = false; // 충돌 여부 초기화
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"인 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            if(Ch03Fruit != null)
            {
                _Ch03_FruitSnap = Ch03Fruit.GetComponent<Ch03_FruitSnap>();

                if(_Ch03_FruitSnap.isDetected)
                {
                    AudioSource.PlayOneShot(EnterSound);

                    hasCollided = true; // 충돌 여부 설정
                    PressEffect.Stop();
                    EnterPopupRender.enabled = false;
                    MouseRend.enabled = false;

                    for(int i=0; i<3; i++){
                        Mouse.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    
                }
            }
        }
    }
}
