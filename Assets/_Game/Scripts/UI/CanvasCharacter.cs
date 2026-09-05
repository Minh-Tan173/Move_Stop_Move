using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacter : MonoBehaviour
{
    [SerializeField] private CharacterBase character;

    [Header("Name Tag")]
    [SerializeField] private RectTransform nameTag;
    [SerializeField] private Vector2 nameTagDefaultPos;
    [SerializeField] private TextMeshProUGUI nameCharText;
    [SerializeField] private Image indexCharImage;
    [SerializeField] private TextMeshProUGUI indexCharText;

    [Header("Exp Gain")]
    [SerializeField] private TextMeshProUGUI expGainText;
    [SerializeField] private RectTransform expGainTextRect;
    [SerializeField] private Vector2 expTextStartPos;
    [SerializeField] private float flyHeight;
    [SerializeField] private float duration = 1f;

    private float defaultY;
    
    private Coroutine currentCoroutine;

    private IEnumerator IEPlayEXPGain(int exp) {

        expGainTextRect.anchoredPosition = expTextStartPos;
        expGainText.text = $"+{exp}";
        SetAlphaText(1f);

        Vector2 expTextEndPos = expTextStartPos + Vector2.up * flyHeight;
        float elapsed = 0f;

        while (elapsed <= duration) {

            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float easeT = AnimationEase.EaseOutCubic(t);

            expGainTextRect.anchoredPosition = Vector2.Lerp(expTextStartPos, expTextEndPos, easeT);

            //SetAlphaText(1f - easeT);

            yield return null;
        }

        yield return null;
        SetAlphaText(0f);

        expGainTextRect.anchoredPosition = expTextStartPos;
        expGainText.gameObject.SetActive(false);

    }

    private void SetAlphaText(float alphaValue) {

        Color temp = expGainText.color;
        temp.a = alphaValue;
        expGainText.color = temp;
    }

    public void OnInit() {

        nameTag.anchoredPosition = nameTagDefaultPos;
        defaultY = nameTag.anchoredPosition.y;

        expGainText.gameObject.SetActive(false);
    }

    public void OnDespawn() {

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    public void UpdateHeight(float bodyScale) {

        Vector2 pos = nameTag.anchoredPosition;

        pos.y = defaultY * bodyScale;

        nameTag.anchoredPosition = pos;
    }

    public void ShowEXPGain(int expGain) {

        expGainText.gameObject.SetActive(true);

        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        currentCoroutine = StartCoroutine(IEPlayEXPGain(expGain));
    }

    public void SetName(string name) {

        nameCharText.text = $"{name}";
    }

    public void SetIndex(int indexChar) {

        indexCharText.text = $"{indexChar}";
    }

    public void ShowNameTag() {
        nameTag.gameObject.SetActive(true);
    }

    public void HideNameTag() {
        nameTag.gameObject.SetActive(false);
    }

    public string GetName() {
        return nameCharText.text;
    }

    public Sprite GetIndexCharSprite() {
        return indexCharImage.sprite;
    }

    public string GetIndexCharText() {
        return indexCharText.text;
    }

    public Vector3 GetNametagPosition() {
        return nameTag.position;
    }
}
