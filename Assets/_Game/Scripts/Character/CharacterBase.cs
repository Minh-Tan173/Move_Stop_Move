using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBase : PoolUnit
{
    [Header("Visual")]
    [SerializeField] protected CharacterAnimatorBase charAnimator;
    [SerializeField] protected CharacterVisual charVisual;

    [Header("Attack Behavior")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected Transform shootingPoint;

    [Header("Ref")]
    [SerializeField] protected CharacterStats characterStats;

    [Header("TEST -- REMOVE AFTER")]
    [SerializeField] protected WeaponType currentWeaponType; // TẠM THỜI

    #region Attack Behavior
    protected float elapsedAttackDuration;
    protected float elapsedAttackCD;
    protected bool isInAttackDuration;
    protected CharacterBase attackTarget;
    #endregion

    protected bool isDead;

    public virtual void OnInit() {

        characterStats.ResetAttackSize();
        charAnimator.ResetAnim();
    }

    public virtual void OnGamePlaying() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual void OnDespawn() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual bool CanSelectAttackTarget(CharacterBase target) {
        return target != this;
    }

    public virtual void SetAttackTarget(CharacterBase target) {
        attackTarget = target;
    }

    public void OnDead() {

        charAnimator.TriggerDeadAnim();
        OnDespawn();
    }

    public virtual bool IsMoving() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
        return true;
    }

    public void Attack() {

        isInAttackDuration = true;
        ResetAttackTimers();

        charAnimator.TriggerAttackAnim();
    }

    public void Throw() {

        BulletBase bulletPrefab = weaponSO.GetBulletPrefab(currentWeaponType);
        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.ActiveMovement(this);
    }

    public void UpdateAttackDuration(float time) {

        elapsedAttackDuration += time;
    }

    public bool IsOverAttackDuration() {

        return elapsedAttackDuration >= attackDuration;
    }

    public bool IsInAttackDuration() {

        return isInAttackDuration;
    }

    public void FinishAttack() {

        isInAttackDuration = false;
        ResetAttackTimers();

        Throw();
    }

    public void CancelAttack() {

        isInAttackDuration = false;
        ResetAttackTimers();
    }

    public void UpdateAttackCD(float time) {
        elapsedAttackCD += time;
    }

    public bool IsOverAttackCD() {
        return elapsedAttackCD >= characterStats.GetAttackCD(); 
    }

    public void ResetAttackTimers() {

        elapsedAttackDuration = 0f;
        elapsedAttackCD = 0f;
    }

    public float GetTrueAttackRange() {
        return this.characterStats.GetAttackRange();
    }

    public bool IsAttackTargetValid() {

        if (attackTarget == null) { return false; }

        // Is target not still alive
        if (!attackTarget.gameObject.activeSelf) { return false; }

        // Is target not still in attack range
        float sqrDistanceToTarget = (attackTarget.UnitTF.position - this.UnitTF.position).sqrMagnitude;
        float sqrTrueAttackRange = GetTrueAttackRange() * GetTrueAttackRange();
        if (sqrDistanceToTarget > sqrTrueAttackRange) { return false; }

        return true;
    }

    public bool CanScanTarget() {
        return IsMoving() && !IsAttackTargetValid();
    }

    public bool IsDead() {
        return isDead;
    }

    public Transform GetAttackTarget() {
        return attackTarget.UnitTF;
    }
}
