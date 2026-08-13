using UnityEngine;

public class PlayerAnimator : CharacterAnimatorBase
{
    [Header("Parent")]
    [SerializeField] private Player player;

    [Header("Ref component")]
    [SerializeField] private Animator animator;


    private void Update() {

        HandleIdleAnim();
    }

    public override void ResetAnim() {


    }

    public override void HandleIdleAnim() {

        animator.SetBool(CharacterAnimatorBase.IS_MOVING, player.IsMoving());
    }

    public override void TriggerAttackAnim() {
        animator.SetTrigger(CharacterAnimatorBase.ATTACK_ANIM);
    }

    public override void TriggerDeadAnim() {

        animator.SetTrigger(CharacterAnimatorBase.DEAD_ANIM);
    }
}
