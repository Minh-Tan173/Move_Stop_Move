using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private Player player;

    [Header("Ref component")]
    [SerializeField] private Animator animator;

    private const string IDLE_ANIM = "TriggerIdle";
    private const string IS_MOVING = "IsMoving";
    private const string DEAD_ANIM = "TriggerDead";
    private const string ATTACK_ANIM = "TriggerAttack";

    private void Update() {

        HandleIdleAnim();
    }

    private void HandleIdleAnim() {

        animator.SetBool(IS_MOVING, player.IsMoving());
    }

    public void ResetAnim() {
        
     
    }

    public void TriggerAttackAnim() {
        animator.SetTrigger(ATTACK_ANIM);
    }

    public void TriggerDeadAnim() {

        animator.SetTrigger(DEAD_ANIM);
    }
}
