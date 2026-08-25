using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum UIAxis {
    X,
    Y
}

public enum UIAnimationType {

    EaseInOut,
    FadeInOut
}

public class UIElementAnim : MonoBehaviour
{
    [Header("Animation Type")]
    [SerializeField] private UIAnimationType animationType;

    [Header("Ref")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rect;

    [Header("Animation Settings")]
    [SerializeField] private float appearDuration = 0.35f;
    [SerializeField] private float hideDuration = 0.25f;

    [Header("Move Settings")]
    [SerializeField] private UIAxis axis;
    [SerializeField] private float startPos;
    [SerializeField] private float endPos;

    private Coroutine currentCoroutine;
    private bool isShow = false;

    private Button button;
    private Button Button => button == null ? button = GetComponent<Button>() : button;

    private void SetActive(bool active) {

        gameObject.SetActive(active);
    }

    private void ActiveAnimation(UIAnimationType uiTransition) {

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }


        switch (uiTransition) {

            case UIAnimationType.EaseInOut:

                float start = isShow ? startPos : endPos;
                float end = isShow ? endPos : startPos;

                currentCoroutine = StartCoroutine(EaseAnimation(start, end));

                break;


            case UIAnimationType.FadeInOut:

                currentCoroutine = StartCoroutine(FadeAnimation());

                break;
        }
    }

    private IEnumerator FadeAnimation() {
 
        if (canvasGroup == null) {
            Debug.LogError($"This {gameObject.name} is missing canvas group");
            yield break;
        }

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float startAlpha = isShow ? 0f : 1f;
        float endAlpha = isShow ? 1f : 0f;

        canvasGroup.alpha = startAlpha;

        float elapsed = 0f;
        float duration = isShow ? appearDuration : hideDuration;


        while (elapsed <= duration) {

            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float easeT = AnimationEase.EaseInOut(t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easeT);

            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;

        SetActive(isShow);
    }


    private IEnumerator EaseAnimation(float start, float end) {

        if (Button != null) {
            // This element is button
            button.interactable = false;
        }

        Vector2 startVector = GetPosition(start);
        Vector2 endVector = GetPosition(end);

        rect.anchoredPosition = startVector;

        float elapsed = 0f;
        float duration = isShow ? appearDuration : hideDuration;


        while (elapsed <= duration) {

            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float easeT = isShow ? AnimationEase.EaseOutBack(t) : AnimationEase.EaseInBack(t);

            rect.anchoredPosition = Vector2.LerpUnclamped(startVector, endVector, easeT);

            yield return null;
        }

        rect.anchoredPosition = endVector;

        if (Button != null) {
            // This element is button
            button.interactable = true;
        }

        SetActive(isShow);
    }


    private Vector2 GetPosition(float value) {

        Vector2 pos = rect.anchoredPosition;

        if (axis == UIAxis.X) {

            pos.x = value;
        }
        else if (axis == UIAxis.Y) {

            pos.y = value;
        }

        return pos;
    }

    public void ShowElement() {

        SetActive(true);

        isShow = true;

        ActiveAnimation(animationType);
    }


    public void HideElement() {
        isShow = false;

        ActiveAnimation(animationType);
    }

}
