using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform indicatorRect;

    [Header("Arrow")]
    [SerializeField] private RectTransform arrowRect;
    [SerializeField] private float arrowPadding = 10f;


    [Header("Icon")]
    [SerializeField] private Image indexCharImage;
    [SerializeField] private RectTransform indexCharRect;
    [SerializeField] private TextMeshProUGUI indexCharText;

    private CanvasCharacter targetChar;

    public void UpdateArrowPosition(Vector2 direction) {

        float absX = Mathf.Abs(direction.x);    
        float absY = Mathf.Abs(direction.y);

        if (absX > absY) {
            // Left / Right

            if (direction.x > 0) {
                // Right side
                arrowRect.anchoredPosition = new Vector2(indexCharRect.rect.width * 0.5f + arrowPadding, 0f);
            }
            else {
                // Left side
                arrowRect.anchoredPosition = new Vector2(-(indexCharRect.rect.width * 0.5f + arrowPadding), 0f); 
            }
        }
        else {
            // Top / Bottom

            if (direction.y > 0) {
                // Top side
                arrowRect.anchoredPosition = new Vector2(0f, indexCharRect.rect.height * 0.5f + arrowPadding);
            }
            else {
                // Bottom side
                arrowRect.anchoredPosition = new Vector2(0f, -(indexCharRect.rect.height * 0.5f + arrowPadding));
            }
        }
    }


    public void SetArrowDirection(Vector2 direction) {

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        arrowRect.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

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

        arrowRect.anchoredPosition = Vector2.zero;
        arrowRect.localRotation = Quaternion.identity;

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
        return targetChar != null;
    }
}
