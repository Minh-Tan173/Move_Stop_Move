using UnityEngine;
using System.Collections.Generic;

public class ShopCategory : MonoBehaviour
{
    [SerializeField] private List<ShopTab> shopTabList;

    public void SelectedTab(ShopTab shopTab) {
        shopTab.SetAlphaImage(0f);
    }

    public void UnSelecedTab(ShopTab shopTab) {
        shopTab.SetAlphaImage(0.7f);
    }

    public void SelectedFirstTab() {
        shopTabList[0].ClickTab();
    }

    public void UpdateVisualTab(ShopTab tabSelected) {

        foreach (ShopTab shopTab in shopTabList) {

            if (shopTab == tabSelected) {

                SelectedTab(shopTab);
            }
            else {

                UnSelecedTab(shopTab);
            }
        }
    }
}
