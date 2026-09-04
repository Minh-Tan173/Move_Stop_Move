using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PantSO : ScriptableObject
{
    private Dictionary<int, PantItemData> pantDict = new Dictionary<int, PantItemData>();

    public List<PantItemData> pantItemDataList;

    public PantItemData GetPantItemData(int pantID) {

        if (!pantDict.ContainsKey(pantID)) {

            foreach (PantItemData pantItem in pantItemDataList) {

                if (pantItem.IsSameID(pantID)) {

                    pantDict.Add(pantID, pantItem);
                    break;
                }
            }
        }

        return pantDict[pantID];
    }

    public Texture2D GetPantTexture(int pantID) {
     
        return GetPantItemData(pantID).GetTexture();
    }
}

[System.Serializable]
public class PantItemData : IItemData{

    [Header("Base Data")]
    [SerializeField] private int pantID;
    [SerializeField] private string pantName;
    [SerializeField] private Texture2D pantTexture;
    [SerializeField] private Sprite pantSprite;

    [Header("Price")]
    [SerializeField] private int price;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList;

    public int GetItemID() {
        return pantID;
    }

    public Sprite GetItemSprite() {
        return pantSprite;
    }

    public string GetItemName() {
        return pantName;
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

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedPant(pantID);
    }

    public bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedPantID == pantID;
    }

    public void Unlock() {

        DataManager.UnlockPant(pantID);
    }

    public void Equip() {

        DataManager.ChangeEquippedPantTo(pantID);
    }

    public void Preview(CharacterBase character) {

        character.GetCharacterVisual().ChangePants(character, pantID);
    }

    public bool IsSameID(int pantID) {
        return this.pantID == pantID;
    }

    public Texture2D GetTexture() {
        return pantTexture;
    }

    public void ApplyBoosterFor(CharacterBase character) {

        foreach (BoosterData booster in boosterDataList) {
            booster.Apply(character);
        }
    }
}