using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SujiManage : MonoBehaviour
{
    public Animation animationComponent;    // Animation ������Ʈ ����
    public GameObject SujiChat;

    public AnimationClip JumpInAni;         // JumpIn Animation Clip
    public AnimationClip JumpOutAni;        // JumpOut Animation Clip
    public AnimationClip IdleAni;           // IDLE Animation Clip 

    // ���� + �̵�
    public Transform target;                // �̵� ������
    public float rotationSpeed = 5f;        // ȸ�� �ӵ�
    public float moveSpeed = 5f;            // �̵� �ӵ�
    Quaternion targetRotation;
    Quaternion targetRotation2;

    public bool isChange = false;
    public bool isSleepOut = false;
    bool isDelay = true;
    bool isJumpInAni = true;

    bool isJump = false;
    bool isMove = false;
    bool isRotateFin = false;
    bool isJumpFin = false;
    bool isMoveFin = false;
    
    // ����ȸ�� & ��� ����
    bool isRotate2 = false;
    public bool isRotateFin2 = false;

    // VFX 
    public ParticleSystem SleepOutEffect;

    // ��¥ ������ �κ� �λ��ϴ� �ִϸ��̼� ��ȯ
    public GameObject Player;
    public AnimationClip ByeAni;           // IDLE Animation Clip 
    PlayerState _PlayerState;
    bool isBye = false;
    bool isRotate = false;

    // Sound
    public AudioClip HealingSound;
    private AudioSource HealingAudioSource;

    void Start()
    {
        SujiChat.SetActive(false);
        gameObject.SetActive(false);
        SleepOutEffect.Play();
        HealingAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        _PlayerState = Player.GetComponent<PlayerState>();

        // SleepOut �ִϸ��̼� ������ ����Ʈ ���� & Ű���� ������ ���� + �̵�
        if (animationComponent.IsPlaying("SleepOut") == false && !isChange)
        {
            if(isDelay)
            {
                // ������ n�� ���� �� else �ڵ� ���� 
                StartCoroutine(Wait3f()); // Wait() �ڷ�ƾ�� �����մϴ�.
            }
            else
            {
                // ����Ʈ ����
                SleepOutEffect.Stop();

                // ȸ��
                if (!isRotate)
                {
                    // ȸ���� �ϳ��� �����ϰ� ���Ŀ��� �� �̻� ȸ������ �ʵ��� isRotate�� true�� �����մϴ�.
                    isRotate = true;
                
                    // ��ǥ ȸ�� ���� ����
                    targetRotation = Quaternion.Euler(0, -120, 0);
                }

                // ȸ�� ����
                if (isRotate && !isRotateFin)
                {
                    Debug.Log("ȸ�� ���� ������");

                    // Lerp �Լ��� ����Ͽ� �ε巴�� ȸ���մϴ�.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    
                    // ���� ȸ�� ������ ��ǥ ȸ�� ������ ���� ������ ȸ���� �Ϸ��ߴٰ� �����ϰ� isRotate�� false�� ����
                    if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                    {
                        isRotateFin = true;
                        isMove = true;

                        // �ִϸ��̼� ���� - JumpIn
                        if (animationComponent != null && JumpInAni != null)
                        {
                            animationComponent.Stop();
                            animationComponent.clip = JumpInAni;
                        }

                        Debug.Log("isRotateFin = true");
                    }
                }

                // ȸ�� ���� & ������ �̵� ����
                if(isMove && !isMoveFin)
                {
                    Debug.Log("target���� �̵���");

                    if(isJumpInAni)
                    {
                        animationComponent.Play();

                        // 2�� �� ��� ���� 
                        StartCoroutine(Wait2f());
                    }
                    else
                    {
                        MoveNPC(target.position, 3f);

                        if (Vector3.Distance(transform.position, target.position) < 0.1f)
                        {
                            // �ִϸ��̼� ���� - JumpOut
                            if (animationComponent != null && JumpOutAni != null)
                            {
                                animationComponent.Stop();
                                animationComponent.clip = JumpOutAni;
                            }
                            animationComponent.Play();
                            isMoveFin = true;
                            isRotate2 = true;
                            Debug.Log("isMoveFin = true");
                        }
                    }
                }
                
                // �̵� ���� -> �������� ȸ�� & IDLE �ִϸ��̼� ��� & ��� ����
                // ���� ȸ�� ����
                if(!isRotate2)
                {
                    // ��ǥ ȸ�� ���� ����
                    targetRotation2 = Quaternion.Euler(0, -90, 0);
                }
                else if(isRotate2 && !isRotateFin2 && isMoveFin)
                {
                    // Lerp �Լ��� ����Ͽ� �ε巴�� ȸ���մϴ�.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation2, Time.deltaTime * rotationSpeed);

                    // ���� ȸ�� ������ ��ǥ ȸ�� ������ ���� ������ ȸ���� �Ϸ��ߴ�
                    if (Quaternion.Angle(transform.rotation, targetRotation2) < 1f)
                    {
                        isRotateFin2 = true;
                        SujiChat.SetActive(true);
                    }
                }
            }
        }
       
        // _PlayerState isTeleport true -> ȸ�� & �ִϸ��̼� // �÷��̾ �� �Ѱ�� �����Ǿ��� ��
        if(_PlayerState.isTeleport)
        {
            if(!isRotate)
            {
                transform.Rotate(0, 180, 0);
                isRotate = true;
            }

            // Bye �ִϸ��̼� Ŭ������ ��ȯ
            if (animationComponent != null && ByeAni != null && !isBye)
            {
                Debug.Log("Suji Bye");
                animationComponent.Stop();
                animationComponent.clip = ByeAni;
                animationComponent.Play();

                isBye = true;
            }
        }
    }

    // 3�� ������
    private IEnumerator Wait3f()
    {
        yield return new WaitForSeconds(3f);
        isDelay = false;
    }

    private IEnumerator Wait2f()
    {
        yield return new WaitForSeconds(1f);
        isJumpInAni = false;
    }

    // ������ �ð� ���� ������ �̵���Ű�� �Լ�
    public void MoveNPC(Vector3 destination, float duration)
    {
        StartCoroutine(MoveCoroutine(destination, duration));
    }

    private System.Collections.IEnumerator MoveCoroutine(Vector3 destination, float duration)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination; // ��Ȯ�� ��ǥ ��ġ�� �̵�
    }
}
