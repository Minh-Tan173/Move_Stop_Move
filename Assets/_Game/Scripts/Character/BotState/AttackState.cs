using UnityEngine;
using UnityEngine.UIElements;

public static class AttackState
{

    public static void OnEnter(Bot bot, ICharacterAnimator botAnimator) {

        //bot.ResetAttackCD();
        bot.RollMaxAttackCount();
        bot.LookAttackTarget();

        bot.Attack();

        bot.ChangeBotSMTo(BotSM.attack);
    }

    public static void OnExcute(Bot bot, ICharacterAnimator botAnimatort) {

        if (!LevelManager.Instance.IsGamePlaying()) {

            bot.ChangeBotStateTo(BotStateSet.Idle);
            return;
        }

        if (!bot.IsAttackTargetValid()) {

            bot.ChangeBotStateTo(BotStateSet.Patrol);
            return;
        }

        if (bot.IsInAttackDuration()) {
            // In Attack Duration

            bot.UpdateAttackDuration(Time.deltaTime);

            if (bot.IsOverAttackDuration()) {

                bot.FinishAttack();
                bot.IncreaseAttackCount();

                if (bot.IsOverMaxAttackCount()) {

                    bot.IgnoreCurrentAttackTarget();

                    bot.ChangeBotStateTo(BotStateSet.Patrol);
                    return;
                }
            }

            return;
        }


        // Wait CD between 2 attack behavior
        bot.UpdateAttackCD(Time.deltaTime);

        if (bot.IsOverAttackCD()) {

            bot.LookAttackTarget();
            bot.Attack();
        }
    }

    public static void OnExit(Bot bot, ICharacterAnimator botAnimator) {

        bot.CancelAttack();
        bot.SetAttackTarget(null);
    }
}
