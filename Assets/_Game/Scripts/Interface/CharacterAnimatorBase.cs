using UnityEngine;

public class CharacterAnimatorBase : MonoBehaviour  
{
    protected const string IS_MOVING = "IsMoving";
    protected const string IDLE_ANIM = "TriggerIdle";
    protected const string DEAD_ANIM = "TriggerDead";
    protected const string ATTACK_ANIM = "TriggerAttack";

    public virtual void ResetAnim() { }

    public virtual void HandleIdleAnim() { }

    public virtual void TriggerAttackAnim() { }

    public virtual void TriggerDeadAnim() { }
}
