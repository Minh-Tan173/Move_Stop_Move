using System;
using UnityEngine;
using UnityEngine.AI;

public class Bot : CharacterBase
{

    [Header("Movement")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    private BotState currentState;
    private bool isGamePlaying;

    #region Patrol Behavior
    private int indexFindType;
    private Vector3 moveTarget;
    #endregion

    public override void OnInit() {

        ActiveNavMesh();

        moveTarget = Vector3.zero;
        attackTarget = null;

        isGamePlaying = false;

        ChangeBotStateTo(BotStates.Idle);
    }

    public override void OnGamePlaying() {

        Debug.Log("Switch to Patrol");

        ChangeBotStateTo(BotStates.Patrol);
    }

    public override void OnDespawn() {
        DeactiveNavMesh();
    }

    private void Update() {

        if (!isGamePlaying && LevelManager.Instance.IsGamePlaying()) {
            isGamePlaying = true;

            OnGamePlaying();
        }
        
        if (currentState != null) {

            currentState?.OnExcute(this, charAnimator);
        }
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

    public FindDestinationType GetFindType() {
        return (FindDestinationType)indexFindType;
    }

    public void MoveToDestination(Vector3 moveTarget) {

        this.moveTarget = moveTarget;
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

        if (attackTarget == null) return;

        Vector3 direction = attackTarget.UnitTF.position - UnitTF.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f) return;

        UnitTF.rotation = Quaternion.LookRotation(direction);
    }

    public override bool IsMoving() {
        return currentState == BotStates.Patrol;
    }
}
