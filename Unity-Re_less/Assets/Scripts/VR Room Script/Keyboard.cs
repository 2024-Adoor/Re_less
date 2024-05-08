using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public float downwardAmount = 0.3f; // �������� ��
    public float downwardSpeed = 0.1f; // �������� �ӵ�
    public bool enterDown = false; 

    public GameObject Ch03Fruit;       // Fruit & Cursor 
    Ch03_FruitSnap _Ch03_FruitSnap;

    private bool hasCollided = false; // �浹 ���� üũ
    private Vector3 initialPosition; // �ʱ� ��ġ ����

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
        initialPosition = transform.position; // �ʱ� ��ġ ����
        PressEffect.Stop();

        AudioSource = GetComponent<AudioSource>(); // AudioSource�� ������
        EnterPopupRender = EnterPopup.GetComponent<Renderer>();
        MouseRend = Mouse.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (hasCollided)
        {
            // �Ʒ��� �̵�
            transform.position -= Vector3.up * downwardSpeed * Time.deltaTime;
            enterDown = true;

            // Ư�� ��ġ �Ʒ��� �̵� �Ϸ� ��
            if (transform.position.y <= initialPosition.y - downwardAmount)
            {
                hasCollided = false; // �浹 ���� �ʱ�ȭ
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"�� ���
        if (collision.gameObject.CompareTag("Player"))
        {
            if(Ch03Fruit != null)
            {
                _Ch03_FruitSnap = Ch03Fruit.GetComponent<Ch03_FruitSnap>();

                if(_Ch03_FruitSnap.isDetected)
                {
                    AudioSource.PlayOneShot(EnterSound);

                    hasCollided = true; // �浹 ���� ����
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
