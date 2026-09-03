using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType {

    Knife = 0,
    Hammer = 1,
    Axe = 2,
    Boomerang = 3
}

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private List<WeaponItemData> weaponItemDataList;

    private Dictionary<WeaponType, WeaponItemData> weaponItemDict = new Dictionary<WeaponType, WeaponItemData>();

    public WeaponItemData GetWeaponItemData(WeaponType weaponType) {
        
        if (!weaponItemDict.ContainsKey(weaponType)) {

            foreach (WeaponItemData itemData in weaponItemDataList) {

                if (itemData.IsSameWeaponType(weaponType)) {

                    weaponItemDict.Add(weaponType, itemData);
                }            
            }
        }

        return weaponItemDict[weaponType];
    }

    public Weapon GetWeaponPrefab(WeaponType weaponType) {

        return GetWeaponItemData(weaponType).GetPrefab();
    }

    public BulletBase GetBulletPrefab(WeaponType weaponType) {

        return GetWeaponItemData(weaponType).GetBulletPrefab();
    }

    public WeaponSkinData GetWeaponSkinData(WeaponType weaponType, int skinID) {

        return GetWeaponItemData(weaponType).GetSkinData(skinID);
    }

    public List<WeaponSkinData> GetWeaponSkinDataList(WeaponType weaponType) {

        return GetWeaponItemData(weaponType).GetSkinDataList();
    }
}

[System.Serializable]
public class WeaponItemData {

    [Header("Base Data")]
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string name;
    [SerializeField] private int price;

    [Header("Prefab")]
    [SerializeField] private Weapon prefab;
    [SerializeField] private BulletBase bulletPrefab;

    [Header("Skin")]
    [SerializeField] private List<WeaponSkinData> weaponSkinDataList;

    private Dictionary<int, WeaponSkinData> weaponSkinDict = new Dictionary<int, WeaponSkinData>();


    public WeaponType WeaponType => weaponType;
    public string Name => name;
    public int Price => price;

    public bool IsSameWeaponType(WeaponType weaponType) {
        return this.weaponType == weaponType;
    }

    public Weapon GetPrefab() {
        return this.prefab;
    }

    public BulletBase GetBulletPrefab() {
        return this.bulletPrefab;
    }

    public List<WeaponSkinData> GetSkinDataList() {
        return weaponSkinDataList;
    }

    public WeaponSkinData GetSkinData(int skinID) {

        if (!weaponSkinDict.ContainsKey(skinID)) {

            foreach (WeaponSkinData skinData in weaponSkinDataList) {

                if (skinData.IsSameID(skinID)) {

                    weaponSkinDict.Add(skinID, skinData);
                    break;
                }
            }
        }

        return weaponSkinDict[skinID];
    }

    public bool IsUnlocked() {
        return DataManager.GetGameData().GetPlayerData().IsOwnedWeapon(weaponType);
    }

    public void UnlockWeapon() {
        DataManager.UnlockWeapon(weaponType);
    }

    public bool IsEquippedWeapon() {
        return DataManager.GetGameData().GetPlayerData().EquippedWeaponType == weaponType;
    }

    public void EquipWeapon() {
        DataManager.ChangeEquippedWeaponTo(weaponType);
    }

    public bool IsOwnedSkin(int skinID) {

        return DataManager.GetGameData().GetPlayerData().IsOwnedWeaponSkin(weaponType, skinID);
    }

    public bool IsEquippedSkin(int skinID) {

        return DataManager.GetGameData().GetPlayerData().IsEquippedWeaponSkin(weaponType, skinID);
    }

    public void UnlockSkin(int skinID) {

        DataManager.UnlockWeaponSkin(weaponType, skinID);
    }

    public void EquipSkin(int skinID) {

        DataManager.ChangeWeaponSkin(weaponType, skinID);
    }
}

[System.Serializable]
public class WeaponSkinData {

    [Header("Base Data")]
    [SerializeField] private int skinID;

    [Header("Visual")]
    [SerializeField] private Texture2D skinTexture;
    [SerializeField] private Sprite skinSprite;

    [Header("Price")]
    [SerializeField] private int price;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList = new List<BoosterData>();

    public int GetItemID() => skinID;
    public Sprite GetItemSprite() => skinSprite;
    public int GetItemPrice() => price;

    public Texture2D GetTexture() => skinTexture;

    public void ApplyBoosterFor(CharacterBase character) {
        
        foreach (BoosterData booster in boosterDataList) {
            booster.Apply(character);
        }
    }

    public void RemoveBoosterFor(CharacterBase character) {

        foreach (BoosterData booster in boosterDataList) {
            booster.Remove(character);
        }
    }

    public bool IsSameID(int id) {
        return skinID == id;
    }
}