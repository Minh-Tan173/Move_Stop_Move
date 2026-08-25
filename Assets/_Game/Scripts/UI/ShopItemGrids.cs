using System.Collections.Generic;
using UnityEngine;

public class ShopItemGrids : MonoBehaviour
{
    [SerializeField] private ShopItemSlot itemSlotPrefab;

    private List<ShopItemSlot> shopItemSlotList = new List<ShopItemSlot>();

    private Transform shopTransform;
    private Transform ShopTransform => shopTransform == null ? shopTransform = this.transform : shopTransform; 

    public void SpawnItemSlots(List<ShopItemViewData> itemlist ) {

        shopItemSlotList.Clear();

        foreach (ShopItemViewData item in itemlist) {

            ShopItemSlot itemSlot = Instantiate(itemSlotPrefab, ShopTransform);

            // TODO: Add data for item slot

            shopItemSlotList.Add(itemSlot);
        }

    }

    public void DespawnItemSlots() {

        for (int i =  shopItemSlotList.Count - 1; i >= 0; i--) {

            Destroy(shopItemSlotList[i].gameObject);
        }

        shopItemSlotList.Clear();
    }
}
