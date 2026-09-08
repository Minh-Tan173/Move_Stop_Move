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
public class PantItemData : ItemDataBase
{
    [Header("Visual Data")]
    [SerializeField] private Texture2D pantTexture;
    [SerializeField] private Sprite pantSprite;


    public override bool IsOwned() {

        return DataManager.GetGameData().GetPlayerData().IsPlayerOwnedPant(itemID);
    }

    public override bool IsEquipped() {

        return DataManager.GetGameData().GetPlayerData().EquippedPantID == itemID;
    }

    public override void Unlock() {

        DataManager.UnlockPant(itemID);
    }

    public override void Equip() {

        DataManager.ChangeEquippedPantTo(itemID);
    }

    public override void Preview(CharacterBase character) {

        character.GetCharacterVisual().ChangePants(character, itemID);
    }

    public Texture2D GetTexture() {
        return pantTexture;
    }
}