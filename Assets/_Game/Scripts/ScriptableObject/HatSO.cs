using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HatSO : ScriptableObject
{
    private readonly Dictionary<int, HatItemData> hatDict = new Dictionary<int, HatItemData>();

    public List<HatItemData> hatItemDataList;

    public PoolUnit GetHatPrefab(int hatID) {

        if (!hatDict.ContainsKey(hatID)) {

            foreach (HatItemData hatItem in hatItemDataList) {

                if (hatItem.IsSameID(hatID)) {

                    hatDict.Add(hatID, hatItem);

                    break;
                }
            }
        }

        return hatDict[hatID].GetPrefab();    
    }
}

[System.Serializable]
public class HatItemData 
{

    [SerializeField] private int hatID;
    [SerializeField] private string namehat;
    [SerializeField] private PoolUnit hatPrefab;

    public bool IsSameID(int hatID) {
        return this.hatID == hatID;
    }

    public PoolUnit GetPrefab() {
        return hatPrefab;
    }
}



