using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PantSO : ScriptableObject
{
    private Dictionary<int, PantItemData> pantDict = new Dictionary<int, PantItemData>();

    public List<PantItemData> pantItemDataList;

    public PantItemData GetPantItemData(int pantID) {

        if (!pantDict.ContainsKey(pantID)) {

            foreach (PantItemData pantItem in pantItemDataList) {

                if (pantItem.IsSameID(pantID)) {

                    pantDict.Add(pantID, pantItem);
                    break;
                }
            }
        }

        return pantDict[pantID];
    }

    public Texture2D GetPantTexture(int pantID) {
     
        return GetPantItemData(pantID).GetTexture();
    }
}

[System.Serializable]
public class PantItemData {

    [Header("Base Data")]
    [SerializeField] private int pantID;
    [SerializeField] private string pantName;
    [SerializeField] private Texture2D pantTexture;

    [Header("Booster")]
    [SerializeField] private List<BoosterData> boosterDataList;

    public bool IsSameID(int pantID) {
        return this.pantID == pantID;
    }

    public Texture2D GetTexture() {
        return pantTexture;
    }

    public void ApplyBoosterFor(CharacterBase character) {

        foreach (BoosterData booster in boosterDataList) {
            booster.Apply(character);
        }
    }
}