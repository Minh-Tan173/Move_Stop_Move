using UnityEngine;

public class BotAnimator : ICharacterAnimator {

    [SerializeField] private Bot bot;
    [SerializeField] private Animator animator;

    private void ResetAllTriggers() {
        animator.ResetTrigger(CharacterConst.IDLE_ANIM);
        animator.ResetTrigger(CharacterConst.RUN_ANIM);
        animator.ResetTrigger(CharacterConst.ATTACK_ANIM);
        animator.ResetTrigger(CharacterConst.DEAD_ANIM);
    }

    public override void ResetAnim() {

        animator.Rebind();
        animator.Update(0f);
    }

    public override void TriggerIdleAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.IDLE_ANIM);
    }

    public override void TriggerRunAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.RUN_ANIM);
    }

    public override void TriggerAttackAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.ATTACK_ANIM);
    
    }

    public override void TriggerDeadAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.DEAD_ANIM);
    }
}
