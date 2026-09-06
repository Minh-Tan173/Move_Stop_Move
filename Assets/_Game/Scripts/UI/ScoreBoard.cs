using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [Header("Kill Count Text")]
    [SerializeField] private TextMeshProUGUI killCountLV1Text;
    [SerializeField] private TextMeshProUGUI killCountLV2Text;
    [SerializeField] private TextMeshProUGUI killCountLV3Text;
    [SerializeField] private TextMeshProUGUI killCountLV4Text;
    [SerializeField] private TextMeshProUGUI killCountLVHigherText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Best Score Icon")]
    [SerializeField] private Image bestScoreIcon;
    [SerializeField] private RectTransform bestScoreRect;
    [SerializeField] private float targetScaleSize;
    [SerializeField] private float showDuration = 0.35f;

    private void SetIconAlpha(float alphaValue) {

        Color temp = bestScoreIcon.color;
        temp.a = alphaValue;
        bestScoreIcon.color = temp;
    }

    private IEnumerator IEShowBestScoreIcon() {

        Vector3 finalSize = Vector3.one * targetScaleSize;
        
        SetIconAlpha(0f);
        bestScoreRect.localScale = Vector3.zero;

        float timer = 0f;

        while (timer < showDuration) {

            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / showDuration);

            float ease = AnimationEase.EaseOutBack(t);

            bestScoreRect.localScale = finalSize * ease;

            SetIconAlpha(t);

            yield return null;
        }

        bestScoreRect.localScale = finalSize;
        SetIconAlpha(1f);
    }

    public void UpdateKillScore() {

        PlayerScore playerScore = CharacterManager.Instance.GetPlayer().GetPlayerScore();

        int level1 = 1;
        killCountLV1Text.text = $"{playerScore.GetKillCount(level1)}";

        int level2 = 2;
        killCountLV2Text.text = $"{playerScore.GetKillCount(level2)}";

        int level3 = 3;
        killCountLV3Text.text = $"{playerScore.GetKillCount(level3)}";

        int level4 = 4;
        killCountLV4Text.text = $"{playerScore.GetKillCount(level4)}";


        killCountLVHigherText.text = $"{playerScore.GetKillCountHigherThanLevel(level4)}";

        int totalScore = playerScore.GetTotalScore();

        if (totalScore > DataManager.GetGameData().GetPlayerData().BestScore) {

            DataManager.UpdateNewBestScore(totalScore);

            bestScoreIcon.gameObject.SetActive(true);
            StartCoroutine(IEShowBestScoreIcon());
        }
        else {

            bestScoreIcon.gameObject.SetActive(false);
        }

        totalScoreText.text = $"Score: {playerScore.GetTotalScore()}";
    }
}
