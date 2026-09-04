using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected AttackRangeVisual attackRangeVisual;

    [Header("Ref")]
    [SerializeField] private UpgradeVFX upgradeVFX;

    [Header("Data")]
    [SerializeField] private CharacterStatsSO characterStatsSO;

    // Player Bonus
    private const float playerAttackRangeBonus = 1f;
    private float playerMoveSpeedBonus = 0.25f;
    private float playerExpMultiplier = 1.15f;

    private float attackRange;
    private float baseAttackRange;

    private float moveSpeed;

    private int currentLevel;
    private int expProgress;
    private float bodySizeScale;

    private CharacterBase character;

    public void OnInit(CharacterBase character) {

        this.character = character;

        ResetMoveSpeed();
        ResetAttackSize();
        ResetLevel();
    }

    #region Attack Range
    public void SetAttackSize(float value) {

        float oldTrueRange = character.GetCharacterCombat().GetTrueAttackRange();

        baseAttackRange = value;
        attackRange = baseAttackRange;

        if (character is Player) {

            attackRange += playerAttackRangeBonus;
        }

        attackRangeVisual.UpdateVisual();

        float newTrueRange = character.GetCharacterCombat().GetTrueAttackRange();

        if (character is Player) {

            CameraManager.Instance.UpdateZoom(newTrueRange, oldTrueRange);
        }
    }

    public void AddAttackSize(float value) {
        SetAttackSize(baseAttackRange + value);
    }

    public void UpAttackSize() {
        SetAttackSize(baseAttackRange + 1);
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

        float bonusSpeed = character is Player ? playerMoveSpeedBonus : 0f;
        SetMoveSpeed(characterStatsSO.GetMoveSpeed() + bonusSpeed);
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

        character.UpdateBodySize(bodySizeScale);
    }

    private bool CanLevelUp() {

        int expRequired = GetCurrentLevelData().GetExpRequired();

        return expProgress >= expRequired;  // Enough EXP to Upgrade
    }

    private void ApplyLevelData() {
     
        UpdateBodySize();
        UpAttackSize();
    }

    public void ResetLevel() {
        
        currentLevel = 1;
        expProgress = 0;

        UpdateBodySize();
    }

    private int GetExpBonus(int exp) {

        if (character is Player) {

            return Mathf.RoundToInt(exp * playerExpMultiplier);
        }

        return exp;
    }

    public void SetSpawnLevel(int level) {

        currentLevel = 1;
        expProgress = 0;

        while (currentLevel < level) {

            currentLevel += 1;

            if (!characterStatsSO.IsOverLevelList(currentLevel)) {

                ApplyLevelData();
            }
        }
    }

    public void LevelUp() {

        if (character.IsDead()) { return; }

        currentLevel += 1;

        if (!characterStatsSO.IsOverLevelList(currentLevel)) {
            // If current level having data for setup

            ApplyLevelData();
        }

        // Active Immortal State
        float immortalDuration = characterStatsSO.GetImmortalDuration();
        character.TriggerImmortal(immortalDuration);

        upgradeVFX.PlayVFX(immortalDuration, bodySizeScale);
    }

    public void AddExp(int expGet) {

        int finalExp = GetExpBonus(expGet);
        expProgress += finalExp;

        character.GetCanvasCharacter().ShowEXPGain(finalExp);

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
