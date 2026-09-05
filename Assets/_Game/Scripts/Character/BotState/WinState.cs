using UnityEngine;

public static class WinState
{
    public static void OnEnter(Bot bot, CharacterAnimator animator) {

        bot.StopMovement();
        bot.Win();
    }

    public static void OnExcute(Bot bot, CharacterAnimator animator) {

    }

    public static void OnExit(Bot bot, CharacterAnimator animator) {

    }
}
