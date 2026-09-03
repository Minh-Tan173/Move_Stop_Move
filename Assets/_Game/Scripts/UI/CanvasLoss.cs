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

        string nameBotKillPlayer = CharacterManager.Instance.GetKilledPlayer().GetCanvasCharacter().GetName();
        string indexBotKillPlayer = CharacterManager.Instance.GetKilledPlayer().GetCanvasCharacter().GetIndexCharText();
        killedNotiText.text = $"You was killed by #{indexBotKillPlayer}. {nameBotKillPlayer}";
    }

    public void RestartGame() {
        LevelManager.Instance.OnRestart();
    }
    
    public void ReturnHome() {

    }
}
