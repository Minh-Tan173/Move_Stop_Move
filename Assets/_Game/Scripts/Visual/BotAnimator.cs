using UnityEngine;

public class BotAnimator : MonoBehaviour, ICharacterAnimator {

    [SerializeField] private Bot bot;
    [SerializeField] private Animator animator;

    private void Update() {
        
        
    }

    public void HandleIdleAnim() {
    }

    public void ResetAnim() {
    }

    public void TriggerAttackAnim() {
    }

    public void TriggerDeadAnim() {
    }
}
