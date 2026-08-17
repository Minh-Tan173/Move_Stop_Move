using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot, ICharacterAnimator botAnimator) {

        bot.StopMovement();
    }

    public static void OnExcute(Bot bot, ICharacterAnimator botAnimator) {

        if (bot.HasAttackTarget()) {

            bot.ChangeBotStateTo(BotStateSet.Attack);
            return;
        }

        bot.UpdateElapsedIdleDuration(Time.deltaTime);

        if (bot.IsOverIdleDuration()) {

            bot.ChangeBotStateTo(BotStateSet.Patrol);
            return;
        }
        
    }

    public static void OnExit(Bot bot, ICharacterAnimator botAnimator) {
        bot.ResetElapsedIdleDuration();
    }
}
