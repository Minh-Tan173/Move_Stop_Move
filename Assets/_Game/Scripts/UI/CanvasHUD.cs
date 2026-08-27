using TMPro;
using UnityEngine;

public class CanvasHUD : UICanvas
{
    [SerializeField] private TextMeshProUGUI aliveLeftText;
    [SerializeField] private Setting setting;

    public override void SetUp() {

        setting.OnInit(this);
    }

    public void UpdateAliveLeftText(int aliveLeftValue) {

        aliveLeftText.text = $"Alive: {aliveLeftValue}";
    }
}
