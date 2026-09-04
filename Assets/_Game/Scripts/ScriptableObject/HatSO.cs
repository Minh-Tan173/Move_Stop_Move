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
public class HatItemData : IItemData
{
    [Header("Base Data")]
    [SerializeField] private int hatID;
    [SerializeField] private string hatName;
    [SerializeField] private PoolUnit hatPrefab;
    [SerializeField] private Sprite hatSprite;

    [Header("Price")]
    [SerializeField] private int price;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList;


    public int GetItemID() {
        return hatID;
    }

    public Sprite GetItemSprite() {
        return hatSprite;
    }

    public string GetItemName() {
        return hatName;
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

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedHat(hatID);
    }

    public bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedHatID == hatID;
    }

    public void Unlock() {

        DataManager.UnlockHat(hatID);
    }

    public void Equip() {

        DataManager.ChangeEquippedHatTo(hatID);
    }

    public void Preview(CharacterBase character) {

        character.GetCharacterVisual().ChangeHats(character, hatID);
    }


    public bool IsSameID(int hatID) {
        return this.hatID == hatID;
    }

    public PoolUnit GetPrefab() {
        return hatPrefab;
    }

    public void ApplyBoosterFor(CharacterBase character) {
        
        foreach (BoosterData booster in boosterDataList) {
            booster.Apply(character);
        }
    }
}
