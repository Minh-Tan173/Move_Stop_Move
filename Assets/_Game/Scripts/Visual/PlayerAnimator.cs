using UnityEngine;

public class PlayerAnimator : ICharacterAnimator
{
    [Header("Parent")]
    [SerializeField] private Player player;

    [Header("Ref component")]
    [SerializeField] private Animator animator;

    public override void ResetAnim() {


    }

    public override void TriggerAttackAnim() {
        animator.SetTrigger(CharacterConst.ATTACK_ANIM);
    }

    public override void TriggerDeadAnim() {

        animator.SetTrigger(CharacterConst.DEAD_ANIM);
    }
}
