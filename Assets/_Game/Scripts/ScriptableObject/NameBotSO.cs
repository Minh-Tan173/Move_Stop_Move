using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class NameBotSO : ScriptableObject
{
    [SerializeField] private string[] nameBotArray;

    private List<string> availableNames = new List<string>();

    private void RefillNames() {    

        availableNames.Clear();
        availableNames.AddRange(nameBotArray);

        // Shuffle
        for (int i = availableNames.Count - 1; i > 0; i--) {

            int randomIndex = Random.Range(0, i + 1);

            (availableNames[i], availableNames[randomIndex]) =
                (availableNames[randomIndex], availableNames[i]);
        }
    }

    public string GetRandomName() {

        if (nameBotArray.Length == 0) {
            return "BOT";
        }

        if (availableNames.Count == 0) {
            RefillNames();
        }

        int lastIndex = availableNames.Count - 1;

        string randomName = availableNames[lastIndex];

        availableNames.RemoveAt(lastIndex);

        return randomName;

    }
}
