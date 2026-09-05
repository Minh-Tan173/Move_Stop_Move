using System.Collections.Generic;
using UnityEngine;

public enum ColorType {
    White = 0,
    Yellow = 1,
    Red = 2,
    Purple = 3
}

[CreateAssetMenu()]
public class ColorSO : ScriptableObject
{
    [SerializeField] private List<ColorData> colorList;

    private Dictionary<ColorType, ColorData> colorDict = new Dictionary<ColorType, ColorData>();

    private ColorData GetColorData(ColorType colorType) {


        if (!colorDict.ContainsKey(colorType)) {

            foreach (ColorData colorData in colorList) {

                if (colorData.IsSameColorType(colorType)) {

                    colorDict.Add(colorType, colorData);
                    break;
                }
            }
        }

        return colorDict[colorType];
    }

    public Color GetColorWithType(ColorType colorType) {

        return GetColorData(colorType).GetColor();
    }

    public int GetTotalColor() {
        return colorList.Count;
    }
}

[System.Serializable]
public class ColorData {

    [SerializeField] private ColorType colorType;
    [SerializeField] private Color color;

    public bool IsSameColorType(ColorType colorType) {
        return this.colorType == colorType;
    } 

    public Color GetColor() {
        return this.color;
    }
}