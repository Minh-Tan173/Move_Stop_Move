using System.Collections.Generic;
using UnityEngine;

public enum AccessoryType {

    Normal,
    Special
}

[CreateAssetMenu()]
public class AccessorySO : ScriptableObject
{
    private Dictionary<int, AccessoryItemData> accessoryItemDict = new Dictionary<int, AccessoryItemData>();

    public List<AccessoryItemData> accessoryItemDataList;

    public AccessoryItemData GetAccessoryItemData(int accessoryID) {

        if (!accessoryItemDict.ContainsKey(accessoryID)) {

            foreach (AccessoryItemData accessoryItem in accessoryItemDataList) {

                if (accessoryItem.IsSameID(accessoryID)) {

                    accessoryItemDict.Add(accessoryID, accessoryItem);
                    break;
                }
            }
        }

        return accessoryItemDict[accessoryID];
    }

    public PoolUnit GetAccesoryPrefab(int accessoryID) {

        return GetAccessoryItemData(accessoryID).GetPrefab();
    }
}

[System.Serializable]
public class AccessoryItemData : IItemData
{
    [Header("Base Data")]
    [SerializeField] private string accessoryName;
    [SerializeField] private int accessoryID;
    [SerializeField] private PoolUnit accessoryPrefab;
    [SerializeField] private Sprite accessorySprite;

    [Header("Accessory Type")]
    [SerializeField] private AccessoryType accessoryType;

    [Header("Price")]
    [SerializeField] private int price;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList;

    public int GetItemID() {
        return accessoryID;
    }

    public Sprite GetItemSprite() {
        return accessorySprite;
    }

    public string GetItemName() {
        return accessoryName;
    }


    public int GetItemPrice() {
        return price;
    }

    public string GetBoosterDescription() {

        List<string> descriptions = new List<string>();

        foreach (BoosterData booster in boosterDataList) {
            descriptions.Add(booster.GetDescription());
        }

        return string.Join("\n", descriptions);
    }

    public bool IsOwned() {

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedAccessory(accessoryID);
    }

    public bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedAccessoryID == accessoryID;
    }

    public void Preview(CharacterBase character) {
        character.GetCharacterVisual().ChangeAccessories(character, accessoryID);
    }

    public void Unlock() {

        DataManager.UnlockAccess(accessoryID);
    }

    public void Equip() {

        DataManager.ChangeEquippedAccessoryTo(accessoryID);
    }

    public bool IsSameID(int accessoryID) {
        return this.accessoryID == accessoryID;
    }

    public bool IsSpecialAccessory() {
        return accessoryType == AccessoryType.Special;
    }

    public PoolUnit GetPrefab() {
        return accessoryPrefab;
    }

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
}
