using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType {

    Knife = 0,
    Hammer = 1,
    Axe = 2,
    Boomerang = 3
}

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private List<WeaponItemData> weaponItemDataList;

    public WeaponBase GetWeaponPrefab(WeaponType weaponType) {

        foreach (WeaponItemData itemData in weaponItemDataList) {

            if (!itemData.IsSameWeaponType(weaponType)) { continue; }

            return itemData.GetPrefab();
        }

        return null;
    }

    public BulletBase GetBulletPrefab(WeaponType weaponType) {

        foreach (WeaponItemData itemData in weaponItemDataList) {

            if (!itemData.IsSameWeaponType(weaponType)) { continue; }

            return itemData.GetBullet();
        }

        return null;
    }
}

[System.Serializable]
public class WeaponItemData {

    [Header("Base Data")]
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string name;

    [Header("Prefab")]
    [SerializeField] private WeaponBase prefab;
    [SerializeField] private BulletBase prefabBullet;

    [Header("Price")]
    [SerializeField] private int goldPrice;
    [SerializeField] private Sprite sprite;

    public bool IsSameWeaponType(WeaponType weaponType) {
        return this.weaponType == weaponType;
    }

    public WeaponBase GetPrefab() {
        return this.prefab;
    }

    public BulletBase GetBullet() {
        return this.prefabBullet;
    }
}