using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum FindDestinationType {

    NearestCharacter = 0,
    RandomCharacter = 1,
    //RandomPosition // Tạm thời chưa dùng
}

public static class PatrolState 
{
    private static CharacterBase GetNearestCharacter(Bot bot) {

        List<CharacterBase> charList = new List<CharacterBase>(CharacterManager.Instance.GetActiveCharacterList());
        if (charList.Count <= 1) { return null; }

        CharacterBase nearestCharacter = null;
        float nearestDistance = Mathf.Infinity;

        foreach (CharacterBase character in charList) {

            if (!bot.CanSelectTarget(character)) { continue; }

            float sqrDistance = (character.UnitTF.position - bot.UnitTF.position).sqrMagnitude;

            if (sqrDistance < nearestDistance) {

                nearestDistance = sqrDistance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    private static CharacterBase GetRandomCharacter(Bot bot) {

        List<CharacterBase> charList = new List<CharacterBase>(CharacterManager.Instance.GetActiveCharacterList());

        if (charList.Count <= 1) return null;

        int botIndex = charList.IndexOf(bot);

        int randomIndex = Random.Range(0, charList.Count - 1);

        if (randomIndex >= botIndex) {
            randomIndex += 1;
        }

        return charList[randomIndex];
    }

    private static Transform GetTargetByType(FindDestinationType findType, Bot bot) {

        switch (findType) {
            case FindDestinationType.NearestCharacter: return GetNearestCharacter(bot).UnitTF;
            case FindDestinationType.RandomCharacter: return GetRandomCharacter(bot).UnitTF;
        }

        return null;
    }

    public static void OnEnter(Bot bot, CharacterAnimator botAnimator) {

        bot.RollFindType();

        bot.Run();

        bot.ChangeBotSMTo(BotSM.patrol);
        
    }

    public static void OnExecute(Bot bot, CharacterAnimator botAnimator) {

        //if (!LevelManager.Instance.IsGamePlaying()) {

        //    bot.ChangeBotStateTo(BotStateSet.Idle);
        //    return;
        //}

        if (CharacterManager.Instance.IsLastAliveCharacter(bot)) {

            bot.ChangeBotStateTo(BotStateSet.Win);
            return;
        }

        if (bot.HasMoveTarget()) {
            // On moving to moveTarget
          
            if (bot.GetCharacterCombat().IsAttackTargetValid()) {
                // Is target is valid

                bot.ChangeBotStateTo(BotStateSet.Attack);
                return;
            }

            Vector3 moveTarget = bot.GetMoveTarget();

            float sqrDistanceToMoveTarget = (moveTarget - bot.UnitTF.position).sqrMagnitude;

            if (sqrDistanceToMoveTarget <= 0.2f * 0.2f) {
                // Reached move target

                if (bot.GetCharacterCombat().HasAttackTarget()) {
                    // Last check

                    bot.ChangeBotStateTo(BotStateSet.Attack);
                    return;
                }
                else {
                    // Still none attack target

                    bot.ChangeBotStateTo(BotStateSet.Idle);
                    return;
                }

            }

        }
        else {
            // If dont having move target

            bot.RollFindType();

            Transform newMoveTarget = GetTargetByType(bot.GetFindType(), bot);
            bot.MoveToDestination(newMoveTarget.position);
        }
    }

    public static void OnExit(Bot bot, CharacterAnimator botAnimator) {

        bot.StopMovement();
    }
}
