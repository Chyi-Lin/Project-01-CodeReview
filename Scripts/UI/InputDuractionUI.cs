using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputDuractionUI : MonoBehaviour
{
    [System.Serializable]
    public struct InputDuractionColor
    {
        public float durcationValue;
        public Color color;
    }

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private InputDuractionColor[] duractionColors;

    private bool isTouching = false;

    [Header("UI")]
    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private Image duractionImage;

    [SerializeField]
    private Image strengthImage;

    [Header("Warning Animation")]
    [SerializeField]
    private bool isWarning = false;
    
    [SerializeField]
    private float warningAmount = 0.35f;

    [SerializeField]
    private float warningSacle = 1.15f;

    [SerializeField]
    private Color warningColor;

    private void OnEnable()
    {
        gameInput.OnTouchUp += GameInput_OnTouchUp;
        gameInput.OnTouchDown += GameInput_OnTouchDown;
    }

    private void OnDisable()
    {
        gameInput.OnTouchUp -= GameInput_OnTouchUp;
        gameInput.OnTouchDown -= GameInput_OnTouchDown;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
            enabled = false;

        duractionImage.fillAmount = 1f - (gameInput.GetTouchingTime / gameInput.GetInfluenceDuraction);

        if(!isTouching || duractionImage.fillAmount <= 0f)
            strengthImage.fillAmount = Mathf.Lerp(strengthImage.fillAmount, duractionImage.fillAmount, .15f);

        StrengthColor(gameInput.GetStrengthValue / gameInput.GetInfluenceDuraction);

        WarningAniamtion();
    }

    private void GameInput_OnTouchDown()
    {
        isTouching = true;

        strengthImage.fillAmount = 1f - (gameInput.GetTouchingTime / gameInput.GetInfluenceDuraction);
    }

    private void GameInput_OnTouchUp()
    {
        isTouching = false;
    }

    private void StrengthColor(float strength)
    {
        for (int i = duractionColors.Length - 1; i >= 0; i--)
        {
            if (duractionColors[i].durcationValue <= strength)
            {
                strengthImage.color = Color.Lerp(strengthImage.color, duractionColors[i].color, .5f);
                break;
            }
        }
    }

    private void WarningAniamtion()
    {
        if (duractionImage.fillAmount < warningAmount)
        {
            if (!isWarning)
            {
                isWarning = true;
                TweenUI.TweenTextColor(titleText, warningColor, .25f, loops: -1);
                TweenUI.TweenScale(titleText.rectTransform, Vector2.one * warningSacle, .25f, loops: -1);
            }
        }
        else
        {
            if (isWarning)
            {
                isWarning = false;
                TweenUI.CheckSameSequenceIsPlayAndDone(titleText);
                TweenUI.CheckSameSequenceIsPlayAndDone(titleText.rectTransform);
            }
        }
    }

    private void OnDestroy()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText);
        TweenUI.CheckSameSequenceIsPlayAndDone(titleText.rectTransform);
    }

}
