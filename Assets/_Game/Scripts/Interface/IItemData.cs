using UnityEngine;

public interface IItemData
{
    public int GetItemID();
    
    public Sprite GetItemSprite();
    
    public string GetItemName();

    public int GetItemPrice();

    public string GetBoosterDescription();

    public bool IsOwned();
    
    public bool IsEquipped();

    public void Unlock();
    
    public void Equip();

    public void Preview(CharacterBase character);
}
