using System.Collections;
using UnityEngine;

public class CharacterBase : PoolUnit
{
    [Header("Visual")]
    [SerializeField] protected CharacterAnimator charAnimator;
    [SerializeField] protected CharacterVisual charVisual;
    [SerializeField] protected AttackRangeVisual attackRangeVisual;
    [SerializeField] private CanvasCharacter canvasCharacter;

    [Header("Ref")]
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CharacterCombat characterCombat;
    [SerializeField] protected CapsuleCollider capsuleCollider;

    private float immortalEndTime;
    protected bool isDead;

    private float defaultColliderHeight;
    private float defaultColliderRadius;
    private Vector3 defaultColliderCenter;

    private void Awake() {

        defaultColliderHeight = capsuleCollider.height;
        defaultColliderRadius = capsuleCollider.radius;
        defaultColliderCenter = capsuleCollider.center;
    }

    public virtual void OnInit() {

        immortalEndTime = 0f;

        characterCombat.OnInit(this, charAnimator);
        characterStats.OnInit(this);
        charAnimator.ResetAnim();


        // Show/Hide Attack Range base on Type
        if (this is Player) {
            attackRangeVisual.Show();
        }
        else {
            attackRangeVisual.Hide();
        }
    }

    public virtual void OnGamePlaying() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual void OnDespawn() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual bool IsMoving() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
        return true;
    }

    public virtual void UpdateBodySize(float newSize) {

        charVisual.UpdateSize(newSize);

        capsuleCollider.height = defaultColliderHeight * newSize;
        capsuleCollider.radius = defaultColliderRadius * newSize;
        capsuleCollider.center = defaultColliderCenter * newSize;
    }

    public virtual void SetAttackTarget(CharacterBase target) {

        characterCombat.SetAttackTarget(target);
    }
    public virtual bool CanSelectTarget(CharacterBase target) {
        return characterCombat.IsTargetAvailable(target);
    }

    public void Idle() {

        charAnimator.TriggerIdleAnim();
    }

    public void Run() {

        charAnimator.TriggerRunAnim();
    }

    #region Life Control

    public void TriggerImmortal(float duration) {   

        immortalEndTime = Time.time + duration;
    }

    public bool IsImmortal() {

        return Time.time < immortalEndTime;
    }

    public void Dead() {

        OnDespawn();
        charAnimator.TriggerDeadAnim();
    }

    #endregion

    public bool IsDead() {
        return isDead;
    }

    public CharacterStats GetCharacterStats() {
        return characterStats;
    }

    public CharacterCombat GetCharacterCombat() {
        return characterCombat;
    }

    public CharacterVisual GetCharacterVisual() {
        return this.charVisual;
    }

    public CanvasCharacter GetCanvasCharacter() {
        return this.canvasCharacter;
    }
}
