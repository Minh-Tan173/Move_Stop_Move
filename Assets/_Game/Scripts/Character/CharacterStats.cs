using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected AttackRangeVisual attackRangeVisual;

    [Header("Data")]
    [SerializeField] private CharacterStatsSO characterStatsSO;

    private float attackRange;

    private float moveSpeed;

    private int currentLevel;
    private int expProgress;
    private float bodySizeScale;

    private CharacterBase character;
    private CharacterBase Character => character == null ? character = GetComponent<CharacterBase>() : character;

    public void OnInit() {

        ResetMoveSpeed();
        ResetAttackSize();
        ResetLevel();
    }

    #region Attack Range
    public void SetAttackSize(float value) {
        
        attackRange = value;

        attackRangeVisual.UpdateVisual();
    }

    public void AddAttackSize(float value) {
        SetAttackSize(attackRange += value);
    }

    public void UpAttackSize() {
        SetAttackSize(attackRange += 1);
    }

    public void ResetAttackSize() {
        SetAttackSize(characterStatsSO.GetARDefault());
    }

    public float GetAttackRange() {
        return this.attackRange;
    }
    #endregion

    #region Attack CD
    public float GetAttackCD() {
        return this.characterStatsSO.GetAttackCD();
    }
    #endregion

    #region Move Speed
    private void SetMoveSpeed(float moveSpeed) {    
        
        this.moveSpeed = moveSpeed;

        if (character is Bot) {

            Bot bot = character as Bot;
            bot.UpdateNavMeshSpeed();
        }
    }

    public void AddMoveSpeed(float value) {

        SetMoveSpeed(moveSpeed + value);
    }

    public void ResetMoveSpeed() {

        SetMoveSpeed(characterStatsSO.GetMoveSpeed());
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    #endregion

    #region Level
    private CharLevelData GetCurrentLevelData() {
        return characterStatsSO.GetValidLevelData(currentLevel);
    }

    private void UpdateBodySize() {

        float newScale = characterStatsSO.GetCharLevelData(currentLevel).GetBodyScale();
        bodySizeScale = 1f + newScale;

        Character.UpdateBodySize(bodySizeScale);
    }

    private bool CanLevelUp() {

        int expRequired = GetCurrentLevelData().GetExpRequired();

        return expProgress >= expRequired;  // Enough EXP to Upgrade
    }

    public void ResetLevel() {
        
        currentLevel = 1;
        expProgress = 0;

        UpdateBodySize();
    }


    public void LevelUp() {

        currentLevel += 1;
        
        if (!characterStatsSO.IsOverLevelList(currentLevel)) {
            // If current level having data for setup

            UpdateBodySize();
            UpAttackSize();
        }
    }

    public void AddExp(int expGet) {

        expProgress += expGet;

        if (CanLevelUp()) {

            expProgress -= characterStatsSO.GetCharLevelData(currentLevel).GetExpRequired();
            LevelUp();
        }
    }
    
    public int GetExpReward() {
        return characterStatsSO.GetCharLevelData(currentLevel).GetExpReward();
    }

    #endregion
}
