using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ColorType
{
    // Decimal              // Binary
    //Nothing               // 0000 0000
    Red = (1 << 0),         // 0000 0001
    Orange = (1 << 1),      // 0000 0010
    Yellow = (1 << 2),
    Green = (1 << 3),
    Cyan = (1 << 4),
    Blue = (1 << 5),
    Purple = (1 << 6),
}

[Serializable]
public struct ColorData
{
    public ColorType colorType;
    public Color color;
}

[CreateAssetMenu(fileName = "New Color File", menuName = "Ring Toss Game/Create Customize/Create New Color Data", order = 51)]
public class CustomizeColorData : ScriptableObject
{
    public List<ColorData> colorDatas;

    public List<Color> GetSelectedColors(ColorType colorSelected)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < colorDatas.Count; i++)
        {
            if((colorSelected & colorDatas[i].colorType) == colorDatas[i].colorType)
            {
                colors.Add(colorDatas[i].color);
            }
        }

        return colors;
    }
}
