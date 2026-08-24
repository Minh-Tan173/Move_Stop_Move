using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

    [SerializeField] private Animator animator;

    private void ResetAllTriggers() {
        animator.ResetTrigger(CharacterConst.IDLE_ANIM);
        animator.ResetTrigger(CharacterConst.RUN_ANIM);
        animator.ResetTrigger(CharacterConst.ATTACK_ANIM);
        animator.ResetTrigger(CharacterConst.DEAD_ANIM);
    }

    public void ResetAnim() {

        animator.Rebind();
        animator.Update(0f);
    }

    public void TriggerIdleAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.IDLE_ANIM);
    }

    public void TriggerRunAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.RUN_ANIM);
    }

    public void TriggerAttackAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.ATTACK_ANIM);

    }

    public void TriggerDeadAnim() {

        ResetAllTriggers();
        animator.SetTrigger(CharacterConst.DEAD_ANIM);
    }
}
