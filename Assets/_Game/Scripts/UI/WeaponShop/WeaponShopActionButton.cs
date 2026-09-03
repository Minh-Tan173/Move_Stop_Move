using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopActionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;


    private CanvasWeaponShop owner;


    public void OnInit(CanvasWeaponShop owner) {
        this.owner = owner;
    }


    public void SetBuy(int price) {
        button.interactable = true;
        buttonText.text = $"BUY {price}";
    }


    public void SetEquip() {
        button.interactable = true;
        buttonText.text = "EQUIP";
    }


    public void SetEquipped() {
        button.interactable = false;
        buttonText.text = "EQUIPPED";
    }


    public void OnClick() {
        owner.OnClickActionButton();
    }
}
