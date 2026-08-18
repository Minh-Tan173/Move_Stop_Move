using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterBase : PoolUnit
{
    [Header("Visual")]
    [SerializeField] protected ICharacterAnimator charAnimator;
    [SerializeField] protected CharacterVisual charVisual;

    [Header("Attack Behavior")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected Transform shootingPoint;

    [Header("Ref")]
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CapsuleCollider capsuleCollider;

    [Header("TEST -- REMOVE AFTER")]
    [SerializeField] protected WeaponType currentWeaponType; // TẠM THỜI

    #region Attack Behavior
    protected float elapsedAttackDuration;
    protected float elapsedAttackCD;
    protected bool isInAttackDuration;
    protected CharacterBase attackTarget;
    #endregion

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


        characterStats.OnInit();
        charAnimator.ResetAnim();
    }

    public virtual void OnGamePlaying() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual void OnDespawn() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
    }

    public virtual void SetAttackTarget(CharacterBase target) {
        attackTarget = target;
    }

    public void Dead() {

        OnDespawn();
        charAnimator.TriggerDeadAnim();
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

    public bool IsTargetAvailable(CharacterBase target) {
        // if target is null, dead or hide
        return target != null && target != this && !target.IsDead() && target.gameObject.activeSelf;
    }

    public virtual bool CanSelectAttackTarget(CharacterBase target) {
        return IsTargetAvailable(target);
    }

    public bool IsAttackTargetValid() {

        // Is target not still availble
        if (!IsTargetAvailable(attackTarget)) { return false; }

        // Is target not still in attack range
        float sqrDistanceToTarget = (attackTarget.UnitTF.position - this.UnitTF.position).sqrMagnitude;
        float sqrTrueAttackRange = GetTrueAttackRange() * GetTrueAttackRange();

        return sqrDistanceToTarget <= sqrTrueAttackRange;
    }

    public bool CanScanTarget() {
        return IsMoving() && !IsAttackTargetValid();
    }

    public bool IsDead() {
        return isDead;
    }

    public CharacterStats GetCharacterStats() {
        return characterStats;
    }

    public void UpdateBodySize(float newSize) {

        charVisual.UpdateSize(newSize);

        capsuleCollider.height = defaultColliderHeight * newSize;
        capsuleCollider.radius = defaultColliderRadius * newSize;
        capsuleCollider.center = defaultColliderCenter * newSize;
    }
}
