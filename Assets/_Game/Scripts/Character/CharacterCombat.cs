using System;
using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public event EventHandler OnAttackCompleted;

    [Header("Attack Behavior")]
    [SerializeField] private bool canScanWhileMoving;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform shootingPoint;

    [Header("TEST -- REMOVE AFTER")]
    [SerializeField] private WeaponType currentWeaponType;

    private CharacterBase character;
    private CharacterAnimator charAnimator;

    private CharacterBase attackTarget;

    private float elapsedAttackCD;
    private bool isAttacking;

    private IEnumerator IELookToAttackTarget(Quaternion targetRot) {

        Quaternion startRot = character.UnitTF.rotation;
        float elapsed = 0f;
        float duration = 0.1f;

        while (elapsed <= duration) {

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            character.UnitTF.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        character.UnitTF.rotation = targetRot;
    }

    private void LookAttackTarget() {

        if (attackTarget == null) { return; }

        Vector3 direction = attackTarget.UnitTF.position - character.UnitTF.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.2f * 0.2f) { return; } // If attack target too near character --> dont need rotate

        StartCoroutine(IELookToAttackTarget(Quaternion.LookRotation(direction)));
    }

    public void OnInit(CharacterBase character, CharacterAnimator charAnimator) {

        this.character = character;
        this.charAnimator = charAnimator;

        attackTarget = null;

        elapsedAttackCD = 0f;
        isAttacking = false;

        // Weapon Prepared
        character.GetCharacterVisual().ChangeWeapon(currentWeaponType);
    }

    public void StartAttack() {

        LookAttackTarget();

        isAttacking = true;
        //hasCompletedAttack = false;

        ResetAttackCD();

        charAnimator.TriggerAttackAnim();
    }

    public void Throw() {

        if (!IsAttacking()) { return; } // If attack behavior was interrupt before throw bullet

        BulletBase bulletPrefab = weaponSO.GetBulletPrefab(currentWeaponType);
        BulletBase bullet = SimplePool.Spawn<BulletBase>(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.ActiveThrow(character);
    }

    public void CompleteAttack() {

        if (!isAttacking) return;

        isAttacking = false;
        ResetAttackCD();

        OnAttackCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void InterruptAttack() {

        isAttacking = false;
        //hasCompletedAttack = false;

        ResetAttackCD();
    }


    public bool IsAttacking() {
        // if Character is doing Attack Behavior
        return isAttacking;
    }


    public void UpdateAttackCD(float time) {
        elapsedAttackCD += time;
    }

    public bool IsOverAttackCD() {
        return elapsedAttackCD >= character.GetCharacterStats().GetAttackCD();
    }

    public void ResetAttackCD() {
        elapsedAttackCD = 0f;
    }

    public void SetAttackTarget(CharacterBase target) {

        attackTarget = target;
    }

    public CharacterBase GetAttackTarget() {
        return attackTarget;
    }

    public float GetTrueAttackRange() {
        return character.GetCharacterStats().GetAttackRange();
    }

    public bool HasAttackTarget() {
        return attackTarget != null;
    }

    public bool IsTargetAvailable(CharacterBase target) {
        // if target is null, dead or hide
        return target != null && target != character && !target.IsDead() && target.gameObject.activeSelf;
    }

    public bool IsAttackTargetValid() {

        // Is target not still availble
        if (!IsTargetAvailable(attackTarget)) { return false; }

        // Is target not still in attack range
        float sqrDistanceToTarget = (attackTarget.UnitTF.position - character.UnitTF.position).sqrMagnitude;
        float sqrTrueAttackRange = GetTrueAttackRange() * GetTrueAttackRange();

        return sqrDistanceToTarget <= sqrTrueAttackRange;
    }

    public bool CanScanTarget() {

        if (canScanWhileMoving) {
            return character.IsMoving() && !IsAttackTargetValid();
        }

        return !character.IsMoving() && !IsAttackTargetValid();
    }
}
