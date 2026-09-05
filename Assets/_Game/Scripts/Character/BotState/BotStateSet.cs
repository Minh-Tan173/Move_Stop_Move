using System;

public static class BotStateSet
{
    public static readonly BotState Idle = new BotState(IdleState.OnEnter, IdleState.OnExecute, IdleState.OnExit);

    public static readonly BotState Patrol = new BotState(PatrolState.OnEnter, PatrolState.OnExecute, PatrolState.OnExit);
    
    public static readonly BotState Attack = new BotState(AttackState.OnEnter, AttackState.OnExecute, AttackState.OnExit);

    public static readonly BotState Win = new BotState(WinState.OnEnter, WinState.OnExcute, WinState.OnExit);
    
}

public class BotState {

    public readonly Action<Bot, CharacterAnimator> OnEnter;
    public readonly Action<Bot, CharacterAnimator> OnExecute;
    public readonly Action<Bot, CharacterAnimator> OnExit;

    public BotState(Action<Bot, CharacterAnimator> onEnter, Action<Bot, CharacterAnimator> onExecute, Action<Bot, CharacterAnimator> onExit) {

        OnEnter = onEnter;
        OnExecute = onExecute;
        OnExit = onExit;
    }
}
