using TMPro;
using UnityEngine;

public class CanvasWin : UICanvas
{
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private ScoreBoard scoreBoard;

    public override void SetUp() {

        PlayPartical();

        scoreBoard.UpdateKillScore();
    }

    private void PlayPartical() {

        StopPartical();
        particleSystem.Play();

        Invoke(nameof(StopPartical), 2.5f);

    }

    private void StopPartical() {

        particleSystem.Stop();
        particleSystem.Clear();
    }

    public void NextLevel() {

        UIManager.Instance.CloseUI<CanvasWin>(0.25f);

        LevelManager.Instance.Invoke(nameof(LevelManager.Instance.SwitchToNextLevel), 0.5f);
    }

    public void Home() {

        UIManager.Instance.CloseUI<CanvasLoss>(0.25f);

        LevelManager.Instance.BackToMainMenu();
    }
}
