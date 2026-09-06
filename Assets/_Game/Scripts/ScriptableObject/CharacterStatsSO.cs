using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Attack")]
    [SerializeField] private float arDefaultSize;
    [SerializeField] private float attackCD;

    [Header("Level")]
    [SerializeField] private List<CharLevelData> charLevelDataList;
    [SerializeField] private float immortalDuration;

    private Dictionary<int, CharLevelData> charLevelDict = new Dictionary<int, CharLevelData>();

    public CharLevelData GetCharLevelData(int level) {

        if (!charLevelDict.ContainsKey(level)) {
            
            foreach (CharLevelData charLevel in charLevelDataList) {

                if (charLevel.IsSameLevel(level)) {

                    charLevelDict.Add(level, charLevel);
                }
            }
        }

        return charLevelDict[level];
    }

    public float GetMoveSpeed() {
        return this.moveSpeed;
    }

    public float GetARDefault() {
        return arDefaultSize;
    }

    public float GetAttackCD() {
        return attackCD;
    }

    public bool IsOverLevelList(int level) {
        return level > charLevelDataList.Count;     
    }

    public CharLevelData GetValidLevelData(int level) {

        int validLevel = Mathf.Min(level, charLevelDataList.Count);

        return GetCharLevelData(validLevel);
    }

    public float GetImmortalDuration() {
        return immortalDuration;
    }
}

[System.Serializable]
public class CharLevelData {

    [SerializeField] private int level;
    [SerializeField] private float bodyScaleSize;

    [Header("EXP")]
    [SerializeField] private int expRequired;
    [SerializeField] private int expReward;

    [Header("Score")]
    [SerializeField] private int scoreReward;

    public bool IsSameLevel(int level) {
        return this.level == level;
    }

    public int GetLevel() {
        return this.level;
    }

    public float GetBodyScale() {
        return this.bodyScaleSize;
    }

    public int GetExpRequired() {
        return expRequired;
    }

    public int GetExpReward() {
        return expReward;
    }

    public int GetScoreReward() {
        return scoreReward;
    }
}
