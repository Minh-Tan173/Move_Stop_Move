using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public const int NONE_ID = -1;

    #region Field
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private int currentGold;
    [SerializeField] private int bestScore;

    // Color Visual
    [SerializeField] private int equippedColorID;

    // Current Equipped Item   
    [SerializeField] private int equippedHatID;
    [SerializeField] private int equippedPantID;
    [SerializeField] private int equippedAccessoryID;

    // Owned Item
    [SerializeField] private List<int> ownedHatIDList = new List<int>();
    [SerializeField] private List<int> ownedPantIDList = new List<int>();
    [SerializeField] private List<int> ownedAccessoryIDList = new List<int>();

    // Current Equipped Weapon
    [SerializeField] private WeaponType equippedWeaponType;

    // Owned Weapon
    [SerializeField] private List<WeaponSaveData> weaponSaveDataList = new List<WeaponSaveData>();

    #endregion

    public PlayerData() {

        currentLevelIndex = 0;
        currentGold = 0;

        equippedColorID = 0;

        equippedHatID = NONE_ID;
        equippedPantID = NONE_ID;
        equippedAccessoryID = NONE_ID;

    }

    private WeaponSaveData GetWeaponSaveData(WeaponType weaponType) {

        foreach (WeaponSaveData weaponData in weaponSaveDataList) {

            if (weaponData.WeaponType == weaponType) {
                return weaponData;
            }
        }

        // If dont having weapon data before --> Create new
        WeaponSaveData newData = new WeaponSaveData(weaponType);

        weaponSaveDataList.Add(newData);

        return newData;
    }

    #region Getter
    public int CurrentLevelIndex => currentLevelIndex;
    public int CurrentGold => currentGold;
    public int BestScore => bestScore;

    public int EquippedColorID => equippedColorID;
    public int EquippedHatID => equippedHatID;
    public int EquippedPantID => equippedPantID;
    public int EquippedAccessoryID => equippedAccessoryID;

    public List<int> OwnedHatIDList => ownedHatIDList;
    public List<int> OwnedPantIDList => ownedPantIDList;
    public List<int> OwnedAccessoryIDList => ownedAccessoryIDList;

    public WeaponType EquippedWeaponType => equippedWeaponType;
    #endregion

    public void SetCurrentLevel(int value) { currentLevelIndex = value; }
    public void SetCurrentGold(int value) { currentGold = value; }
    public void SetNewBestScore(int newBestScore) { bestScore = newBestScore; }

    public void SetEquipColorIndex(int colorID) { equippedColorID = colorID; }

    public void SetEquippedHatID(int hatID) { equippedHatID = hatID; }
    public void SetEquippedPantID(int pantID) { equippedPantID = pantID; }
    public void SetEquippedAccessoryID(int accessoryID) { equippedAccessoryID = accessoryID; }

    #region Unlock
    public void UnlockNewHatID(int hatID) {
        ownedHatIDList.Add(hatID);
    }

    public void UnlockNewPantID(int pantID) {
        ownedPantIDList.Add(pantID);
    }

    public void UnlockNewAccessoryID(int accessoryID) {
        ownedAccessoryIDList.Add(accessoryID);
    }

    public void UnlockWeapon(WeaponType weaponType) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        weaponData.UnlockWeapon();
    }

    public void UnlockWeaponSkin(WeaponType weaponType, int skinID) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        if (!weaponData.IsOwnedSkin(skinID)) {

            weaponData.AddNewSkin(skinID);
        }
    }
    #endregion

    public bool IsPlayerOwnedHat(int hatID) {
        return ownedHatIDList.Contains(hatID);
    }

    public bool IsPlayerOwnedPant(int pantID) {
        return ownedPantIDList.Contains(pantID);
    }

    public bool IsPlayerOwnedAccessory(int accessoryID) {
        return ownedAccessoryIDList.Contains(accessoryID);
    }

    public bool IsOwnedWeapon(WeaponType weaponType) {
        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        return weaponData.IsUnlocked;
    }

    public bool IsOwnedWeaponSkin(WeaponType weaponType, int skinID) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        return weaponData.IsOwnedSkin(skinID);
    }

    public bool IsEquippedWeaponSkin(WeaponType weaponType, int skinID) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        return weaponData.EquippedSkinID == skinID;
    }

    public int GetEquippedWeaponSkinID(WeaponType weaponType) {

        return GetWeaponSaveData(weaponType).EquippedSkinID;
    }

    public void EquipWeaponSkinOfType(WeaponType weaponType, int skinID) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        weaponData.EquipSkin(skinID);
    }

    public void EquipWeapon(WeaponType weaponType) {

        equippedWeaponType = weaponType;
    }
}