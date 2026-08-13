using UnityEngine;
using UnityEngine.UIElements;

public static class AttackState
{

    public static void OnEnter(Bot bot, CharacterAnimatorBase botAnimator) {

        bot.ResetAttackCD();
        bot.LookAttackTarget();
    }

    public static void OnExcute(Bot bot, CharacterAnimatorBase botAnimatort) {

        if (!LevelManager.Instance.IsGamePlaying()) {

            bot.ChangeBotStateTo(BotStates.Idle);
            return;
        }

        if (bot.IsAttackTargetValid()) {

            bot.UpdateAttackCD(Time.deltaTime);

            if (bot.IsOverAttackCD()) {

                bot.ResetAttackCD();
                bot.Attack();
            }

        }
        else {
            bot.ChangeBotStateTo(BotStates.Patrol);
        }
    }

    public static void OnExit(Bot bot, CharacterAnimatorBase botAnimator) {

    }
}
