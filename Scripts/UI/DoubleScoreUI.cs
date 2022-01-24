using System;
using TMPro;
using UnityEngine;

public class DoubleScoreUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentCanvas;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text doubleRateText;

    [SerializeField]
    private TMP_Text comboText;

    private EventHandler OnCompelteEvent;

    public Component componenter { get; private set; }

    public void SetPosition(Vector2 position)
    {
        parentCanvas.anchoredPosition = position;
    }

    public void SetDoubleScoreText(int score, float rate, int combo)
    {
        scoreText.SetText(score.ToString());
        

        if(combo > 1)
        {
            doubleRateText.SetText($"x{rate}");
            comboText.SetText($"{combo} COMBO");
        }
        else
        {
            doubleRateText.SetText("");
            comboText.SetText("");
        }
            
    }

    public void SetCompleteEvent(Component componenter, EventHandler OnComplete)
    {
        this.componenter = componenter;
        this.OnCompelteEvent = OnComplete;
    }

    public void OnAnimationComplete()
    {
        parentCanvas.gameObject.SetActive(false);

        OnCompelteEvent?.Invoke(this, EventArgs.Empty);
    }

}
