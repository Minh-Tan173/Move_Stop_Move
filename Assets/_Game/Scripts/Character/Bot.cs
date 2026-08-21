using System;
using UnityEngine;
using UnityEngine.AI;


// TEST
public enum BotSM {
    idle,
    patrol,
    attack
}
public class Bot : CharacterBase
{

    [Header("Movement")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;

    private BotState currentState;

    #region Navmesh Setup
    private float defaultAgentRadius;
    private bool cachedAgentSize;
    #endregion

    #region Idle Behavior
    private float elapsedIdleDuration;
    #endregion

    #region Patrol Behavior
    private int indexFindType;
    private Vector3 moveTarget;
    #endregion

    #region Attack Behavior
    private int attackCount;
    private int maxAttackCount;
    private CharacterBase ignoredAttackTarget;
    #endregion

    // TEST
    private BotSM currentSM;

    public override void OnInit() {

        base.OnInit();

        ActiveNavMesh();

        moveTarget = Vector3.zero;
        attackTarget = null;

        isDead = false;

        elapsedIdleDuration = LevelManager.Instance.IsGamePlaying() ? 0f : idleDuration;
        ChangeBotStateTo(BotStateSet.Idle);

        // Weapon Prepared
        charVisual.ChangeWeapon(currentWeaponType);

        // Visual
        PantItemData pant = charVisual.ChangePants();
        if (pant != null) { pant.ApplyBoosterFor(this); }

        HatItemData hat = charVisual.ChangeHats();
        if (hat != null) { hat.ApplyBoosterFor(this); }

        charVisual.ChangeAccessories();
    }

    public override void OnDespawn() {

        isDead = true;

        currentState?.OnExit(this, charAnimator);
        currentState = null;

        charAnimator.ResetAnim();
        charVisual.OnDespawn();

        DeactiveNavMesh();
    }

    private void Update() {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }

        if (currentState != null) {

            currentState?.OnExcute(this, charAnimator);
        }
    }


    private void CacheAgentSize() {

        if (cachedAgentSize) return;

        defaultAgentRadius = navMeshAgent.radius;
        cachedAgentSize = true;
    }

    public override void UpdateBodySize(float newSize) {

        base.UpdateBodySize(newSize);

        CacheAgentSize();

        navMeshAgent.radius = defaultAgentRadius * newSize;
    }

    public void UpdateNavMeshSpeed() {

        navMeshAgent.speed = GetCharacterStats().GetMoveSpeed();
    }
    

    private void DeactiveNavMesh() {

        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh) {

            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
        }

        navMeshAgent.enabled = false;
    }

    private void ActiveNavMesh() {

        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;

    }

    public void ChangeBotStateTo(BotState newState) {

        if (this.IsDead()) { return;}

        currentState?.OnExit(this, charAnimator);

        currentState = newState;

        currentState?.OnEnter(this, charAnimator);
    }

    public void RollFindType() {


        if (CharacterManager.Instance.GetActiveCharacterList().Count <= 3) {
            // On field has too few characters --> Default moveTarget is nearest, not other type

            indexFindType = 0;
            return;
        }

        int totalFindType = Enum.GetValues(typeof(FindDestinationType)).Length;
        indexFindType = UnityEngine.Random.Range(0, totalFindType);
    }

    public void RollMaxAttackCount() {

        attackCount = 0;
        maxAttackCount = UnityEngine.Random.Range(2, 4);
    }

    public FindDestinationType GetFindType() {
        return (FindDestinationType)indexFindType;
    }

    public void MoveToDestination(Vector3 moveTarget) {

        this.moveTarget = moveTarget;

        navMeshAgent.stoppingDistance = 0f;

        navMeshAgent.SetDestination(moveTarget);
    }

    public void StopMovement() {

        moveTarget = Vector3.zero;
        navMeshAgent.ResetPath();
        navMeshAgent.velocity = Vector3.zero;
    }

    public bool HasMoveTarget() {
        return moveTarget != Vector3.zero;
    }

    public bool HasAttackTarget() {
        return attackTarget != null;
    }

    public Vector3 GetMoveTarget() {
        return this.moveTarget;
    }

    public void LookAttackTarget() {

        if (attackTarget == null) { return;}

        Vector3 direction = attackTarget.UnitTF.position - UnitTF.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.2f * 0.2f) { return; }

        UnitTF.rotation = Quaternion.LookRotation(direction);
    }

    public override bool IsMoving() {

        if (!navMeshAgent.enabled || !navMeshAgent.isOnNavMesh) {
            return false;
        }

        return navMeshAgent.velocity.sqrMagnitude > 0.01f;
    }

    public override bool CanSelectTarget(CharacterBase target) {

        return base.CanSelectTarget(target) && target != ignoredAttackTarget;
    }

    public override void SetAttackTarget(CharacterBase target) {

        attackTarget = target;

        if (target != null) {

            if (target != ignoredAttackTarget) {
                ignoredAttackTarget = null;
            }
        }
    }

    public void IncreaseAttackCount() {
        attackCount += 1;
    }

    public bool IsOverMaxAttackCount() {
        return attackCount >= maxAttackCount;
    }

    public void IgnoreCurrentAttackTarget() {

        ignoredAttackTarget = attackTarget;
        attackTarget = null;
    }

    public void UpdateElapsedIdleDuration(float time) {
        elapsedIdleDuration += Time.deltaTime;
    }

    public void ResetElapsedIdleDuration() {
        elapsedIdleDuration = 0f;
    }

    public bool IsOverIdleDuration() {
        return elapsedIdleDuration >= idleDuration;
    }

    // TEST ONLY
    public void ChangeBotSMTo(BotSM botSM) {

        currentSM = botSM;
    }
}
