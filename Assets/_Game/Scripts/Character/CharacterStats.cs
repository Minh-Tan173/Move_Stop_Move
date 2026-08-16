using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Attack Range")]
    [SerializeField] private float attackRange;
    [SerializeField] protected float arDefaultSize;

    [SerializeField] private float attackCD;

    #region Attack Range

    public void SetAttackSize(float value) {
        attackRange = value;
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

