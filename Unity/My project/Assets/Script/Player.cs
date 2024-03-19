using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public Transform respawnPoint; // 리스폰 지점

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    bool isJump;

    Rigidbody rigid;
    Animator anim;

    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();

        if (transform.position.y < -50f || Input.GetKeyDown(KeyCode.R))
        {
            respawn();
        }
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("IsRun", moveVec != Vector3.zero);
        anim.SetBool("IsWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("IsJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            isJump = false;
            anim.SetBool("IsJump", false);
        }
        else if (collision.gameObject.tag == "car")
        {
            respawn();
        }
        else if (collision.gameObject.tag == "ship")
        {   
            isJump = false;
            anim.SetBool("IsJump", false);

            transform.position = collision.transform.position;
        }
    }


     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {
            Invoke("respawn", 0.3f); // 리스폰 함수를 respawnDelay 시간만큼 지연하여 호출
        }
    }

    void respawn()
    {
        transform.position = respawnPoint.position;
    }
}
