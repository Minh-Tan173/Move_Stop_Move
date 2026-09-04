using TMPro;
using UnityEngine;

public class BSTDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform boardRect;

    [SerializeField] private float baseHeight = 40f;
    [SerializeField] private float linePadding = 25f;

    private void ResizeBoard(int lineCount) {

        Vector2 size = boardRect.sizeDelta;

        size.y = baseHeight + linePadding * (lineCount - 1);

        boardRect.sizeDelta = size;
    }

    public void ShowPanel(string description) {

        if (string.IsNullOrEmpty(description)) {

            HidePanel();
            return;
        }

        gameObject.SetActive(true);

        descriptionText.text = description;

        int lineCount = description.Split("\n").Length;
        ResizeBoard(lineCount);
    }

    public void HidePanel() {
        this.gameObject.SetActive(false);
    }
}
