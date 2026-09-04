using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasWeaponShop : UICanvas
{
    [Header("Data")]
    [SerializeField] private WeaponSO weaponData;

    [Header("Preview")]
    [SerializeField] private WeaponPreview weaponPreview;

    [Header("Skin")]
    [SerializeField] private Transform skinContainer;
    [SerializeField] private WeaponSkinSlot skinSlotPrefab;

    [Header("Button")]
    [SerializeField] private WeaponShopActionButton actionButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameWeaponText;
    [SerializeField] private TextMeshProUGUI goldTotalText;

    [Header("Description Board")]
    [SerializeField] private BSTDescriptionPanel bstDescriptionPanel;

    private WeaponItemData currentSelectedWeapon;
    private List<WeaponSkinSlot> skinSlotList = new List<WeaponSkinSlot>();

    private WeaponSkinData currentSelectedSkin;
    private int currentWeaponIndex;


    public override void SetUp() {

        CameraManager.Instance.SwitchCam(CameraType.ShopCamera);

        WeaponType equippedWeapon = DataManager.GetGameData().GetPlayerData().EquippedWeaponType;

        currentWeaponIndex = (int)equippedWeapon;

        LoadWeapon(equippedWeapon);

        actionButton.OnInit(this);

        UpdateCoin();
    }

    private void UpdateCoin() {

        int totalGold = DataManager.GetGameData().GetPlayerData().CurrentGold;
        goldTotalText.text = $"{totalGold}";
    }

    private void BuyWeapon() {
        int price = currentSelectedWeapon.Price;

        if (DataManager.GetGameData().GetPlayerData().CurrentGold < price) {
            // Not Enough Gold
            return;
        }

        DataManager.UpdateGold(price, false);

        currentSelectedWeapon.UnlockWeapon();

        ShowWeaponUnlocked();

        LoadSkinSlots();

        UpdateCoin();
    }

    private void BuySkin() {

        int price = currentSelectedSkin.GetItemPrice();

        if (DataManager.GetGameData().GetPlayerData().CurrentGold < price) {
            return;
        }

        DataManager.UpdateGold(price, false);

        currentSelectedWeapon.UnlockSkin(currentSelectedSkin.GetItemID());

        // After Buy Skin
        RefreshSkinSlots();

        UpdateCoin();
    }

    private void LoadWeapon(WeaponType weaponType) {

        currentSelectedWeapon = weaponData.GetWeaponItemData(weaponType);

        nameWeaponText.text = $"{currentSelectedWeapon.Name}";

        currentSelectedSkin = null;
        weaponPreview.ShowWeapon(currentSelectedWeapon);

        bstDescriptionPanel.HidePanel();

        if (!currentSelectedWeapon.IsUnlocked()) {
            ClearSkinSlots();
            ShowWeaponLocked();
        }
        else {
            ShowWeaponUnlocked();
            LoadSkinSlots();
        }
    }

    private void LoadSkinSlots() {

        ClearSkinSlots();

        WeaponSkinSlot equippedSlot = null;
        WeaponSkinData equippedSkin = null;

        foreach (WeaponSkinData skinData in currentSelectedWeapon.GetSkinDataList()) {

            WeaponSkinSlot slot = Instantiate(skinSlotPrefab, skinContainer);

            slot.OnInit(this,currentSelectedWeapon, skinData);

            skinSlotList.Add(slot);

            if (currentSelectedWeapon.IsEquippedSkin(skinData.GetItemID())) {

                equippedSlot = slot;
                equippedSkin = skinData;
            }
        }

        if (equippedSlot != null) { 

            SelectSkin(equippedSlot, equippedSkin);
        }
    }
    private void RefreshSkinSlots() {

        foreach (WeaponSkinSlot slot in skinSlotList) {

            slot.Refresh();
        }
    }

    private void ClearSkinSlots() {

        foreach (WeaponSkinSlot slot in skinSlotList) {

            Destroy(slot.gameObject);
        }

        skinSlotList.Clear();
    }

    private void UpdateActionButton() {

        if (!currentSelectedWeapon.IsEquippedWeapon()) {
            // If player is not equipped this weapon

            actionButton.SetEquip();
            return;
        }

        int skinID = currentSelectedSkin.GetItemID();

        if (currentSelectedWeapon.IsEquippedSkin(skinID)) {

            actionButton.SetEquipped();
        }
        else if (currentSelectedWeapon.IsOwnedSkin(skinID)) {
            
            actionButton.SetEquip();
        }
        else {
            // Not owned this weapon
            actionButton.SetBuy(currentSelectedSkin.GetItemPrice());
        }
    }

    private void ShowWeaponLocked() {

        currentSelectedSkin = null;

        skinContainer.gameObject.SetActive(false);

        actionButton.gameObject.SetActive(true);

        actionButton.SetBuy(currentSelectedWeapon.Price);

        bstDescriptionPanel.HidePanel();
    }

    private void ShowWeaponUnlocked() {

        skinContainer.gameObject.SetActive(true);

        actionButton.gameObject.SetActive(true);
    }

    public void SelectSkin(WeaponSkinSlot selectedSlot, WeaponSkinData skinData) {

        currentSelectedSkin = skinData;

        foreach (WeaponSkinSlot skinSlot in skinSlotList) {

            bool isHighlighThisSlot = skinSlot == selectedSlot;
            skinSlot.SetHighlight(isHighlighThisSlot);
        }


        weaponPreview.ApplySkin(skinData.GetTexture());


        UpdateActionButton();

        bstDescriptionPanel.ShowPanel(skinData.GetBoosterDescription());
    }

    public void OnClickActionButton() {
        // When click on Action Button

        if (!currentSelectedWeapon.IsUnlocked()) {
            // If not unlocked --> Buy

            BuyWeapon();
          
        }
        else {

            if (!currentSelectedWeapon.IsEquippedWeapon()) {
                // If not equipped current selected button --> Equipped

                currentSelectedWeapon.EquipWeapon();
                UpdateActionButton();

                CharacterManager.Instance.GetPlayer().OnInit();
                return;
            }

            int skinID = currentSelectedSkin.GetItemID();

            if (currentSelectedWeapon.IsOwnedSkin(skinID)) {

                currentSelectedWeapon.EquipSkin(skinID);

                CharacterManager.Instance.GetPlayer().OnInit();
            }
            else {

                BuySkin();
            }

            UpdateActionButton();
        }
    }

    public void NextWeapon() {

        int nextIndex = currentWeaponIndex + 1;
        int totalWeapon = Enum.GetValues(typeof(WeaponType)).Length;
        currentWeaponIndex = nextIndex % totalWeapon;

        LoadWeapon((WeaponType)currentWeaponIndex);
    }

    public void PreviousWeapon() {

        int previousIndex = currentWeaponIndex - 1;
        int totalWeapon = Enum.GetValues(typeof(WeaponType)).Length;
        currentWeaponIndex = (previousIndex + totalWeapon) % totalWeapon;

        LoadWeapon((WeaponType)currentWeaponIndex);
    }

    public void CloseShop() {

        UIManager.Instance.CloseUI<CanvasWeaponShop>(0.25f);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}
