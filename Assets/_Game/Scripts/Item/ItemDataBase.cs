using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDataBase : IItemData {

    [Header("Base Data")]
    [SerializeField] protected string itemName;
    [SerializeField] protected int itemID;
    [SerializeField] protected Sprite itemSprite;

    [Header("Price")]
    [SerializeField] protected int price;

    [Header("Booster")]
    [SerializeField] protected List<BoosterData> boosterDataList;

    #region Booster Apply / Remove Behavior
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
    #endregion

    #region Getter
    public int GetItemID() {
        return itemID;
    }

    public Sprite GetItemSprite() {
        return itemSprite;
    }

    public string GetItemName() {
        return itemName;
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
    #endregion

    #region Item Shop Helper
    public bool IsSameID(int itemID) {
        
        return this.itemID == itemID;
    }

    public abstract bool IsOwned();

    public abstract bool IsEquipped();

    public abstract void Unlock();

    public abstract void Equip();

    public abstract void Preview(CharacterBase character);
    #endregion
}
