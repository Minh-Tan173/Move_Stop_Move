using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [Header("Elements Anim")]
    [SerializeField] private float lineSpacing;
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private float animDelay = 0.05f;
    [SerializeField] private List<RectTransform> elementRectList;

    [Header("Button")]
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button homeButton;

    private Coroutine currentCoroutine;
    private bool isOpenSetting = false;

    private void ShowButton() {
        restartGameButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
    }

    private void HideButton() {
        restartGameButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
    }

    private IEnumerator ElementsAnim() {

        float elapsed = 0f;

        List<RectTransform> activeElementList = new List<RectTransform>();

        List<Vector2> startPosList = new List<Vector2>();


        foreach (RectTransform element in elementRectList) {

            if (!element.gameObject.activeSelf) { continue; }

            if (isOpenSetting) {

                element.anchoredPosition = Vector2.zero;
            }

            activeElementList.Add(element);
            startPosList.Add(element.anchoredPosition);
        }


        float totalDuration = animDuration + animDelay * (activeElementList.Count - 1);

        while (elapsed < totalDuration) {

            elapsed += Time.deltaTime;

            for (int i = 0; i < activeElementList.Count; i++) {

                float elementTime = elapsed - animDelay * i;

                if (elementTime <= 0f) continue;

                float t = Mathf.Clamp01(elementTime / animDuration);

                float easeT = isOpenSetting ? AnimationEase.EaseOutBack(t) : AnimationEase.EaseInBack(t);

                Vector2 targetPos = isOpenSetting ? Vector2.down * lineSpacing * (i + 1) : Vector2.zero;

                activeElementList[i].anchoredPosition = Vector2.LerpUnclamped(startPosList[i], targetPos, easeT);
            }

            yield return null;
        }

        currentCoroutine = null;
    }

    public void OnInit(UICanvas parentCanvas) {

        if (parentCanvas is CanvasMainMenu) {
            // If is in main menu

            HideButton();
        }
        else {
            // If is in game

            ShowButton();
        }

        foreach (RectTransform element in elementRectList) {

            if (element.gameObject.activeSelf) {
                element.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void TriggerSetting() {


        isOpenSetting = !isOpenSetting;

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        currentCoroutine = StartCoroutine(ElementsAnim());
    }

    public void RestartGame() {
    }

    public void MutedMusic(bool isMutedMusic) {

    }

    public void MutedSFX(bool isMutedSFX) {

    }
}
