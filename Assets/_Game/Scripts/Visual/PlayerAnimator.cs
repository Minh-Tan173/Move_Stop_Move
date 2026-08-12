using UnityEngine;

public class PlayerAnimator : MonoBehaviour, ICharacterAnimator
{
    [Header("Parent")]
    [SerializeField] private Player player;

    [Header("Ref component")]
    [SerializeField] private Animator animator;


    private void Update() {

        HandleIdleAnim();
    }

    public void ResetAnim() {


    }

    public void HandleIdleAnim() {

        animator.SetBool(ICharacterAnimator.IS_MOVING, player.IsMoving());
    }

    public void TriggerAttackAnim() {
        animator.SetTrigger(ICharacterAnimator.ATTACK_ANIM);
    }

    public void TriggerDeadAnim() {

        animator.SetTrigger(ICharacterAnimator.DEAD_ANIM);
    }
}
