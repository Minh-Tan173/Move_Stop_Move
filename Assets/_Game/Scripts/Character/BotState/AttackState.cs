using UnityEngine;
using UnityEngine.UIElements;

public static class AttackState
{

    public static void OnEnter(Bot bot, CharacterAnimator botAnimator) {

        bot.RollMaxAttackCount();

        bot.GetCharacterCombat().StartAttack();

        bot.ChangeBotSMTo(BotSM.attack);
    }

    public static void OnExecute(Bot bot, CharacterAnimator botAnimatort) {

        CharacterCombat botCombat = bot.GetCharacterCombat();

        //if (!LevelManager.Instance.IsGamePlaying()) {

        //    bot.ChangeBotStateTo(BotStateSet.Idle);
        //    return;
        //}

        if (CharacterManager.Instance.IsLastAliveCharacter(bot)) {

            bot.ChangeBotStateTo(BotStateSet.Win);
            return;
        }

        if (!botCombat.IsAttackTargetValid()) {
            // If attack target not valid anymore

            bot.ChangeBotStateTo(BotStateSet.Patrol);

        }
        else {
            // If attack target valid

            if (botCombat.IsAttacking()) {
                // Wait until attack animation completed

            }
            else {

                botCombat.UpdateAttackCD(Time.deltaTime);

                if (botCombat.IsOverAttackCD()) {

                    botCombat.StartAttack();
                }
            }
        }
    }

    public static void OnExit(Bot bot, CharacterAnimator botAnimator) {

        bot.GetCharacterCombat().InterruptAttack();
        bot.SetAttackTarget(null);
    }
}
