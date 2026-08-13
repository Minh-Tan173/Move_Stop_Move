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

        List<CharacterBase> charList = CharacterManager.Instance.GetActiveCharacterList();

        CharacterBase nearestCharacter = null;
        float nearestDistance = Mathf.Infinity;

        foreach (CharacterBase character in charList) {

            if (character == bot) continue;

            float sqrDistance = (character.UnitTF.position - bot.UnitTF.position).sqrMagnitude;

            if (sqrDistance < nearestDistance) {

                nearestDistance = sqrDistance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    private static CharacterBase GetRandomCharacter(Bot bot) {

        List<CharacterBase> charList = CharacterManager.Instance.GetActiveCharacterList();

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

    public static void OnEnter(Bot bot, CharacterAnimatorBase botAnimator) {

        bot.RollFindType();

    }

    public static void OnExcute(Bot bot, CharacterAnimatorBase botAnimator) {

        if (!LevelManager.Instance.IsGamePlaying()) {

            bot.ChangeBotStateTo(BotStates.Idle);
            return;
        }

        if (bot.HasMoveTarget()) {
            // On moving to moveTarget
          
            if (bot.IsAttackTargetValid() && bot.CanAttackCurrentTarget()) {
                // 60% attack chance

                bot.ChangeBotStateTo(BotStates.Attack);
                return;
            }

            Vector3 moveTarget = bot.GetMoveTarget();

            float sqrDistanceToMoveTarget = (moveTarget - bot.UnitTF.position).sqrMagnitude;

            if (sqrDistanceToMoveTarget <= 0.2f * 0.2f) {
                // Reached move target

                if (bot.HasAttackTarget()) {
                    // Last check

                    bot.ChangeBotStateTo(BotStates.Attack);
                    return;
                }
                else {
                    // Still none attack target

                    bot.RollFindType();

                    Transform newMoveTarget = GetTargetByType(bot.GetFindType(), bot);
                    bot.MoveToDestination(newMoveTarget.position);
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

    public static void OnExit(Bot bot, CharacterAnimatorBase botAnimator) {

        bot.StopMovement();
    }
}
