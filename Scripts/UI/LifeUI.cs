using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [Header("Life UI")]
    [SerializeField]
    private TMP_Text lifeText;

    [SerializeField]
    private Image lifeImage;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = .25f;

    [SerializeField]
    private Vector3 changeScale = Vector3.one * 1.2f;

    [Header("Warning Animation")]
    [SerializeField]
    private int warningLife = 1;

    [SerializeField]
    private float warningDuraction = .5f;

    [SerializeField]
    private Vector3 warningScale = Vector3.one * 1.2f;

    [SerializeField]
    private Color warningColor = Color.red;

    private Color originColor;

    private bool isWarning = false;

    private GameSpawner gameSpawner;

    private void Awake()
    {
        gameSpawner = FindObjectOfType<GameSpawner>();
    }

    private void Start()
    {
        SetLifeText(GameStat.currentRing);
    }

    private void OnEnable()
    {
        RingTarget.OnChangeRingAmountEvent += SetLifeText;
        RingTarget.OnChangeRingAmountEvent += LifeWarningAnimation;

        gameSpawner.OnRingAmountChangeEvent += SetLifeText;
    }

    private void OnDisable()
    {
        RingTarget.OnChangeRingAmountEvent -= SetLifeText;
        RingTarget.OnChangeRingAmountEvent -= LifeWarningAnimation;

        gameSpawner.OnRingAmountChangeEvent -= SetLifeText;
    }

    private void OnDestroy()
    {
        // Scale Animation
        TweenUI.CheckSameSequenceIsPlayAndDone(lifeText.rectTransform);
        TweenUI.CheckSameSequenceIsPlayAndDone(lifeImage.rectTransform);
        // Color Animation
        TweenUI.CheckSameSequenceIsPlayAndDone(lifeText);
        TweenUI.CheckSameSequenceIsPlayAndDone(lifeImage);
    }

    private void SetLifeText(int life)
    {
        lifeText.SetText(life.ToString());

        // Scale Animation
        TweenUI.TweenScale(lifeText.rectTransform, changeScale, changeDuraction, DG.Tweening.Ease.Linear);
        TweenUI.TweenScale(lifeImage.rectTransform, changeScale, changeDuraction, DG.Tweening.Ease.Linear);
    }

    private void LifeWarningAnimation(int life)
    {
        if(life <= warningLife)
        {
            // Warning
            if (isWarning)
                return;

            isWarning = true;

            // Scale Animation
            TweenUI.TweenScale(lifeText.rectTransform, warningScale, warningDuraction, DG.Tweening.Ease.Linear, -1);
            TweenUI.TweenScale(lifeImage.rectTransform, warningScale, warningDuraction, DG.Tweening.Ease.Linear, -1);
            // Color Animation
            TweenUI.TweenTextColor(lifeText, warningColor, warningDuraction, DG.Tweening.Ease.Linear, -1);
            TweenUI.TweenImageColor(lifeImage, warningColor, warningDuraction, DG.Tweening.Ease.Linear, -1);
        }
        else
        {
            // Safe
            if (!isWarning)
                return;

            isWarning = false;

            // Scale Animation
            TweenUI.CheckSameSequenceIsPlayAndDone(lifeText.rectTransform);
            TweenUI.CheckSameSequenceIsPlayAndDone(lifeImage.rectTransform);
            // Color Animation
            TweenUI.CheckSameSequenceIsPlayAndDone(lifeText);
            TweenUI.CheckSameSequenceIsPlayAndDone(lifeImage);
        }
    }
}
