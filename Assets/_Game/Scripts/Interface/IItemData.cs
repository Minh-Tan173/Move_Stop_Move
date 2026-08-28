using UnityEngine;

public interface IItemData
{
    public int GetItemID();
    
    public Sprite GetItemSprite();
    
    public string GetItemName();

    public int GetItemPrice();

    public bool IsOwned();
    
    public bool IsEquipped();

    public void Unlock();
    
    public void Equip();

    public void Preview(CharacterBase character);
}
