using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AccessorySO : ScriptableObject
{
    private Dictionary<int, AccessoryItemData> accessoryItemDict = new Dictionary<int, AccessoryItemData>();

    public List<AccessoryItemData> accessoryItemDataList;

    public PoolUnit GetAccesoryPrefab(int accessoryID) {

        if (!accessoryItemDict.ContainsKey(accessoryID)) {

            foreach (AccessoryItemData accessoryItem in accessoryItemDataList) {

                if (accessoryItem.IsSameID(accessoryID)) {

                    accessoryItemDict.Add(accessoryID, accessoryItem);
                    break;
                }
            }
        }

        return accessoryItemDict[accessoryID].GetPrefab();
    }
}

[System.Serializable]
public class AccessoryItemData {

    [SerializeField] private int accessoryID;
    [SerializeField] private string accessoryName;
    [SerializeField] private PoolUnit accessoryPrefab;

    public bool IsSameID(int accessoryID) {
        return this.accessoryID == accessoryID;
    }

    public PoolUnit GetPrefab() {
        return accessoryPrefab;
    }
}
