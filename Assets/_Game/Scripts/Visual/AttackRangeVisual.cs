using UnityEngine;

public class AttackRangeVisual : MonoBehaviour
{
    [SerializeField] private CharacterBase character;

    private Transform attackRangeTF;

    public Transform AttackRangeTF => attackRangeTF != null ? attackRangeTF : (attackRangeTF = transform);

    public void OnInit() {

        if (character is Player) {
            Show();
        }
        else {
            Hide();
        }
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    public void UpdateVisual() {

        float size = character.GetCharacterCombat().GetTrueAttackRange();
        AttackRangeTF.localScale = Vector3.one * size;
    }
}
