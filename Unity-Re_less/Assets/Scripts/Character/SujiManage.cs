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

    public Transform JumpTarget;            // ���� ������? 

    public bool isChange = false;
    public bool isSleepOut = false;
    bool isDelay = true;
    bool isJump = false;
    bool isMove = false;
    bool isMoveFin = false;

    // VFX 
    public ParticleSystem SleepOutEffect;

    // ��¥ ������ �κ� �λ��ϴ� �ִϸ��̼� ��ȯ
    public GameObject Player;
    public AnimationClip ByeAni;           // IDLE Animation Clip 
    PlayerState _PlayerState;
    bool isBye = false;
    bool isRotate = false;

    void Start()
    {
        gameObject.SetActive(false);
        SleepOutEffect.Play();
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
                StartCoroutine(Wait()); // Wait() �ڷ�ƾ�� �����մϴ�.
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
                if (isRotate)
                {
                    // Lerp �Լ��� ����Ͽ� �ε巴�� ȸ���մϴ�.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    
                    // ���� ȸ�� ������ ��ǥ ȸ�� ������ ���� ������ ȸ���� �Ϸ��ߴٰ� �����ϰ� isRotate�� false�� ����
                    if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                    {
                        isRotate = false;
                        isMove = true;
                    }
                }

                // ȸ�� ���� & ������ �̵� ����
                if(isJump)
                {
                    //MoveToTarget(JumpTarget);
                }

                if(isMove)
                {
                    MoveNPC(target.position, 2f);
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

    // n�� ������
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        isDelay = false;
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
