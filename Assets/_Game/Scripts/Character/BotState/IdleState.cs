using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot, CharacterAnimator botAnimator) {

        bot.StopMovement();

        bot.Idle();

        bot.ChangeBotSMTo(BotSM.idle);
    }

    public static void OnExcute(Bot bot, CharacterAnimator botAnimator) {

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

    public static void OnExit(Bot bot, CharacterAnimator botAnimator) {
        bot.ResetElapsedIdleDuration();
    }
}
