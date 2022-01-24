using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    public event Action<ColorData, bool> OnToggleEvent;

    [SerializeField]
    private Toggle toggleButton;

    [SerializeField]
    private Image colorImage;

    [SerializeField]
    private ColorData colorData;

    private CustomizeColor customizeColor;

    private void OnEnable()
    {
        toggleButton.onValueChanged.AddListener(ToggleChangeListener);
    }

    private void OnDisable()
    {
        toggleButton.onValueChanged.RemoveListener(ToggleChangeListener);
    }

    private void ToggleChangeListener(bool toggle)
    {
        if (!toggle)
        {
            ColorType cur = customizeColor.GetColorSelected;
            cur &= ~colorData.colorType;
            if (cur.GetHashCode() == -128)
            {
                toggleButton.isOn = !toggle;
                return;
            }     
        }

        OnToggleEvent?.Invoke(colorData, toggle);
    }

    public void SetColorData(CustomizeColor customizeColor, ColorData colorData, bool isOn, bool interactable)
    {
        this.customizeColor = customizeColor;
        this.colorData = colorData;
        colorImage.color = colorData.color;

        toggleButton.interactable = interactable;
        toggleButton.isOn = isOn & interactable;
    }
}
