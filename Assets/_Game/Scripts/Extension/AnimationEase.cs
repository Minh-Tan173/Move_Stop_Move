using UnityEngine;

public static class AnimationEase
{
    private const float c1 = 1.70158f;
    private const float c3 = c1 + 1f;

    public static float EaseOutQuad(float t) {
        return t * (2f - t);
    }

    public static float EaseOutBack(float t) {
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public static float EaseInBack(float t) {
        return c3 * t * t * t - c1 * t * t;
    }

    public static float EaseInOut(float t) {
        return t * t * (3f - 2f * t);
    }

    public static float EaseOutCubic(float t) {

        return 1f - Mathf.Pow(1f - t, 3f);
    }

}
