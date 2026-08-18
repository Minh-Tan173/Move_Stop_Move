using UnityEngine;

[CreateAssetMenu()]
public class BSTAddSpeedSO : BoosterSO {

    public override void Apply(CharacterBase character, float value) {

        character.GetCharacterStats().AddMoveSpeed(value);
    }
}
