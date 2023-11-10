using UnityEngine;
using UnityEngine.Animations;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    //플레이어 Controller
    private PlayerController playerController;
    //플레이어 애니메이션
    private Animator animator;
    //플레이어 SpriteRenderer
    public SpriteRenderer m_spriteNone;
    //플레이어가 변경할 SpriteRenderer
    public Sprite m_spriteGun;
    private bool b_hasGun = false;
    public RuntimeAnimatorController m_defaultController;
    public RuntimeAnimatorController m_subMachineController;


    private void Awake()
    {
        //할당
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

    
    public void SetIsGrounded(bool isGrounded) //idle 애니메이션
    {
        animator.SetBool("idle", isGrounded);
    }
    public void SetSpeed(float speed)   //walk 애니메이션
    {
        animator.SetFloat("speed", speed);
    }
    public void TriggerJump()   //jump 애니메이션
    {
        animator.SetTrigger("jump");
    }

    public void TriggerDash() //dash 애니메이션
    {
        animator.SetTrigger("dash");
    }
    public void TriggerCrouch() //crouch 애니메이션
    {
        animator.SetTrigger("crouch");
    }
    public void TriggerSleep() //sleep 애니메이션
    {
        animator.SetBool("sleep", true);
    }

    public void TriggerWakeUp() //wake up 애니메이션
    {
        animator.SetBool("wakeUp", true);
    }
    public void TriggerLookingUp() // looking up 애니메이션
    {
        animator.SetTrigger("lookingUp");
    }
    public void BooleanLaddering(bool value) //laddering 애니메이션
    {
        animator.SetBool("laddering",true);
    }
    public void TriggerDead() //dead 애니메이션
    {
        animator.SetTrigger("dead");
    }
}