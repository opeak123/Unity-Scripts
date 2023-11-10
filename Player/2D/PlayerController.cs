using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;                     //플레이어 RigidBody2D
    private PlayerAnimation playerAnimation;    //플레이어 애니메이션

    private float m_moveSpeed = 3f;              //플레이어 이동속도
    private float m_jumpForce = 5f;              //플레이어 점프높이
    private bool b_isJumping = false;            //플레이어 점프여부
    private bool b_isGrounded;                   //플레이어가 바닥에 닿았는지 체크
    private float m_groundCheckRadius = 0.2f;    //Ground 체크 반지름
    private LayerMask m_whatIsGround;            //Layer 3번 - Ground 
    private Transform m_groundCheck;             //플레이어 Transform           
    private float m_moveInputX;                  //플레이어 X값 키 방향
    private float m_dashTime;                   //플레이어 Dash 시간체크
    private float m_dashPower = 4000f;          //플레이어 Dash 파워
    private bool b_isDashing = false;       //플레이어 Dash 중인지 체크
    private bool b_laddering = false;       //플레이어가 사다리에 올랐는지 체크
    private bool b_isWallSliding = false;   //플레이어가 벽에 닿았는지 체크
    private bool b_hasGun = false;              //플레이어가 총을 가졌는지
    private bool b_isCrouch = false;            //플레이어가 앉았는지

    public void SetHasGun(bool hasGun) //캡슐화 
    {
        this.b_hasGun = hasGun;
    }

    public void SetCrouch(bool isCrouch) //캡슐화 
    {
        this.b_isCrouch = isCrouch;
    }

    private void Awake()
    {
        //할당
        m_whatIsGround = LayerMask.GetMask("Ground");
        m_groundCheck = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        //게임매니저에서 플레이어 이동 제어
        if (!GameManager.Instance.canMove && GameManager.Instance.dead)
            return;

        if (GameManager.Instance.canMove && !GameManager.Instance.dead)
        {
            PlayerMove();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.canMove)
            return;

        //플레이어의 X값 이동
        if (GameManager.Instance.canMove)
        {
            m_moveInputX = Input.GetAxis("Horizontal");
            if (!b_isWallSliding)
                rb.velocity = new Vector2(m_moveInputX * m_moveSpeed, rb.velocity.y);

            if (m_moveInputX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (m_moveInputX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        //사다리에 올랐을때
        //1.카메라가 플레이어 추적
        //2.무중력상태 
        if (b_laddering)
        {
            float moveInputY = Input.GetAxis("Vertical");
            FindObjectOfType<CameraFollow>().followTarget = true;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, moveInputY * m_moveSpeed) / 2;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }


    //플레이어 이동함수
    void PlayerMove()
    {
        //Physics , 땋에 닿았는지 반지름 체크 , 애니메이션 재생
        b_isGrounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundCheckRadius, m_whatIsGround);
        playerAnimation.SetIsGrounded(b_isGrounded);

        if (b_isGrounded)
        {
            //플레이어가 땅에 닿았는지 체크하고 X값을 절대값으로 반환
            //애니메이션 재생
            playerAnimation.SetSpeed(Mathf.Abs(m_moveInputX));

            if (Input.GetKey(KeyCode.Space) && !b_isJumping)
            {
                b_isJumping = true;
                SoundManager.Instance.PlaySFX("player-jump-sfx", 1f);
                rb.velocity = Vector2.up * m_jumpForce;
                playerAnimation.TriggerJump();
            }

        }

        //플레이어 Dash 부분 구현 , 애니메이션 재생
        if (Input.GetKeyDown(KeyCode.C) && !b_isDashing)
        {
            rb.AddForce(new Vector2(transform.localScale.x * m_dashPower, rb.velocity.y));
            playerAnimation.TriggerDash();
            b_isDashing = true;
        }
        if (b_isDashing)
        {
            m_dashTime -= Time.deltaTime;

            if (m_dashTime <= 0)
            {
                b_isDashing = false;
                rb.velocity = new Vector2(0, 0);
            }
        }

        //플레이어가 앉았을 때 레이어 감지
        //Ground에서만 앉기 가능
        if (Input.GetKey(KeyCode.DownArrow) && !b_isJumping && !rb.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                b_isCrouch = true;
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                playerAnimation.TriggerCrouch();
                playerAnimation.SetIsGrounded(false);
            }
        }
        //키를 땠을 때 Rigidbody의 제약 해제
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            b_isCrouch = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimation.SetIsGrounded(true);
        }
    }

    ////////////////////////////////////COLLISION//////////////////////////
    //Collision 충돌 함수 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어가 벽에 닿았을 때 Dash를 하지 못하게
        //Vector2 값을 0으로 초기화
        if (collision.gameObject.CompareTag("WALL"))
        {
            b_isJumping = true;
            b_isDashing = false;
            rb.AddForce(new Vector2(0f, 0f));
        }
        if (collision.gameObject.layer == 3)
        {
             b_isJumping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //플레이어가 벽에 닿았을 때 좌우 방향키를 눌러도 미끄러지게 
        if (collision.gameObject.CompareTag("WALL"))
        {
            b_isWallSliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        b_isWallSliding = false;
    }

    ////////////////////////////////////TRIGGER//////////////////////////
    //Trigger 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 사다리에 오를수 있게 태그 감지
        if (collision.gameObject.CompareTag("LADDER"))
        {
            b_laddering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LADDER"))
        {
            b_laddering = false;
        }
    }
}

