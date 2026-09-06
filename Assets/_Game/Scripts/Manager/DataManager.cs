using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class DataManager
{
    private const string GAME_DATA_KEY = "GameData";

    private static GameData gameData;
    
    private static void SaveDataToPrefs() {

        string jsonText = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(GAME_DATA_KEY, jsonText);
        PlayerPrefs.Save();
    }

    public static void OnInit() {


        gameData = GetGameData();
    }

#if UNITY_EDITOR

    public static void ForceResetGame() {
        gameData = new GameData();
        SaveDataToPrefs();
    }
#endif

    public static GameData GetGameData() {

        if (gameData == null) {
            //  Get Default Data First

            gameData = new GameData();

            if (PlayerPrefs.HasKey(GAME_DATA_KEY)) {
                // Having saved data before

                string saveDataText = PlayerPrefs.GetString(GAME_DATA_KEY);

                if (!string.IsNullOrEmpty(saveDataText)) {

                    JsonUtility.FromJsonOverwrite(saveDataText, gameData);
                }
            }
            else {
                // First time playing

                SaveDataToPrefs();
            }
        }


        return gameData;
    }

    public static void MutedSFX(bool isMuted) {

        if (isMuted) {
            
            gameData.MutedSFX();
        }
        else {

            gameData.UnMutedSFX();
        }

        SaveDataToPrefs();
    }

    public static void MutedMusic(bool isMuted) {

        if (isMuted) {

            gameData.MutedMusic();
        }
        else {

            gameData.UnMutedMusic();
        }

        SaveDataToPrefs();
    }

    public static void UpdateSavedLevel(int newLevelIndex) {

        gameData.GetPlayerData().SetCurrentLevel(newLevelIndex);

        SaveDataToPrefs();
    }

    public static void UpdateGold(int value, bool isIncrease = true) {

        int newGold = isIncrease ? gameData.GetPlayerData().CurrentGold + value : gameData.GetPlayerData().CurrentGold - value;
        newGold = Mathf.Max(0, newGold); // ensure new gold not lower than 0


        gameData.GetPlayerData().SetCurrentGold(newGold);

        SaveDataToPrefs();
    }

    public static void UpdateNewBestScore(int newBestScore) {

        gameData.GetPlayerData().SetNewBestScore(newBestScore);

        SaveDataToPrefs();
    }

    #region Hat Item Saved
    public static void UnlockHat(int hatID) {
        
        gameData.GetPlayerData().UnlockNewHatID(hatID);

        SaveDataToPrefs();
    }

    public static void ChangeEquippedHatTo(int hatID) {


        gameData.GetPlayerData().SetEquippedHatID(hatID);

        SaveDataToPrefs();
    }
    #endregion

    #region Pant Item Saved
    public static void UnlockPant(int pantID) {

        gameData.GetPlayerData().UnlockNewPantID(pantID);

        SaveDataToPrefs();
    }

    public static void ChangeEquippedPantTo(int pantID) {

        gameData.GetPlayerData().SetEquippedPantID(pantID);

        SaveDataToPrefs();
    }
    #endregion

    #region Accessory Item Saved
    public static void UnlockAccess(int accessoryID) {

        gameData.GetPlayerData().UnlockNewAccessoryID(accessoryID);

        SaveDataToPrefs();
    }

    public static void ChangeEquippedAccessoryTo(int accessoryID) {

        gameData.GetPlayerData().SetEquippedAccessoryID(accessoryID);

        SaveDataToPrefs();
    }
    #endregion

    #region Weapon Item Saved

    public static void UnlockWeapon(WeaponType weaponType) {
        gameData.GetPlayerData().UnlockWeapon(weaponType);

        SaveDataToPrefs();
    }


    public static void UnlockWeaponSkin(WeaponType weaponType, int skinID) {

        gameData.GetPlayerData().UnlockWeaponSkin(weaponType, skinID);

        SaveDataToPrefs();
    }

    public static void ChangeWeaponSkin(WeaponType weaponType, int skinID) {

        gameData.GetPlayerData().EquipWeaponSkinOfType(weaponType, skinID);

        SaveDataToPrefs();
    }

    public static void ChangeEquippedWeaponTo(WeaponType weaponType) {

        gameData.GetPlayerData().EquipWeapon(weaponType);

        SaveDataToPrefs();
    }

    public static int GetEquippedWeaponSkinID(WeaponType weaponType) {

        return gameData.GetPlayerData().GetEquippedWeaponSkinID(weaponType);
    }

    #endregion
}

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

    public void UnlockNewHatID(int hatID) {
        ownedHatIDList.Add(hatID);
    }

    public void UnlockNewPantID(int pantID) {
        ownedPantIDList.Add(pantID);
    }

    public void UnlockNewAccessoryID(int accessoryID) {
        ownedAccessoryIDList.Add(accessoryID);
    }

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

    public void EquipWeaponSkinOfType(WeaponType weaponType, int skinID) {

        WeaponSaveData weaponData = GetWeaponSaveData(weaponType);

        weaponData.EquipSkin(skinID);
    }

    public void EquipWeapon(WeaponType weaponType) {

        equippedWeaponType = weaponType;
    }
}

[System.Serializable]
public class WeaponSaveData {

    // Weapon State
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private bool isUnlocked;

    // Skin 
    [SerializeField] private int equippedSkinID;
    [SerializeField] private List<int> ownedSkinIDList;

    public WeaponType WeaponType => weaponType;
    public bool IsUnlocked => isUnlocked;


    public int EquippedSkinID => equippedSkinID;
    public List<int> OwnedSkinIDList => ownedSkinIDList;

    public WeaponSaveData(WeaponType weaponType) {

        this.weaponType = weaponType;
        this.isUnlocked = false;

        this.equippedSkinID = 0;
        this.ownedSkinIDList = new List<int>() { 0 }; // Default skin is ID 0
    }

    public void UnlockWeapon() {
        isUnlocked = true;
    }

    public void EquipSkin(int skinID) {
        this.equippedSkinID = skinID;
    }

    public bool IsWeaponUnlocked() {
        return isUnlocked;
    }

    public bool IsOwnedSkin(int skinID) {
        return ownedSkinIDList.Contains(skinID);
    }

    public void AddNewSkin(int skinID) {
        ownedSkinIDList.Add(skinID);
    }
}

[System.Serializable]
public class GameData {

    [SerializeField] private bool isMutedMusic;
    [SerializeField] private bool isMutedSFX;
    [SerializeField] private PlayerData playerData;

    public GameData() {

        this.isMutedMusic = false;
        this.isMutedSFX = false;
        this.playerData = new PlayerData();
    } 
    
    public PlayerData GetPlayerData() {
        return playerData;
    }

    public bool IsMutedMusic() {
        return isMutedMusic;
    }

    public bool IsMutedSFX() {
        return isMutedSFX;
    }

    public void MutedSFX() {
        this.isMutedSFX = true;
    }

    public void UnMutedSFX() {
        this.isMutedSFX = false;
    }

    public void MutedMusic() {
        this.isMutedMusic = true;
    }

    public void UnMutedMusic() {
        this.isMutedMusic = false;
    }
}
