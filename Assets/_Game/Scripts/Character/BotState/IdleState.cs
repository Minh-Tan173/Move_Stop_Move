using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot, ICharacterAnimator botAnimator) {

        botAnimator.HandleIdleAnim();
    }

    public static void OnExcute(Bot bot, ICharacterAnimator botAnimator) {

    }

    public static void OnExit(Bot bot, ICharacterAnimator botAnimator) {

    }
}
