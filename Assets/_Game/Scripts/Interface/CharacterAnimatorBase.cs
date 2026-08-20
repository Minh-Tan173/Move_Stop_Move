using UnityEngine;

public class ICharacterAnimator : MonoBehaviour  
{

    public virtual void ResetAnim() { }

    public virtual void TriggerIdleAnim() { }

    public virtual void TriggerRunAnim() { }

    public virtual void TriggerAttackAnim() { }

    public virtual void TriggerDeadAnim() { }
}
