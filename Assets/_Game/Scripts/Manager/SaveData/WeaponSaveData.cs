using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSaveData {

    // Weapon State
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private bool isUnlocked;

    // Skin 
    [SerializeField] private int equippedSkinID;
    [SerializeField] private List<int> ownedSkinIDList;

    public WeaponType WeaponType => weaponType;
    public bool IsUnlocked => isUnlocked;


    public int EquippedSkinID => equippedSkinID;
    public List<int> OwnedSkinIDList => ownedSkinIDList;

    public WeaponSaveData(WeaponType weaponType) {

        this.weaponType = weaponType;
        this.isUnlocked = false;

        this.equippedSkinID = 0;
        this.ownedSkinIDList = new List<int>() { 0 }; // Default skin is ID 0
    }

    public void UnlockWeapon() {
        isUnlocked = true;
    }

    public void EquipSkin(int skinID) {
        this.equippedSkinID = skinID;
    }

    public bool IsWeaponUnlocked() {
        return isUnlocked;
    }

    public bool IsOwnedSkin(int skinID) {
        return ownedSkinIDList.Contains(skinID);
    }

    public void AddNewSkin(int skinID) {
        ownedSkinIDList.Add(skinID);
    }
}

