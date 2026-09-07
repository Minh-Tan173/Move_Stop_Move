using UnityEngine;

[CreateAssetMenu()]
public class BSTBonusGold : BoosterSO {

    public override void Apply(CharacterBase character, float value) {

        if (character is Bot) { return; }

        LevelManager.Instance.UpdateGoldReward((int)value);
    }

    public override void Remove(CharacterBase character, float value) {

        if (character is Bot) { return; }

        LevelManager.Instance.UpdateGoldReward(-(int)value);
    }

    public override string GetDescription(float value) {

        return $"Gold + {value}";
    }
}
