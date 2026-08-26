using UnityEngine;

public interface IItemData
{
    public int GetItemID();
    
    public Sprite GetItemSprite();
    
    public string GetItemName();

    public int GetItemPrice();

    bool IsOwned();
    
    bool IsEquipped();

    void Unlock();
    
    void Equip();
}
