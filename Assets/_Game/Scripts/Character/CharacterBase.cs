using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBase : PoolUnit
{
    [SerializeField] protected CharacterAnimatorBase charAnimator;
    [SerializeField] protected float attackRange;

    [Header("Attack Behavior")]
    [SerializeField] protected float attackCD;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected WeaponType weaponType; // TẠM THỜI

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

        BulletBase bullet = SimplePool.Spawn<BulletBase>(PoolType.Knife, shootingPoint.position, shootingPoint.rotation);
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
