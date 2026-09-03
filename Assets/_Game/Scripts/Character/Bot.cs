using System;
using System.Collections.Generic;
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

    [Header("Weapon Info")]
    [SerializeField] private WeaponSO weaponSO;
 
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

        characterCombat.OnAttackCompleted += Bot_OnAttackCompleted;

        ActiveNavMesh();

        moveTarget = Vector3.zero;

        elapsedIdleDuration = LevelManager.Instance.IsGamePlaying() ? 0f : idleDuration;
        ChangeBotStateTo(BotStateSet.Idle);

        // WeaponPrepared
        WeaponType randomWeapon = (WeaponType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponType)).Length);

        WeaponItemData weaponData =weaponSO.GetWeaponItemData(randomWeapon);
        List<WeaponSkinData> skins =weaponData.GetSkinDataList();

        WeaponSkinData randomSkin = skins[UnityEngine.Random.Range(0, skins.Count)];


        characterCombat.SetWeaponType(randomWeapon, randomSkin.GetItemID());

        // Item Prepared
        PantItemData pant = charVisual.ChangePants(this);
        if (pant != null) { pant.ApplyBoosterFor(this); }

        HatItemData hat = charVisual.ChangeHats(this);
        if (hat != null) { hat.ApplyBoosterFor(this); }

        charVisual.ChangeAccessories(this);
    }

    public override void OnDespawn() {

        base.OnDespawn();

        characterCombat.OnAttackCompleted -= Bot_OnAttackCompleted;

        currentState?.OnExit(this, charAnimator);
        currentState = null;

        charAnimator.ResetAnim();
        charVisual.OnDespawn();

        DeactiveNavMesh();
    }

    private void Bot_OnAttackCompleted(object sender, EventArgs e) {

        IncreaseAttackCount();

        if (IsOverMaxAttackCount()) {

            IgnoreCurrentAttackTarget();
            ChangeBotStateTo(BotStateSet.Patrol);
        }
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

    public Vector3 GetMoveTarget() {
        return this.moveTarget;
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

        base.SetAttackTarget(target);

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

        ignoredAttackTarget = characterCombat.GetAttackTarget();
        SetAttackTarget(null);
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
