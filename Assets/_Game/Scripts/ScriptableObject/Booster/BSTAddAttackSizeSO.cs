using UnityEngine;

[CreateAssetMenu()]
public class BSTAddAttackSizeSO : BoosterSO {
    
    public override void Apply(CharacterBase character, float value) {
        character.GetCharacterStats().AddAttackSize(value);   
    }

    public override void Remove(CharacterBase character, float value) {
        character.GetCharacterStats().AddAttackSize(-value);
    }
}
