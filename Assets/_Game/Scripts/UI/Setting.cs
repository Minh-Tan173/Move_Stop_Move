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

        List<Vector2> startPosList = new List<Vector2>();
        List<Vector2> targetPosList = new List<Vector2>();

        for (int i = 0; i < elementRectList.Count; i++) {

            RectTransform rect = elementRectList[i];

            startPosList.Add(rect.anchoredPosition);

            Vector2 target = isOpenSetting ? rect.anchoredPosition + Vector2.down * lineSpacing * i : rect.anchoredPosition - Vector2.down * lineSpacing * i;

            targetPosList.Add(target);
        }


        float totalDuration = animDuration + animDelay * (elementRectList.Count - 1);

        while (elapsed < totalDuration) {

            elapsed += Time.deltaTime;

            for (int i = 0; i < elementRectList.Count; i++) {
                float elementTime = elapsed - animDelay * i;

                if (elementTime <= 0f)
                    continue;

                float t = Mathf.Clamp01(elementTime / animDuration);

                float easeT = isOpenSetting ? AnimationEase.EaseOutBack(t) : AnimationEase.EaseInBack(t);

                elementRectList[i].anchoredPosition = Vector2.LerpUnclamped(startPosList[i], targetPosList[i], easeT);
            }

            yield return null;
        }

        currentCoroutine = null;
    }

    public void OnInit() {

        if (UIManager.Instance.GetUI<CanvasMainMenu>().gameObject.activeSelf) {
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

        LevelManager.Instance.OnPauseGame();

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }

        isOpenSetting = !isOpenSetting;
        StartCoroutine(ElementsAnim());
    }

    public void MutedMusic(bool isMutedMusic) {

    }

    public void MutedSFX(bool isMutedSFX) {

    }
}
