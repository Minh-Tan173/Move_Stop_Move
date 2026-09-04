using UnityEngine;

[CreateAssetMenu()]
public class BSTAddAttackSizeSO : BoosterSO {
    
    public override void Apply(CharacterBase character, float value) {
        character.GetCharacterStats().AddAttackSize(value);   
    }

    public override void Remove(CharacterBase character, float value) {
        character.GetCharacterStats().AddAttackSize(-value);
    }

    public override string GetDescription(float value) {

        string valueText = value >= 0 ? $"+{value}" : $"{value}";
        return $"Range {value}";
    }
}
