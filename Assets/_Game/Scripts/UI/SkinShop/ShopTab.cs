using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour, IPointerClickHandler {

    [Header("Ref")]
    [SerializeField] private ShopCategory shopCategory;
    [SerializeField] private Image tabImage;

    [Header("Event Invoke")]
    [SerializeField] private UnityEvent onSelectTab;

    public void SetAlphaImage(float alphaValue) {

        Color tempColor = tabImage.color;
        tempColor.a = alphaValue;
        tabImage.color = tempColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // When click on this tab

        ClickTab();
    }

    public void ClickTab() {

        onSelectTab?.Invoke();

        shopCategory.UpdateVisualTab(this);
    }
}
