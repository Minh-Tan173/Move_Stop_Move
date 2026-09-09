using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene {
    LoadingScene,
    GameScene
}

public class CanvasLoadingScreen : UICanvas
{
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Text")]
    [SerializeField] private string[] loadingTextArray;

    private void Start() {

        SetUp();

    }

    public override void SetUp() {

        StartCoroutine(LoadingSceneCoroutine());
        StartCoroutine(LoadingTextCoroutine());
    }

    private IEnumerator LoadingSceneCoroutine() {

        float minimumLoadingTime = 2f;
        float elapsedTime = 0f;
        float visualProgress = 0f;


        yield return null;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(Scene.GameScene.ToString());

        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f || elapsedTime < minimumLoadingTime || visualProgress < 1f) {

            elapsedTime += Time.unscaledDeltaTime;

            float realProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);

            visualProgress = Mathf.MoveTowards(visualProgress, realProgress, Time.unscaledDeltaTime);

            if (loadOperation.progress >= 0.9f) {

                visualProgress = Mathf.MoveTowards(visualProgress, 1f, Time.unscaledDeltaTime);
            }

            yield return null;
        }

        loadOperation.allowSceneActivation = true;
    }



    private IEnumerator LoadingTextCoroutine() {

        float waitTimer = 0.3f;
        int currentIndex = 0;
        int totalIndex = loadingTextArray.Length;

        loadingText.text = loadingTextArray[currentIndex];

        while (true) {

            currentIndex = (currentIndex + 1) % totalIndex;
            loadingText.text = loadingTextArray[currentIndex];

            yield return new WaitForSeconds(waitTimer);
        }

    }
}
