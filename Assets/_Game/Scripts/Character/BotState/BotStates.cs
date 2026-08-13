using System;

public static class BotStates
{
    public static readonly BotState Idle = new BotState(IdleState.OnEnter, IdleState.OnExcute, IdleState.OnExit);

    public static readonly BotState Patrol = new BotState(PatrolState.OnEnter, PatrolState.OnExcute, PatrolState.OnExit);
    
    public static readonly BotState Attack = new BotState(AttackState.OnEnter, AttackState.OnExcute, AttackState.OnExit);
    
}

public class BotState {

    public readonly Action<Bot, CharacterAnimatorBase> OnEnter;
    public readonly Action<Bot, CharacterAnimatorBase> OnExcute;
    public readonly Action<Bot, CharacterAnimatorBase> OnExit;

    public BotState(Action<Bot, CharacterAnimatorBase> onEnter, Action<Bot, CharacterAnimatorBase> onExecute, Action<Bot, CharacterAnimatorBase> onExit) {

        OnEnter = onEnter;
        OnExcute = onExecute;
        OnExit = onExit;
    }
}
