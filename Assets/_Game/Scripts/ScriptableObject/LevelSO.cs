using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject {

    [SerializeField] private List<LevelData> levelList;

    public LevelBase GetLeveLPrefabByIndex(int levelIndex) {

        return levelList[levelIndex].GetLevelPrefab();
    }

    public int GetGoldRewarByIndex(int levelIndex) {
        return levelList[levelIndex].GetGoldReward();
    }

    public int TotalLevel() {
        return levelList.Count;
    }
}

[System.Serializable]
public class LevelData {

    public LevelBase levePrefab;
    public int goldReward;

    public LevelBase GetLevelPrefab() {
        return levePrefab;
    }

    public int GetGoldReward() {
        return goldReward;
    }
}
