using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkinShop : UICanvas
{
    [Header("Shop Tab")]
    [SerializeField] private ShopCategory shopCategory;

    [Header("Shop Grid")]
    [SerializeField] private ShopItemGrids shopItemGrids;

    [Header("Item Data")]
    [SerializeField] private HatSO hatSO;
    [SerializeField] private PantSO pantSO;
    [SerializeField] private AccessorySO accessorySO;

    [Header("Action Button")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI warningNotiText;

    private const string GOLD_WARNING = "Not Enough Gold!!";

    private List<ShopItemViewData> hatViewDataList = new List<ShopItemViewData>();
    private List<ShopItemViewData> pantViewDataList = new List<ShopItemViewData>();
    private List<ShopItemViewData> accessoryViewDataList = new List<ShopItemViewData>();

    private ShopItemViewData currentSelectedItem;

    public override void SetUp() {

        CameraManager.Instance.SwitchCam(CameraType.ShopCamera);

        currentSelectedItem = null;

        HideWarningNoti();

        UpdateGoldText();

        UpdateActionButton();

        shopCategory.SelectedFirstTab();

    }

    private void UpdateGoldText() {

        int currentGold = DataManager.GetGameData().GetPlayerData().CurrentGold;

        goldText.text = currentGold.ToString();
    }

    private void UpdateActionButton() {

        if (currentSelectedItem == null) {

            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);

            return;
        }

        bool isUnlocked = currentSelectedItem.IsUnlocked();

        buyButton.gameObject.SetActive(!isUnlocked);
        if (!isUnlocked) {
            priceText.text = $"{currentSelectedItem.GetPrice()}";
        }

        equipButton.gameObject.SetActive(isUnlocked);
    }

    private void RegisterValueIntoViewList<T>(List<ShopItemViewData> viewDataList, List<T> itemDataList) where T : IItemData {

        if (viewDataList.Count > 0) { return; } // data is not empty

        foreach (T item in itemDataList) {

            ShopItemViewData itemInShop = new ShopItemViewData(item);

            viewDataList.Add(itemInShop);
        }
    }

    private List<ShopItemViewData> GetHatViewDataList() {

        RegisterValueIntoViewList<HatItemData>(hatViewDataList, hatSO.hatItemDataList);

        return hatViewDataList;
    }

    private List<ShopItemViewData> GetPantViewDataList() {

        RegisterValueIntoViewList<PantItemData>(pantViewDataList, pantSO.pantItemDataList);

        return pantViewDataList;
    }

    private List<ShopItemViewData> GetAccessoryViewDataList() {

        RegisterValueIntoViewList<AccessoryItemData>(accessoryViewDataList, accessorySO.accessoryItemDataList);

        return accessoryViewDataList;
    }

    private void ShowItemListInShop(List<ShopItemViewData> itemList) {

        currentSelectedItem = null;

        UpdateActionButton();

        shopItemGrids.DespawnItemSlots();
        shopItemGrids.SpawnItemSlots(itemList);
    }

    private void ShowWarningNoti() {

        warningNotiText.gameObject.SetActive(true);
        warningNotiText.text = $"{GOLD_WARNING}";

        CancelInvoke(nameof(HideWarningNoti));
        Invoke(nameof(HideWarningNoti), 1f);
    }

    private void HideWarningNoti() {
        warningNotiText.gameObject.SetActive(false);
    }

    public void CloseShop() {

        UIManager.Instance.CloseUI<CanvasSkinShop>(0.25f);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void ShowHatTab() {

        ShowItemListInShop(GetHatViewDataList());
    }

    public void ShowPantTab() {

        ShowItemListInShop(GetPantViewDataList());
    }

    public void ShowAccessoryTab() {

        ShowItemListInShop(GetAccessoryViewDataList());
    }

    public void SelectItem(ShopItemViewData itemData) {

        currentSelectedItem = itemData;

        currentSelectedItem.Preview(CharacterManager.Instance.GetPlayer());

        UpdateActionButton();
    }

    public void BuyCurrentItem() {

        int price = currentSelectedItem.GetPrice();

        if (DataManager.GetGameData().GetPlayerData().CurrentGold < price) {
            // Not enough gold

            ShowWarningNoti();
            return;
        }


        DataManager.UpdateGold(price, isIncrease: false);

        UpdateGoldText();   

        currentSelectedItem.Unlock();

        shopItemGrids.UnlockCurrentSelectedSlot();

        UpdateActionButton();
    }

    public void EquipCurrentItem() {

        if (currentSelectedItem == null) return;
        if (!currentSelectedItem.IsUnlocked()) return;

        currentSelectedItem.Equip();
    }

}
