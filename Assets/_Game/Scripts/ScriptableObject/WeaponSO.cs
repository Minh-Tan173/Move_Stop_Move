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

    private Dictionary<WeaponType, WeaponItemData> weaponItemDict = new Dictionary<WeaponType, WeaponItemData>();

    public WeaponItemData GetWeaponItemData(WeaponType weaponType) {
        
        if (!weaponItemDict.ContainsKey(weaponType)) {

            foreach (WeaponItemData itemData in weaponItemDataList) {

                if (itemData.IsSameWeaponType(weaponType)) {

                    weaponItemDict.Add(weaponType, itemData);
                }            
            }
        }

        return weaponItemDict[weaponType];
    }

    public PoolUnit GetWeaponPrefab(WeaponType weaponType) {

        return GetWeaponItemData(weaponType).GetPrefab();
    }

    public BulletBase GetBulletPrefab(WeaponType weaponType) {

        return GetWeaponItemData(weaponType).GetBulletPrefab();
    }
}

[System.Serializable]
public class WeaponItemData {

    [Header("Base Data")]
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string name;

    [Header("Prefab")]
    [SerializeField] private PoolUnit prefab;
    [SerializeField] private BulletBase prefabBullet;

    [Header("Price")]
    [SerializeField] private int goldPrice;
    [SerializeField] private Sprite sprite;

    public bool IsSameWeaponType(WeaponType weaponType) {
        return this.weaponType == weaponType;
    }

    public PoolUnit GetPrefab() {
        return this.prefab;
    }

    public BulletBase GetBulletPrefab() {
        return this.prefabBullet;
    }
}