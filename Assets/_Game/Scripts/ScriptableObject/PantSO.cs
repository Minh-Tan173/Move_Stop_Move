using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PantSO : ScriptableObject
{
    private Dictionary<int, PantItemData> pantDict = new Dictionary<int, PantItemData>();

    public List<PantItemData> pantItemDataList;

    public Texture2D GetPantTexture(int pantID) {
        

        if (!pantDict.ContainsKey(pantID)) {

            foreach (PantItemData pantItem in pantItemDataList) {

                if (pantItem.IsSameID(pantID)) {

                    pantDict.Add(pantID, pantItem);
                    break;
                }
            }
        }

        return pantDict[pantID].GetTexture();
    }
}

[System.Serializable]
public class PantItemData {

    [SerializeField] private int pantID;
    [SerializeField] private string pantName;
    [SerializeField] private Texture2D pantTexture;

    public bool IsSameID(int pantID) {
        return this.pantID == pantID;
    }

    public Texture2D GetTexture() {
        return pantTexture;
    }
}

//[System.Serializable]
//public class BufferPant {

//    public float attackRange;
//    public float speed;
//}