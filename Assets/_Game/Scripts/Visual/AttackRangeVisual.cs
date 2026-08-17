using UnityEngine;

public class AttackRangeVisual : MonoBehaviour
{
    [SerializeField] private CharacterBase character;

    private Transform attackRangeTF;

    public Transform AttackRangeTF => attackRangeTF != null ? attackRangeTF : (attackRangeTF = transform);


    public void UpdateVisual() {

        float size = character.GetTrueAttackRange();
        AttackRangeTF.localScale = Vector3.one * size;
    }
}
