using UnityEngine;

public class BotAnimator : CharacterAnimatorBase {

    [SerializeField] private Bot bot;
    [SerializeField] private Animator animator;

    private void Update() {

        HandleIdleAnim();   
    }

    public override void HandleIdleAnim() {

        animator.SetBool(CharacterConst.IS_MOVING, bot.IsMoving());
    }

    public override void ResetAnim() {
        animator.Rebind();
        animator.Update(0f);
    }

    public override void TriggerAttackAnim() {

        animator.SetTrigger(CharacterConst.ATTACK_ANIM);
    
    }

    public override void TriggerDeadAnim() {

        animator.SetTrigger(CharacterConst.DEAD_ANIM);
    }
}
