using System.Collections.Generic;
using UnityEngine;

public class ShopItemGrids : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private CanvasSkinShop canvasSkinShop;

    [Header("Child")]
    [SerializeField] private ShopItemSlot itemSlotPrefab;
    [SerializeField] private Transform content;

    private List<ShopItemSlot> shopItemSlotList = new List<ShopItemSlot>();

    private Transform shopTransform;
    private Transform ShopTransform => shopTransform == null ? shopTransform = this.transform : shopTransform;

    private ShopItemSlot currentSelectedSlot;

    public void SpawnItemSlots(List<ShopItemViewData> itemlist ) {

        shopItemSlotList.Clear();

        foreach (ShopItemViewData itemViewData in itemlist) {

            ShopItemSlot itemSlot = Instantiate(itemSlotPrefab, content);
            itemSlot.OnInit(this, itemViewData);
            
            shopItemSlotList.Add(itemSlot);

            if (itemViewData.IsEquipped()) {

                SelectSlot(itemSlot, itemViewData);
            }
        }

    }

    public void DespawnItemSlots() {

        for (int i =  shopItemSlotList.Count - 1; i >= 0; i--) {

            Destroy(shopItemSlotList[i].gameObject);
        }

        shopItemSlotList.Clear();

        currentSelectedSlot = null;
    }

    public void SelectSlot(ShopItemSlot itemSlot, ShopItemViewData itemData) {

        if (currentSelectedSlot != null) {
            // Off highlight old slot
            currentSelectedSlot.SetHighlight(false);
        }

        currentSelectedSlot = itemSlot;

        currentSelectedSlot.SetHighlight(true);

        canvasSkinShop.SelectItem(itemData);
    }

    public void UnlockCurrentSelectedSlot() {

        currentSelectedSlot.UnlockItem();
    }
}
