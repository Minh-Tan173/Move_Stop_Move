using TMPro;
using UnityEngine;

public class CanvasLoss : UICanvas
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI killedNotiText;

    public override void SetUp() {

        UpdateKilledText();
    }

    private void UpdateKilledText() {

        Bot killedPlayer = CharacterManager.Instance.GetKilledPlayer();

        if (killedPlayer == null) {

            killedNotiText.text = $"You was killed by {CharacterConst.DEAD_ZONE_NAME}";
            return;
        }

        string nameBotKillPlayer = killedPlayer.GetCanvasCharacter().GetName();
        string indexBotKillPlayer = killedPlayer.GetCanvasCharacter().GetIndexCharText();

        killedNotiText.text = $"You was killed by #{indexBotKillPlayer}. {nameBotKillPlayer}";
    }

    public void RestartGame() {

        UIManager.Instance.CloseUI<CanvasLoss>(0.25f);

        LevelManager.Instance.Invoke(nameof(LevelManager.Instance.OnRestart), 0.3f);
    }
    
    public void ReturnHome() {

    }
}
