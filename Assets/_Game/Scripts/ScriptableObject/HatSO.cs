using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HatSO : ScriptableObject
{
    private readonly Dictionary<int, HatItemData> hatDict = new Dictionary<int, HatItemData>();

    public List<HatItemData> hatItemDataList;

    public HatItemData GetHatData(int hatID) {

        if (!hatDict.ContainsKey(hatID)) {

            foreach (HatItemData hatItem in hatItemDataList) {

                if (hatItem.IsSameID(hatID)) {

                    hatDict.Add(hatID, hatItem);

                    break;
                }
            }
        }

        return hatDict[hatID];
    }

    public PoolUnit GetHatPrefab(int hatID) {

        return GetHatData(hatID).GetPrefab();    
    }
}

[System.Serializable]
public class HatItemData : ItemDataBase
{
    [Header("Visual Data")]
    [SerializeField] private PoolUnit hatPrefab;

    public override bool IsOwned() {

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedHat(itemID);
    }

    public override bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedHatID == itemID;
    }

    public override void Unlock() {

        DataManager.UnlockHat(itemID);
    }

    public override void Equip() {

        DataManager.ChangeEquippedHatTo(itemID);
    }

    public override void Preview(CharacterBase character) {

        character.GetCharacterVisual().ChangeHats(character, itemID);
    }

    public PoolUnit GetPrefab() {
        return hatPrefab;
    }
}
