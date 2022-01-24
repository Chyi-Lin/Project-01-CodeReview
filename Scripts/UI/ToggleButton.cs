using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public event Action<bool> OnToggleEvent;

    [SerializeField]
    private bool isOn = false;

    [SerializeField]
    private Sprite onSprite, offSprite;

    [SerializeField]
    private Button toggleButton;

    public void SetToggle(bool toggle)
    {
        isOn = toggle;

        if (toggle)
            toggleButton.image.sprite = onSprite;
        else
            toggleButton.image.sprite = offSprite;
    }

    private void OnEnable()
    {
        toggleButton.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        toggleButton.onClick.RemoveListener(Toggle);
    }

    private void Toggle()
    {
        isOn = !isOn;

        if (isOn)
            toggleButton.image.sprite = onSprite;
        else
            toggleButton.image.sprite = offSprite;

        OnToggleEvent?.Invoke(isOn);
    }

}
