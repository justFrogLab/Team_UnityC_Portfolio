using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    float hAxis;
    float vAxis;
    bool walkDwon;
    bool jumpDown;
    bool divingDown;
    bool isJump;
    bool isDiving;
    [SerializeField]
    private Camera currentCamera;

    Vector3 moveVec;
    Vector3 divingVec;
    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f);
        GetInput();
        Move();
        Jump();
        Diving();
    }
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDwon = Input.GetKey(KeyCode.LeftShift);
        jumpDown = Input.GetKeyDown(KeyCode.Space);
        divingDown = Input.GetMouseButtonDown(1);
    }
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDiving)
            moveVec = divingVec;
        Quaternion cameraRotation = Quaternion.Euler(0f, currentCamera.transform.eulerAngles.y, 0f);
        moveVec = cameraRotation * moveVec;
        bool isRun = moveVec.magnitude != 0;
        anim.SetBool("isRun", isRun);
        anim.SetBool("isWalk", walkDwon);
        if (isRun || walkDwon)
        {
            transform.position += moveVec * speed * (walkDwon ? 0.3f : 1f) * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 0.1f);
        }

    }
    private void Jump()
    {
        if (jumpDown && !isJump && !isDiving)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FLOOR")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    private void Diving()
    {
        if (divingDown && !isJump && !isDiving && moveVec.magnitude != 0)
        {
            divingVec = new Vector3(hAxis, 0, vAxis).normalized;
            anim.SetTrigger("isDiving");
            isDiving = true;


            Invoke("DivingOut", 0.75f);
        }
    }
    private void DivingOut()
    {
        isDiving = false;
    }
}
