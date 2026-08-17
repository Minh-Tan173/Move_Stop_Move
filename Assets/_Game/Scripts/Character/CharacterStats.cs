using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected AttackRangeVisual attackRangeVisual;

    [Header("Attack Range")]
    [SerializeField] private float arDefaultSize;
    [SerializeField] private float attackCD;

    private float attackRange;

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
        SetAttackSize(arDefaultSize);
    }

    public float GetAttackRange() {
        return this.attackRange;
    }
    #endregion

    #region Attack CD
    public float GetAttackCD() {
        return this.attackCD;
    }
    #endregion
}

