using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text timeText;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = .25f;

    [SerializeField]
    private Vector3 changeScale = Vector3.one * 1.25f;

    private void Start()
    {
        SetTimeText(GameManager.currentTime);
    }

    private void OnEnable()
    {
        GameManager.OnTimeCountdown += SetTimeText;
        titleText.enabled = true;
        timeText.enabled = true;
    }

    private void OnDisable()
    {
        GameManager.OnTimeCountdown -= SetTimeText;
        titleText.enabled = false;
        timeText.enabled = false;
    }

    private void SetTimeText(int time)
    {
        // No time limit
        if (time == -1)
            enabled = false;

        timeText.SetText(time.ToString());

        // Scale Animation
        if (time <= 10) 
            TweenUI.TweenScale(timeText.rectTransform, changeScale, changeDuraction);
    }
}
