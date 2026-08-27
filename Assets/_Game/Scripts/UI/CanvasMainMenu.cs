using TMPro;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{

    [Header("Elemens")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI cointText;
    [SerializeField] private Setting setting;
    


    #region Warning Text
    private const string INVALID_NAME = "Name is Invalid";
    private const string LETTER_REQUIRED = "Letter Required";
    #endregion

    #region Default Player Name
    private const string EMPTY_NAME = "Player";
    #endregion

    private string playerName;

    private void ShowWarningText(string warning) {

        warningText.gameObject.SetActive(true);

        warningText.text = warning;

        Invoke(nameof(HideWarningText), 0.7f);
    }

    private void HideWarningText() {

        warningText.gameObject.SetActive(false);
    }

    private void UpdateCointText() {
        cointText.text = $"{DataManager.GetGameData().GetPlayerData().CurrentGold}";
    }

    private bool IsInputTextValid(string value) {


        bool hasChar = false;

        foreach (char character in value) {

            bool isEnglishLetter = character >= 'A' && character <= 'Z' || character >= 'a' && character <= 'z';

            bool isNumber = character >= '0' && character <= '9';

            bool isSpace = character == ' ';

            if (!isEnglishLetter && !isNumber && !isSpace) {

                ShowWarningText(INVALID_NAME);

                return false;
            }

            if (isEnglishLetter) {
                hasChar = true;
            }
        }

        if (!hasChar) {

            ShowWarningText(LETTER_REQUIRED);
            return false;
        }

        return true;
    }

    public override void SetUp() {

        UpdateCointText();
        HideWarningText();

        setting.OnInit(this);
    }

    public void PlayGame() {
        
        if (IsInputTextValid(inputField.text)) {

            playerName = string.IsNullOrEmpty(inputField.text) ? $"{EMPTY_NAME}" : inputField.text;

            UIManager.Instance.CloseUI<CanvasMainMenu>(0.5f);

            LevelManager.Instance.OnPlay();
        }
        
    }

    public void OnSkinShop() {

        UIManager.Instance.CloseUI<CanvasMainMenu>(0.25f);
        UIManager.Instance.OpenUI<CanvasSkinShop>();
    }

    public void ResetInputField() {
        inputField.text = null;
    }

    public string GetPlayerName() {
        return playerName;
    }
}
