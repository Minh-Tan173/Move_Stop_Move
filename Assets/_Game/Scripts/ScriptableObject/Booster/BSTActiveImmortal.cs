using UnityEngine;

[CreateAssetMenu]
public class BSTActiveImmortal : BoosterSO {

    public override void Apply(CharacterBase character, float value) {

        character.TriggerAccessoryImmortal(value);

    }

    public override void Remove(CharacterBase character, float value) {

        character.CancelImmortal();
    }

    public override string GetDescription(float value) {
        return $"Immortal";
    }
}
