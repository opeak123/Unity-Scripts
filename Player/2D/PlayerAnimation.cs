using UnityEngine;
using UnityEngine.Animations;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    //�÷��̾� Controller
    private PlayerController playerController;
    //�÷��̾� �ִϸ��̼�
    private Animator animator;
    //�÷��̾� SpriteRenderer
    public SpriteRenderer m_spriteNone;
    //�÷��̾ ������ SpriteRenderer
    public Sprite m_spriteGun;
    private bool b_hasGun = false;
    public RuntimeAnimatorController m_defaultController;
    public RuntimeAnimatorController m_subMachineController;


    private void Awake()
    {
        //�Ҵ�
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        m_spriteNone = GetComponent<SpriteRenderer>();
    }
    
    public void SetHasGun(bool hasGun)
    {
        this.b_hasGun = hasGun;

        if (hasGun)
        {
            animator.runtimeAnimatorController = m_subMachineController;
            m_spriteNone.sprite = m_spriteGun; 
        }
    }

    
    public void SetIsGrounded(bool isGrounded) //idle �ִϸ��̼�
    {
        animator.SetBool("idle", isGrounded);
    }
    public void SetSpeed(float speed)   //walk �ִϸ��̼�
    {
        animator.SetFloat("speed", speed);
    }
    public void TriggerJump()   //jump �ִϸ��̼�
    {
        animator.SetTrigger("jump");
    }

    public void TriggerDash() //dash �ִϸ��̼�
    {
        animator.SetTrigger("dash");
    }
    public void TriggerCrouch() //crouch �ִϸ��̼�
    {
        animator.SetTrigger("crouch");
    }
    public void TriggerSleep() //sleep �ִϸ��̼�
    {
        animator.SetBool("sleep", true);
    }

    public void TriggerWakeUp() //wake up �ִϸ��̼�
    {
        animator.SetBool("wakeUp", true);
    }
    public void TriggerLookingUp() // looking up �ִϸ��̼�
    {
        animator.SetTrigger("lookingUp");
    }
    public void BooleanLaddering(bool value) //laddering �ִϸ��̼�
    {
        animator.SetBool("laddering",true);
    }
    public void TriggerDead() //dead �ִϸ��̼�
    {
        animator.SetTrigger("dead");
    }
}