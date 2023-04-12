using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterInfo
{
    float vAxis;
    float hAxis;
    float speed = 10;
    float gravity = -9.81f;
    Vector3 moveVec;
    public int _playerHP;



    bool wDown;
    bool a1Down, a2Down, a3Down, a4Down;
    bool jDown;

    bool isAttack = true;

    bool isWalk;
    bool isRun;
    bool isJump;
    bool isDead;

    
    public GameObject player;
    public CharacterController _controller;
    public Weapon weapon;
    public ChaInfo playerInfo;

    Camera _camera;
    Animator anim;
    Rigidbody rigid;
    void Start()
    {
        createPlayer();
        anim = player.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = player.GetComponent<CharacterController>();
        rigid = player.GetComponent<Rigidbody>();
    }


    void Update()
    {
        GetInput();
        Move();
        Jump();
        Turn();
        Attack();
        CheckDie();
        if (_controller.isGrounded == false)
        {
            moveVec.y += gravity * Time.deltaTime;
        }
        if(!isDead)
            _controller.Move(moveVec * speed * Time.deltaTime);
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        a1Down = Input.GetButtonDown("Attack1");
        a2Down = Input.GetButtonDown("Attack2");
        a3Down = Input.GetButtonDown("Attack3");
        a4Down = Input.GetButtonDown("Attack4");
    }

    void Move()
    {
        if (isDead)
        {
            moveVec = Vector3.zero;
            return;
        }
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        moveVec = (forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal")).normalized;


        anim.SetBool("IsWalk", moveVec != Vector3.zero);
        anim.SetBool("IsRun", wDown);
    }
    void Turn()
    {

        Vector3 dir = moveVec;
        if (dir.x != 0 || dir.y != 0 || dir.z != 0&&!isDead)
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);

        if (Input.GetMouseButton(0)&&!isDead)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * 10f);
        }
    }
    void Jump()
    {
        if (jDown && !isJump && moveVec == Vector3.zero && !isDead)
        {
            moveVec.y = 7.0f;
            //rigid.AddForce(Vector3.up * 150, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            //isJump = true;
        }
    }
    void Attack()
    {
        if (!isAttack|| isDead)
            return;
        int attacknum = -1;
        if (a1Down || a2Down || a3Down || a4Down)
        {
            if (a1Down)
                attacknum = 1;
            if (a2Down)
                attacknum = 2;
            if (a3Down)
                attacknum = 3;
            if (a4Down)
                attacknum = 4;
            weapon.Use(attacknum);
            switch (attacknum)
            {
                case 1:
                    anim.SetTrigger("doAttack" + attacknum);
                    break;
                case 2:
                    anim.SetTrigger("doAttack" + attacknum);
                    break;
                case 3:
                    anim.SetTrigger("doAttack" + attacknum);
                    break;
                case 4:
                    anim.SetTrigger("doAttack" + attacknum);
                    break;
            }
            isAttack = false;
            Invoke("AttackOut", 0.4f);
        }
    }
    void CheckDie()
    {
        if (playerInfo._hp <= 0&& !isDead)
            OnDie();
    }
    void AttackOut()
    {
        isAttack = true;
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        FreezeRotation();
    }
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
    }
    void createPlayer()
    {
        playerInfo._hp = 20;
        playerInfo._defence = 50;
        playerInfo._attack = 10;
    }
    
}
