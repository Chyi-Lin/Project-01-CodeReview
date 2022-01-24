using System.Collections.Generic;
using UnityEngine;

public class CustomizeColor : MonoBehaviour
{
    [SerializeField]
    private ColorType colorSelected;

    [SerializeField]
    private CustomizeColorData customizeColorData;

    [SerializeField]
    private List<ColorToggle> colorToggles;

    public ColorType GetColorSelected => colorSelected;

    private void Start()
    {
        if (PlayerStat.Instance != null)
            colorSelected = PlayerStat.Instance.selectedColors;

        int colorTotal = customizeColorData.colorDatas.Count - 1;

        for (int i = 0; i < colorToggles.Count; i++)
        {
            if (colorTotal < i)
                break;

            // Read player selected colors
            if (PlayerStat.Instance != null)
                colorToggles[i].SetColorData(this, customizeColorData.colorDatas[i], 
                    (PlayerStat.Instance.selectedColors & customizeColorData.colorDatas[i].colorType) == customizeColorData.colorDatas[i].colorType, 
                    true);
            else
                colorToggles[i].SetColorData(this, customizeColorData.colorDatas[i], true, true);

            colorToggles[i].OnToggleEvent += ColorToggleListener;
        }
    }

    private void ColorToggleListener(ColorData colorData, bool toggle)
    {
        if(toggle)
            colorSelected |= colorData.colorType;
        else
            colorSelected &= ~colorData.colorType;

        // Write to player selected colors
        if (PlayerStat.Instance != null)
            PlayerStat.Instance.selectedColors = colorSelected;
    }
}
