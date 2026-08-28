using UnityEngine;

public class CharacterBase : PoolUnit
{
    [Header("Visual")]
    [SerializeField] protected CharacterAnimator charAnimator;
    [SerializeField] protected CharacterVisual charVisual;
    [SerializeField] private CanvasCharacter canvasCharacter;

    [Header("Attack Behavior")]
    [SerializeField] protected bool canScanWhileMoving;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected Transform shootingPoint;

    [Header("Ref")]
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CapsuleCollider capsuleCollider;

    [Header("TEST -- REMOVE AFTER")]
    [SerializeField] protected WeaponType currentWeaponType; // TẠM THỜI

    private float immortalEndTime;
    protected bool isDead;

    #region Attack Behavior
    protected float elapsedAttackDuration;
    protected float elapsedAttackCD;
    protected bool isInAttackDuration;
    protected CharacterBase attackTarget;
    #endregion

    #region Navmesh Setup
    private float defaultColliderHeight;
    private float defaultColliderRadius;
    private Vector3 defaultColliderCenter;
    #endregion

    private void Awake() {

        defaultColliderHeight = capsuleCollider.height;
        defaultColliderRadius = capsuleCollider.radius;
        defaultColliderCenter = capsuleCollider.center;
    }

    public virtual void OnInit() {

        immortalEndTime = 0f;

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

    public virtual bool CanSelectTarget(CharacterBase target) {
        return IsTargetAvailable(target);
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

    #region Combat
    public void Attack() {

        isInAttackDuration = true;
        ResetAttackTimers();

        charAnimator.TriggerAttackAnim();
    }

    public void Throw() {

        BulletBase bulletPrefab = weaponSO.GetBulletPrefab(currentWeaponType);
        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.ActiveThrow(this);
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

    public void LookAttackTarget() {

        if (attackTarget == null) { return; }

        Vector3 direction = attackTarget.UnitTF.position - UnitTF.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.2f * 0.2f) { return; }

        UnitTF.rotation = Quaternion.LookRotation(direction);
    }
    #endregion

    public bool IsTargetAvailable(CharacterBase target) {
        // if target is null, dead or hide
        return target != null && target != this && !target.IsDead() && target.gameObject.activeSelf;
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

        if (canScanWhileMoving) {

            return IsMoving() && !IsAttackTargetValid();
        }


        return !IsMoving() && !IsAttackTargetValid();
    }

    public bool IsDead() {
        return isDead;
    }

    public CharacterStats GetCharacterStats() {
        return characterStats;
    }

    public CanvasCharacter GetCanvasCharacter() {
        return this.canvasCharacter;
    }

    public CharacterVisual GetCharacterVisual() {
        return this.charVisual;
    }
}
