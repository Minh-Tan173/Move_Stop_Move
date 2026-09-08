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