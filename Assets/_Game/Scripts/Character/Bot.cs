using System;
using UnityEngine;

public class Bot : CharacterBase
{
    private BotState currentState;

    #region Patrol Behavior
    private int indexFindType;
    private Vector3 moveTarget;
    #endregion

    public override void OnInit() {
        
    }

    public override void OnDespawn() {
        
    }

    private void Update() {
        
        if (currentState != null) {

            currentState?.OnExcute(this, charAnimator);
        }
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

    public void SetMoveTarget(Vector3 moveTarget) {
        this.moveTarget = moveTarget;
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


    public override bool IsMoving() {
        return currentState == BotStates.Patrol;
    }
}
