using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform indicatorRect;
    [SerializeField] private Image indexCharImage;
    [SerializeField] private TextMeshProUGUI indexCharText;

    private CanvasCharacter targetChar;

    public void Bind(CanvasCharacter charTarget) {

        this.targetChar = charTarget;

        indexCharImage.sprite = charTarget.GetIndexCharSprite();
        indexCharText.text = charTarget.GetIndexCharText();

        Hide();
    }

    public void Release() {

        targetChar = null;

        indexCharImage.sprite = null;
        indexCharText.text = string.Empty;

        Hide();
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }


    public CanvasCharacter GetTargetChar() {
        return this.targetChar;
    }

    public RectTransform GetIndicatorRect() {
        return indicatorRect;
    }

    public bool IsCharTargetAvailable() {
        return targetChar == null;
    }
}
