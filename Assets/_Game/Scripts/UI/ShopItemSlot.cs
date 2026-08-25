using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{

    [Header("Image")]
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image lockImage;

    private bool isSelected;
    private bool isLocked;

    public void OnInit() {

        isSelected = false;
        lockImage.gameObject.SetActive(isSelected);
    }

    public void OnDespawn() {

    }
    
    public void OnClickSlot() {

        isSelected = !isSelected;
        lockImage.gameObject.SetActive(isSelected);

    }
}

[System.Serializable]
public class ShopItemViewData {

    private int idItem;
    private Sprite icon;
    private string itemName;

    public ShopItemViewData(int idItem, Sprite icon, string itemName) {
        
        this.idItem = idItem;
        this.icon = icon;
        this.itemName = itemName;
    }

    public int GetIDItem() {
        return this.idItem;
    }

    public Sprite GetIcon() {
        return this.icon;
    }

    public string GetItemName() {
        return this.itemName;
    }
}
