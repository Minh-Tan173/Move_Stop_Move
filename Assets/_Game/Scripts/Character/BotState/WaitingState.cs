using UnityEngine;

public static class WaitingState
{
    public static void OnEnter(Bot bot, ICharacterAnimator botAnimator) {

        bot.StopMovement();
    }

    public static void OnExcute(Bot bot, ICharacterAnimator botAnimator) {

        bot.ChangeBotStateTo(BotStateSet.Patrol);
    }

    public static void OnExit(Bot bot, ICharacterAnimator botAnimator) {

    }
}
