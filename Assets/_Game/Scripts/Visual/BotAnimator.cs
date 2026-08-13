using UnityEngine;

public class BotAnimator : CharacterAnimatorBase {

    [SerializeField] private Bot bot;
    [SerializeField] private Animator animator;

    private void Update() {
        
        
    }

    public override void HandleIdleAnim() {

    }

    public override void ResetAnim() {
    }

    public override void TriggerAttackAnim() {
    }

    public override void TriggerDeadAnim() {
    }
}
