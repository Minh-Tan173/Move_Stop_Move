using UnityEngine;

[CreateAssetMenu()]
public class BSTAddSpeedSO : BoosterSO {

    public override void Apply(CharacterBase character, float value) {

        character.GetCharacterStats().AddMoveSpeed(value);
    }

    public override void Remove(CharacterBase character, float value) {

        character.GetCharacterStats().AddMoveSpeed(-value);

    }

    public override string GetDescription(float value) {

        string valueText = value >= 0 ? $"+{value}" : $"{value}";
        return $"Speed {valueText}";
    }
}
