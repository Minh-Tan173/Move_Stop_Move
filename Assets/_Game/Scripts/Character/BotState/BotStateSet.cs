using System;

public static class BotStateSet
{
    public static readonly BotState Idle = new BotState(IdleState.OnEnter, IdleState.OnExcute, IdleState.OnExit);

    public static readonly BotState Patrol = new BotState(PatrolState.OnEnter, PatrolState.OnExcute, PatrolState.OnExit);
    
    public static readonly BotState Attack = new BotState(AttackState.OnEnter, AttackState.OnExcute, AttackState.OnExit);
    
}

public class BotState {

    public readonly Action<Bot, CharacterAnimator> OnEnter;
    public readonly Action<Bot, CharacterAnimator> OnExcute;
    public readonly Action<Bot, CharacterAnimator> OnExit;

    public BotState(Action<Bot, CharacterAnimator> onEnter, Action<Bot, CharacterAnimator> onExecute, Action<Bot, CharacterAnimator> onExit) {

        OnEnter = onEnter;
        OnExcute = onExecute;
        OnExit = onExit;
    }
}
