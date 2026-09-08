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
public class AccessoryItemData : ItemDataBase
{
    [Header("Visual Data")]
    [SerializeField] private PoolUnit accessoryPrefab;

    [Header("Accessory Type")]
    [SerializeField] private AccessoryType accessoryType;

    public override bool IsOwned() {

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedAccessory(itemID);
    }

    public override bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedAccessoryID == itemID;
    }

    public override void Preview(CharacterBase character) {
        character.GetCharacterVisual().ChangeAccessories(character, itemID);
    }

    public override void Unlock() {

        DataManager.UnlockAccess(itemID);
    }

    public override void Equip() {

        DataManager.ChangeEquippedAccessoryTo(itemID);
    }

    public bool IsSpecialAccessory() {
        return accessoryType == AccessoryType.Special;
    }

    public PoolUnit GetPrefab() {
        return accessoryPrefab;
    }
}
