using UnityEngine;

public class CharacterAnimatorBase : MonoBehaviour  
{

    public virtual void ResetAnim() { }

    public virtual void HandleIdleAnim() { }

    public virtual void TriggerAttackAnim() { }

    public virtual void TriggerDeadAnim() { }
}
