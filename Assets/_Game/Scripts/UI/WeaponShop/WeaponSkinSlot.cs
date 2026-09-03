using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSkinSlot : MonoBehaviour, IPointerDownHandler
{
    [Header("Visual")]
    [SerializeField] private Image skinImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private GameObject highlightObject;

    private CanvasWeaponShop owner;
    private WeaponItemData weaponData;
    private WeaponSkinData skinData;


    public void OnInit(CanvasWeaponShop owner, WeaponItemData weaponData, WeaponSkinData skinData) {

        this.owner = owner;
        this.weaponData = weaponData;
        this.skinData = skinData;

        skinImage.sprite = skinData.GetItemSprite();

        Refresh();

        SetHighlight(false);
    }

    public void Refresh() {
        SetLock(!weaponData.IsOwnedSkin(skinData.GetItemID()));
    }

    public void OnClickSlot() {
        owner.SelectSkin(this, skinData);
    }

    public void SetHighlight(bool value) {
        highlightObject.SetActive(value);
    }

    public void SetLock(bool isLock) {
        lockImage.enabled = isLock;
    }

    public WeaponSkinData GetSkinData() {
        return skinData;
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnClickSlot();
    }
}
