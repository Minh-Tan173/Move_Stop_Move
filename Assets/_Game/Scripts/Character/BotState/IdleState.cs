using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot, CharacterAnimatorBase botAnimator) {

        bot.StopMovement();
    }

    public static void OnExcute(Bot bot, CharacterAnimatorBase botAnimator) {

        //botAnimator.HandleIdleAnim();
    }

    public static void OnExit(Bot bot, CharacterAnimatorBase botAnimator) {

    }
}
