using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics.Editor;
using UnityEngine;

public class CanvasHUD : UICanvas
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI aliveLeftText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Event Noti")]
    [SerializeField] private UIElementAnim eventNoti;

    [Header("Setting")]
    [SerializeField] private Setting setting;

    private const string READY_TEXT = "Ready!";

    private Coroutine currentCoroutine;

    private IEnumerator PlayCountdownTextAnimation(string text) {

        float targetScale = string.Equals(text, READY_TEXT) ? 1.25f : 1f;

        RectTransform textRect = countdownText.rectTransform;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * targetScale;

        float appearDuration = 0.3f;
        float holdDuration = 0.45f;
        float hideDuration = 0.2f;

        // Zoom out
        float elapsedTime = 0f;
        textRect.localScale = startScale;

        while (elapsedTime < appearDuration) {

            elapsedTime += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsedTime / appearDuration);
            float easedT = AnimationEase.EaseOutBack(t);

            textRect.localScale = Vector3.LerpUnclamped(startScale, endScale, easedT);

            yield return null;
        }

        textRect.localScale = endScale;

        yield return new WaitForSecondsRealtime(holdDuration);

        // Zoom in
        elapsedTime = 0f;

        while (elapsedTime < hideDuration) {

            elapsedTime += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsedTime / hideDuration);
            float easedT = AnimationEase.EaseInBack(t);

            textRect.localScale = Vector3.LerpUnclamped(endScale, startScale, easedT);

            yield return null;
        }

        textRect.localScale = startScale;
    }

    private IEnumerator IECountdown(Action callback) {

        for (int i = 3; i >= 0; i--) {

            string notiText = i != 0 ? $"{i}" : $"{READY_TEXT}";
            countdownText.text = $"{notiText}";

            yield return PlayCountdownTextAnimation(notiText);
        }

        countdownText.gameObject.SetActive(false);

        callback?.Invoke();
    }

    public override void SetUp() {

        setting.OnInit(this);
    }

    public void ActiveCountdown() {

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        countdownText.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(IECountdown(() => {

            LevelManager.Instance.ChangeLevelState(LevelState.Playing);
        }));
    }

    public void UpdateAliveLeftText(int aliveLeftValue) {

        aliveLeftText.text = $"Alive: {aliveLeftValue}";
    }

    public void ShowEventNoti() {

        eventNoti.gameObject.SetActive(true);
        eventNoti.ShowElement();
        eventNoti.Invoke(nameof(eventNoti.HideElement), 2f);
    }
}
