using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{

    [Header("Image")]
    [SerializeField] private Image itemSpriteImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image lockedIconImage;

    private ShopItemGrids owner;
    private ShopItemViewData currentData;

    private void SetLock(bool isLock) {

        lockedIconImage.gameObject.SetActive(isLock);
    }

    public void OnInit(ShopItemGrids owner , ShopItemViewData itemViewData) {

        SetHighlight(false);

        currentData = itemViewData;
        this.owner = owner;

        itemSpriteImage.sprite = itemViewData.GetIcon();
        SetLock(!itemViewData.IsUnlocked());

    }

    public void SetHighlight(bool isShow) {

        highlightImage.gameObject.SetActive(isShow);
    }

    public void UnlockItem() {

        SetLock(false);
    }

    public void OnClickSlot() { 

        owner.SelectSlot(this, currentData);
    }
}

[System.Serializable]
public class ShopItemViewData {

    private int itemID;
    private Sprite icon;
    private string itemName;
    private IItemData sourceItemData;
    private bool isUnlocked;

    public ShopItemViewData(IItemData itemData) {

        sourceItemData = itemData;
        itemID = itemData.GetItemID();
        icon = itemData.GetItemSprite();
        itemName = itemData.GetItemName();
        isUnlocked = itemData.IsOwned();
    }

    public int GetItemID() {
        return this.itemID;
    }

    public Sprite GetIcon() {
        return this.icon;
    }

    public string GetItemName() {
        return this.itemName;
    }

    public int GetPrice() {
        return sourceItemData.GetItemPrice();
    }

    public string GetBoosterDescription() {
        return sourceItemData.GetBoosterDescription();
    }

    public bool IsUnlocked() {
        return isUnlocked;
    }

    public bool IsEquipped() {
        return sourceItemData.IsEquipped();
    }

    public void Unlock() {

        sourceItemData.Unlock();

        isUnlocked = true;
    }

    public void Equip() {

        sourceItemData.Equip();
    }

    public void Preview(CharacterBase character) {

        sourceItemData.Preview(character);
    }
}
