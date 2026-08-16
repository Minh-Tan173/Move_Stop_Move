using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot, CharacterAnimatorBase botAnimator) {

        bot.StopMovement();
    }

    public static void OnExcute(Bot bot, CharacterAnimatorBase botAnimator) {

        if (bot.HasAttackTarget()) {
            bot.ChangeBotStateTo(BotStateSet.Attack);
        }

        bot.UpdateElapsedIdleDuration(Time.deltaTime);

        if (bot.IsOverIdleDuration()) {
            bot.ChangeBotStateTo(BotStateSet.Patrol);
        }
        
    }

    public static void OnExit(Bot bot, CharacterAnimatorBase botAnimator) {
        bot.ResetElapsedIdleDuration();
    }
}
