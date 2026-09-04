using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BSTSecretAbilitySO : BoosterSO {

    private void ShowBotAttackRange(CharacterBase character, bool isShow) {

        if (character is Bot) { return; } // Only can apply for Player

        List<CharacterBase> botList = CharacterManager.Instance.GetActiveCharacterList();
        botList.Remove(character); // Remove Player out of list

        if (isShow) {

            foreach (CharacterBase bot in botList) {
                bot.GetAttackRangeVisual().Show();
            }
        }
        else {

            foreach (CharacterBase bot in botList) {
                bot.GetAttackRangeVisual().Hide();
            }
        }
    }

    public override void Apply(CharacterBase character, float value) {

        ShowBotAttackRange(character, isShow: true);
    }

    public override void Remove(CharacterBase character, float value) {

        ShowBotAttackRange(character, isShow: false);
    }

    public override string GetDescription(float value) {
        return "";
    }
}
