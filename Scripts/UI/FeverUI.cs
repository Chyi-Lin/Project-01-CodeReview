using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeverUI : MonoBehaviour
{
    [SerializeField]
    private GameFever gameFever;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private Image duractionImage;

    [Header("Fever Animation")]
    [SerializeField]
    private float feverSacle = 1.15f;

    [SerializeField]
    private Color feverColor;

    private void OnEnable()
    {
        gameFever.OnFeverStartEvent += StartAniamtion;
        gameFever.OnFeverStopEvent += StopAniamtion;
    }

    private void OnDisable()
    {
        gameFever.OnFeverStartEvent -= StartAniamtion;
        gameFever.OnFeverStopEvent -= StopAniamtion;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
            enabled = false;

        duractionImage.fillAmount = gameFever.GetCurrentFever / gameFever.GetMaxValue;
    }

    private void StartAniamtion()
    {
        TweenUI.TweenTextColor(titleText, feverColor, .25f, loops: -1);
        TweenUI.TweenScale(titleText.rectTransform, Vector2.one * feverSacle, .25f, loops: -1);
    }

    private void StopAniamtion()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText);
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText.rectTransform);
    }

    private void OnDestroy()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText);
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText.rectTransform);
    }

}
