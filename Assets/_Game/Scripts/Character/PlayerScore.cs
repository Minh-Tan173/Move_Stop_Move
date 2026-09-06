using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

    private Dictionary<int, int> killCountByLevelDict = new Dictionary<int, int>();

    public void OnInit() {

        killCountByLevelDict.Clear();
    }

    public void AddKill(int levelIndex) {

        if (!killCountByLevelDict.ContainsKey(levelIndex)) {

            killCountByLevelDict.Add(levelIndex, 0);
        }

        killCountByLevelDict[levelIndex] += 1;
    }

    public int GetKillCount(int level) {

        if (!killCountByLevelDict.ContainsKey(level)) {

            return 0;
        }

        return killCountByLevelDict[level];
    }

    public int GetTotalScore() {

        int totalScore = 0;

        foreach (int level in killCountByLevelDict.Keys) {

            int totalKill = GetKillCount(level);
            int score = totalKill * characterStatsSO.GetValidLevelData(level).GetScoreReward();

            totalScore += score;
        }

        return totalScore;
    }

    public int GetKillCountHigherThanLevel(int level) {
        
        int total = 0;

        foreach (int levelKey in killCountByLevelDict.Keys) {

            if (levelKey > level) {

                total += killCountByLevelDict[levelKey];
            }
        }

        return total;
    }
}
