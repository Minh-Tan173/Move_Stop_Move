using UnityEngine;

public class ICharacterAnimator : MonoBehaviour  
{

    public virtual void ResetAnim() { }

    public virtual void HandleMovementAnim() { }

    public virtual void TriggerAttackAnim() { }

    public virtual void TriggerDeadAnim() { }
}
