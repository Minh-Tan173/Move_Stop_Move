using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {

    Knife,
    Hammer,
    Boomerang
}

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    private readonly Dictionary<WeaponType, WeaponItemData> weaponDataDict = new Dictionary<WeaponType, WeaponItemData>();

    [SerializeField] private List<WeaponItemData> weaponItemDataList;

    public WeaponBase GetWeaponByType(WeaponType weaponType) {

        if (!weaponDataDict.ContainsKey(weaponType)) {

            foreach (WeaponItemData weaponItem in weaponItemDataList) {

                if (weaponItem.IsSameWeaponType(weaponType)) {

                    weaponDataDict.Add(weaponType, weaponItem);

                    break;
                }
            }
        }

        return weaponDataDict[weaponType].GetPrefab();
    }
}

[System.Serializable]
public class WeaponItemData {

    [SerializeField] private string name;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponBase prefab;
    [SerializeField] private int goldPrice;

    public bool IsSameWeaponType(WeaponType weaponType) {
        return this.weaponType == weaponType;
    }

    public WeaponBase GetPrefab() {
        return this.prefab;
    }
}