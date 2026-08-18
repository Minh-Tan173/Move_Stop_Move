using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasCharacter : MonoBehaviour
{
    [SerializeField] private CharacterBase character;
    [SerializeField] private TextMeshProUGUI nameChar;

    public void SetName(string name) {

        nameChar.text = $"{nameChar}";
    }
}
