using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<PowerUpType, PowerUpBase> appliedPowerupDict = new Dictionary<PowerUpType, PowerUpBase>();

    private void Awake() {

        defaultColliderHeight = capsuleCollider.height;
        defaultColliderRadius = capsuleCollider.radius;
        defaultColliderCenter = capsuleCollider.center;
    }

    public virtual void OnInit() {

        appliedPowerupDict.Clear();

        immortalEndTime = 0f;

        characterCombat.OnInit(this, charAnimator);
        characterStats.OnInit(this);
        charVisual.OnInit();
        charAnimator.ResetAnim();

        attackRangeVisual.OnInit();

        isDead = false;
    }

    public virtual void OnGamePlaying() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual void OnDespawn() {
        isDead = true;
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

        charVisual.PlayBlood();

        OnDespawn();
        charAnimator.TriggerDeadAnim();
    }

    public bool IsDead() {
        return isDead;
    }

    #endregion

    public void RegisterPowerUp(PowerUpType powerUpType, PowerUpBase powerUp) {

        appliedPowerupDict[powerUpType] = powerUp;
    }

    public void UnregisterPowerUp(PowerUpType powerUpType) {

        appliedPowerupDict.Remove(powerUpType);
    }

    public bool IsHavingThisPowerUp(PowerUpType powerUpType) {

        return appliedPowerupDict.ContainsKey(powerUpType);
    }

    public PowerUpBase GetPowerUp(PowerUpType powerUpType) {

        if (!appliedPowerupDict.ContainsKey(powerUpType)) {
            return null;
        }

        return appliedPowerupDict[powerUpType];
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

    public AttackRangeVisual GetAttackRangeVisual() {
        return this.attackRangeVisual;
    }

    public CanvasCharacter GetCanvasCharacter() {
        return this.canvasCharacter;
    }
}
