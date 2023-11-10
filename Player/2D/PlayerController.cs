using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;                     //�÷��̾� RigidBody2D
    private PlayerAnimation playerAnimation;    //�÷��̾� �ִϸ��̼�

    private float m_moveSpeed = 3f;              //�÷��̾� �̵��ӵ�
    private float m_jumpForce = 5f;              //�÷��̾� ��������
    private bool b_isJumping = false;            //�÷��̾� ��������
    private bool b_isGrounded;                   //�÷��̾ �ٴڿ� ��Ҵ��� üũ
    private float m_groundCheckRadius = 0.2f;    //Ground üũ ������
    private LayerMask m_whatIsGround;            //Layer 3�� - Ground 
    private Transform m_groundCheck;             //�÷��̾� Transform           
    private float m_moveInputX;                  //�÷��̾� X�� Ű ����
    private float m_dashTime;                   //�÷��̾� Dash �ð�üũ
    private float m_dashPower = 4000f;          //�÷��̾� Dash �Ŀ�
    private bool b_isDashing = false;       //�÷��̾� Dash ������ üũ
    private bool b_laddering = false;       //�÷��̾ ��ٸ��� �ö����� üũ
    private bool b_isWallSliding = false;   //�÷��̾ ���� ��Ҵ��� üũ
    private bool b_hasGun = false;              //�÷��̾ ���� ��������
    private bool b_isCrouch = false;            //�÷��̾ �ɾҴ���

    public void SetHasGun(bool hasGun) //ĸ��ȭ 
    {
        this.b_hasGun = hasGun;
    }

    public void SetCrouch(bool isCrouch) //ĸ��ȭ 
    {
        this.b_isCrouch = isCrouch;
    }

    private void Awake()
    {
        //�Ҵ�
        m_whatIsGround = LayerMask.GetMask("Ground");
        m_groundCheck = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        //���ӸŴ������� �÷��̾� �̵� ����
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

        //�÷��̾��� X�� �̵�
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

        //��ٸ��� �ö�����
        //1.ī�޶� �÷��̾� ����
        //2.���߷»��� 
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


    //�÷��̾� �̵��Լ�
    void PlayerMove()
    {
        //Physics , ���� ��Ҵ��� ������ üũ , �ִϸ��̼� ���
        b_isGrounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundCheckRadius, m_whatIsGround);
        playerAnimation.SetIsGrounded(b_isGrounded);

        if (b_isGrounded)
        {
            //�÷��̾ ���� ��Ҵ��� üũ�ϰ� X���� ���밪���� ��ȯ
            //�ִϸ��̼� ���
            playerAnimation.SetSpeed(Mathf.Abs(m_moveInputX));

            if (Input.GetKey(KeyCode.Space) && !b_isJumping)
            {
                b_isJumping = true;
                SoundManager.Instance.PlaySFX("player-jump-sfx", 1f);
                rb.velocity = Vector2.up * m_jumpForce;
                playerAnimation.TriggerJump();
            }

        }

        //�÷��̾� Dash �κ� ���� , �ִϸ��̼� ���
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

        //�÷��̾ �ɾ��� �� ���̾� ����
        //Ground������ �ɱ� ����
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
        //Ű�� ���� �� Rigidbody�� ���� ����
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            b_isCrouch = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimation.SetIsGrounded(true);
        }
    }

    ////////////////////////////////////COLLISION//////////////////////////
    //Collision �浹 �Լ� 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾ ���� ����� �� Dash�� ���� ���ϰ�
        //Vector2 ���� 0���� �ʱ�ȭ
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
        //�÷��̾ ���� ����� �� �¿� ����Ű�� ������ �̲������� 
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
    //Trigger �Լ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾ ��ٸ��� ������ �ְ� �±� ����
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

