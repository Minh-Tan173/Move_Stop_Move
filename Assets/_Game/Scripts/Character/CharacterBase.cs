using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBase : PoolUnit
{
    [Header("Visual")]
    [SerializeField] protected CharacterAnimatorBase charAnimator;
    [SerializeField] protected CharacterVisual charVisual;
    [SerializeField] protected float attackRange;

    [Header("Attack Behavior")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected float attackCD;
    [SerializeField] protected Transform shootingPoint;

    [Header("TEST -- REMOVE AFTER")]
    [SerializeField] protected WeaponType currentWeaponType; // TẠM THỜI

    protected float elapsedAttackCD;

    protected CharacterBase attackTarget;

    protected bool isDead;

    public virtual void OnInit() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
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

    #region Skin Method
    public void ChangeWeapon() {

    }

    public void ChangePants() {

    }
    #endregion

    public void Attack() {

        charAnimator.TriggerAttackAnim();
        Throw();
    }

    public void Throw() {

        BulletBase bulletPrefab = weaponSO.GetBulletPrefab(currentWeaponType);
        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        bullet.ActiveMovement(this);
    }

    public void OnDead() {

        charAnimator.TriggerDeadAnim();
        OnDespawn();
    }

    public virtual bool IsMoving() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
        return true;
    }

    public void ResetAttackCD() {
        elapsedAttackCD = 0f;
    }

    public void UpdateAttackCD(float time) {
        elapsedAttackCD += time;
    }

    public bool IsOverAttackCD() {
        return elapsedAttackCD >= attackCD; 
    }

    public float GetTrueAttackRange(float multiply = 1f) {
        return this.attackRange * multiply;
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
