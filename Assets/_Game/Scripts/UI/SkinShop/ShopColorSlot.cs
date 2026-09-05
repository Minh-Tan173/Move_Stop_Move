using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopColorSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image colorImage;
    [SerializeField] private Image hightLightImage;

    private ColorType colorOfSlot;
    private ShopColorGrids ownerGrids;

    public void OnInit(ShopColorGrids owner, ColorType colorType, Color color) {

        ownerGrids = owner;
        colorOfSlot = colorType;
        colorImage.color = color;
    }

    public void ShowHighlight(bool isShow) {

        hightLightImage.gameObject.SetActive(isShow);
    }

    public ColorType GetColorOfSlot() {
        return colorOfSlot;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // When Clicked on this Slot

        ownerGrids.SelectColorSlot(this);
    }
}
