using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics.Editor;
using UnityEngine;

public enum EventNotiState {
    Hidden,
    Showing,
    Holding,
    Hiding
}

public class CanvasHUD : UICanvas
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI aliveLeftText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Event Noti")]
    [SerializeField] private RectTransform eventNoti;
    [SerializeField] private float eventNotiShowDuration = 0.3f;
    [SerializeField] private float eventNotiHoldDuration = 2f;
    [SerializeField] private float eventNotiHideDuration = 0.2f;

    [Header("Setting")]
    [SerializeField] private Setting setting;

    private const string READY_TEXT = "GO!";

    private Coroutine currentCoroutine;

    private EventNotiState eventNotiState;
    private float eventNotiTimer;

    private void Update() {

        UpdateEventNoti();
    }

    private void UpdateEventNoti() {

        switch (eventNotiState) {

            case EventNotiState.Showing:

                if (UpdateEventNotiScale(true)) {

                    eventNoti.localScale = Vector3.one;

                    eventNotiTimer = 0f;
                    eventNotiState = EventNotiState.Holding;
                }

                break;

            case EventNotiState.Holding:

                eventNotiTimer += Time.unscaledDeltaTime;

                if (eventNotiTimer >= eventNotiHoldDuration) {

                    eventNotiTimer = 0f;
                    eventNotiState = EventNotiState.Hiding;
                }

                break;

            case EventNotiState.Hiding:

                if (UpdateEventNotiScale(false)) {

                    eventNoti.localScale = Vector3.zero;

                    eventNotiTimer = 0f;
                    eventNotiState = EventNotiState.Hidden;

                    eventNoti.gameObject.SetActive(false);
                }

                break;
        }
    }

    private bool UpdateEventNotiScale(bool isShowing) {

        eventNotiTimer += Time.unscaledDeltaTime;

        float duration = isShowing ? eventNotiShowDuration : eventNotiHideDuration;

        float t = Mathf.Clamp01(eventNotiTimer / duration);

        float easedT = isShowing ? AnimationEase.EaseOutBack(t) : AnimationEase.EaseInBack(t);

        Vector3 startScale = isShowing ? Vector3.zero : Vector3.one;
        Vector3 endScale = isShowing ? Vector3.one : Vector3.zero;

        eventNoti.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedT);

        return t >= 1f;
    }


    private IEnumerator IECountdown(Action callback) {

        for (int i = 3; i >= 0; i--) {

            string notiText = i != 0 ? $"{i}" : $"{READY_TEXT}";
            countdownText.text = $"{notiText}";

            // Play SFX base on text show on screen
            SFXType countdownSFX = string.Equals(notiText, READY_TEXT) ? SFXType.CountdownComplete : SFXType.Countdown;
            SoundManager.Instance.PlayUISound(countdownSFX);

            // Configure the target scale based on the text
            float targetScale = string.Equals(notiText, READY_TEXT) ? 1.25f : 1f;
            RectTransform textRect = countdownText.rectTransform;

            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one * targetScale;

            // Define animation durations
            float appearDuration = 0.3f;
            float holdDuration = 0.45f;
            float hideDuration = 0.2f;

            // Appear animation (Zoom out effect)
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

            // Wait for the holding duration
            yield return new WaitForSecondsRealtime(holdDuration);

            // Disappear animation (Zoom in effect)
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

        // Countdown finished
        countdownText.gameObject.SetActive(false);
        callback?.Invoke();
    }

    public override void SetUp() {

        setting.OnInit(this);

        eventNoti.gameObject.SetActive(false);
        eventNotiState = EventNotiState.Hidden;
        eventNotiTimer = 0f;
        eventNoti.localScale = Vector3.zero;

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
        eventNoti.localScale = Vector3.zero;

        eventNotiTimer = 0f;
        eventNotiState = EventNotiState.Showing;
    }

    public void StopUIAnimation() {

        eventNotiTimer = 0f;
        eventNotiState = EventNotiState.Hidden;

        eventNoti.localScale = Vector3.zero;
        eventNoti.gameObject.SetActive(false);
    }
}
