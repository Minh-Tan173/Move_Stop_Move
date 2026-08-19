using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCharacter : MonoBehaviour
{
    [SerializeField] private CharacterBase character;

    [Header("Name Tag")]
    [SerializeField] private RectTransform nameTag;
    [SerializeField] private TextMeshProUGUI nameCharText;
    [SerializeField] private Image indexCharImage;
    [SerializeField] private TextMeshProUGUI indexCharText;

    public void SetName(string name) {

        nameCharText.text = $"{name}";
    }

    public void SetIndex(int indexChar) {

        indexCharText.text = $"{indexChar}";
    }

    public void ShowNameTag() {
        nameTag.gameObject.SetActive(true);
    }

    public void HideNameTag() {
        nameTag.gameObject.SetActive(false);
    }

    public Sprite GetIndexCharSprite() {
        return indexCharImage.sprite;
    }

    public string GetIndexCharText() {
        return indexCharText.text;
    }

    public Vector3 GetNametagPosition() {
        return nameTag.position;
    }
}
