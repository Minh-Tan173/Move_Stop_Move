using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HatSO : ScriptableObject
{
    private readonly Dictionary<int, HatItemData> hatDict = new Dictionary<int, HatItemData>();

    public List<HatItemData> hatItemDataList;

    public HatItemData GetHatData(int hatID) {

        if (!hatDict.ContainsKey(hatID)) {

            foreach (HatItemData hatItem in hatItemDataList) {

                if (hatItem.IsSameID(hatID)) {

                    hatDict.Add(hatID, hatItem);

                    break;
                }
            }
        }

        return hatDict[hatID];
    }

    public PoolUnit GetHatPrefab(int hatID) {

        return GetHatData(hatID).GetPrefab();    
    }
}

[System.Serializable]
public class HatItemData 
{
    [Header("Base Data")]
    [SerializeField] private int hatID;
    [SerializeField] private string namehat;
    [SerializeField] private PoolUnit hatPrefab;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList;

    public bool IsSameID(int hatID) {
        return this.hatID == hatID;
    }

    public PoolUnit GetPrefab() {
        return hatPrefab;
    }

    public void ApplyBoosterFor(CharacterBase character) {
        
        foreach (BoosterData booster in boosterDataList) {
            booster.Apply(character);
        }
    }
}
