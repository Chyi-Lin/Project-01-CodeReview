using System.Collections.Generic;
using UnityEngine;

public class GameScoreCombo : MonoBehaviour
{
    [Header("Combo Reward")]
    [SerializeField]
    private int startCombo = 0;
    public static int currentCombo;

    [SerializeField]
    private int maxCombo = 99;
    public static int currentMaxCombo;

    [SerializeField]
    private float startComboRatio = .1f;
    public static float currentComboRatio;

    [SerializeField]
    private float StartMaxComboRatio = 2f;
    public static float currentMaxComboRatio;

    public static int currentMaximumCombo = 0;

    [SerializeField]
    private float comboTime = 2.5f;
    private float currentComboTime;

    [Header("Combo UI")]
    [SerializeField]
    private DoubleScoreUIPool doubleScoreUI;

    private Dictionary<Component, Queue<DoubleScoreUIPool.UIData>> componetnAndUIPairs = new Dictionary<Component, Queue<DoubleScoreUIPool.UIData>>();

    [SerializeField]
    private Vector2 offset;

    public static float GetCalculateDoubleRate
    {
        get
        {
            if (currentCombo <= 1)
                return 1f;

            float ratio = (float)currentCombo * currentComboRatio + 1f;

            if (ratio >= currentMaxComboRatio)
                return currentMaxComboRatio;
            else
                return ratio;
        }
    }

    public static int GetCalculateDoubleScore(int originScore)
    {
        return Mathf.RoundToInt(originScore * GetCalculateDoubleRate);
    }

    private void SetDoubleScoreUI(Component component, Vector2 position, int score)
    {
        DoubleScoreUIPool.UIData uIData = doubleScoreUI.DequeueUI();
        uIData.doubleScoreUI.SetDoubleScoreText(score, GetCalculateDoubleRate, currentCombo);
        uIData.doubleScoreUI.SetCompleteEvent(component, ScoreUIAnimationComplete);

        // Calculation UI position
        Vector2 finalPosition = position + offset;
        if(componetnAndUIPairs.ContainsKey(component))
            finalPosition.y += offset.y * componetnAndUIPairs[component].Count;
        uIData.doubleScoreUI.SetPosition(finalPosition);

        if (componetnAndUIPairs.ContainsKey(component))
        {
            componetnAndUIPairs[component].Enqueue(uIData);
        }
        else
        {
            Queue<DoubleScoreUIPool.UIData> uIDatas = new Queue<DoubleScoreUIPool.UIData>();
            componetnAndUIPairs.Add(component, uIDatas);
            componetnAndUIPairs[component].Enqueue(uIData);
        }
    } 

    private void ScoreUIAnimationComplete(object sender, System.EventArgs eventArgs)
    {
        DoubleScoreUI scoreUI = (DoubleScoreUI)sender;
        componetnAndUIPairs[scoreUI.componenter].Dequeue();
    }

    private void Awake()
    {
        currentCombo = startCombo;
        currentComboRatio = startComboRatio;
        currentMaxCombo = maxCombo;
        currentMaxComboRatio = StartMaxComboRatio;
        currentComboTime = comboTime;

        // Recode
        currentMaximumCombo = 0;
    }

    private void OnEnable()
    {
        RingTarget.OnEnterEvent += ComboTimeRecovery;
        RingTarget.OnChangeScoreUIEvent += SetDoubleScoreUI;
    }

    private void OnDisable()
    {
        RingTarget.OnEnterEvent -= ComboTimeRecovery;
        RingTarget.OnChangeScoreUIEvent -= SetDoubleScoreUI;
    }

    private void Update()
    {
        if (!GameManager.gameIsReady)
            return;

        if (GameManager.gameIsOver)
            return;

        if (currentComboTime <= 0f)
        {
            // Recode
            if (currentMaximumCombo < currentCombo)
                currentMaximumCombo = currentCombo;

            currentCombo = startCombo;
            currentComboRatio = startComboRatio;
            return;
        }
            
        if(currentComboTime > 0f)
        {
            currentComboTime -= Time.deltaTime;
        }
    }

    private void ComboTimeRecovery()
    {
        currentComboTime = comboTime;
        currentCombo++;
    }
}
