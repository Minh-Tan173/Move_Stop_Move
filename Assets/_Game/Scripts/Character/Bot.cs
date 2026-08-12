using UnityEngine;

public enum BotState {
    Idle,
    Patrol,
    Attack
}

public class Bot : CharacterBase
{
    private BotState currentState;

    public override void OnInit() {
        
    }

    public override void OnDespawn() {
        
    }

    public void SetBotState(BotState botState) {
        currentState = botState;
    }

    public override bool IsMoving() {
        return currentState == BotState.Patrol;
    }
}
