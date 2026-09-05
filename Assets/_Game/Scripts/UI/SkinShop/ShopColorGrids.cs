using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopColorGrids : MonoBehaviour
{
    [SerializeField] private ColorSO colorSO;

    [Header("Spawn Color Slot")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contents;
    [SerializeField] private ShopColorSlot colorSlotPrefab;

    private ShopColorSlot currentColorSlot;
    private List<ShopColorSlot> colorSlotList = new List<ShopColorSlot>();

    private void ScrollToIndex(int index) {

        Canvas.ForceUpdateCanvases();

        float itemHeight = colorSlotList[0].GetComponent<RectTransform>().rect.height;

        float spacing = 5f;

        float targetY = index * (itemHeight + spacing);

        float maxScroll = contents.GetComponent<RectTransform>().rect.height - scrollRect.viewport.rect.height;

        if (maxScroll <= 0) { return; }

        scrollRect.verticalNormalizedPosition = 1f - Mathf.Clamp01(targetY / maxScroll);
    }


    private void SetEquippedColor() {

        int equippedID = DataManager.GetGameData().GetPlayerData().EquippedColorID;


        for (int i = 0; i < colorSlotList.Count; i++) {
            
            if (colorSlotList[i].GetColorOfSlot() == (ColorType)equippedID) {
                
                currentColorSlot = colorSlotList[i];
                currentColorSlot.ShowHighlight(true);

                ScrollToIndex(i);

                break;
            }
        }
    }

    public void OnInit() {

        if (colorSlotList.Count == 0) {
            // If color slot list is empty --> Start Spawn Progress

            int totalColor = colorSO.GetTotalColor();

            for (int i = 0; i < totalColor; i++) {

                ColorType colorType = (ColorType)i;

                ShopColorSlot colorSlot = Instantiate(colorSlotPrefab, contents);
                colorSlot.OnInit(this, colorType, colorSO.GetColorWithType(colorType));

                colorSlotList.Add(colorSlot);
            }
        }

        SetEquippedColor();
    }

    public void SelectColorSlot(ShopColorSlot selectedColorSlot) {

        if (currentColorSlot != selectedColorSlot) {

            currentColorSlot.ShowHighlight(false);
        }

        currentColorSlot = selectedColorSlot;
        currentColorSlot.ShowHighlight(true);

        // Apply New Color For Player
        int colorID = (int)selectedColorSlot.GetColorOfSlot();
        DataManager.GetGameData().GetPlayerData().SetEquipColorIndex(colorID);

        Player player = CharacterManager.Instance.GetPlayer();
        player.GetCharacterVisual().ApplySkinColorFor(player);
       
    }
}
