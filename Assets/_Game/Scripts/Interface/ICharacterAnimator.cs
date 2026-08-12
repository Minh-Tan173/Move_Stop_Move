using UnityEngine;

public interface ICharacterAnimator
{
    protected const string IS_MOVING = "IsMoving";
    protected const string IDLE_ANIM = "TriggerIdle";
    protected const string DEAD_ANIM = "TriggerDead";
    protected const string ATTACK_ANIM = "TriggerAttack";

    public void ResetAnim();

    public void HandleIdleAnim();

    public void TriggerAttackAnim();

    public void TriggerDeadAnim();
}
