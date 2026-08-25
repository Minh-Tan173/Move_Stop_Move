using TMPro;
using UnityEngine;

public class CanvasHUD : UICanvas
{
    [SerializeField] private TextMeshProUGUI aliveLeftText;

    public void UpdateAliveLeftText(int aliveLeftValue) {

        aliveLeftText.text = $"Alive: {aliveLeftValue}";
    }
}
