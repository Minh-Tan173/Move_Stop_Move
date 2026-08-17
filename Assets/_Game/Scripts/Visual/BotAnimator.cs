using UnityEngine;

public class BotAnimator : ICharacterAnimator {

    [SerializeField] private Bot bot;
    [SerializeField] private Animator animator;

    private void Update() {

        HandleMovementAnim();   
    }

    public override void HandleMovementAnim() {

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
