using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField]
    private TMP_Text scoreText;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = .25f;

    [SerializeField]
    private Vector3 changeScale = Vector3.one * 1.25f;

    private void Start()
    {
        SetScoreText(0);
    }

    private void OnEnable()
    {
        RingTarget.OnChangeScoreEvent += SetScoreText;
    }

    private void OnDisable()
    {
        RingTarget.OnChangeScoreEvent -= SetScoreText;
    }

    private void SetScoreText(int score)
    {
        scoreText.SetText(score.ToString());

        // Scale Animation
        TweenUI.TweenScale(scoreText.rectTransform, changeScale, changeDuraction);
    }
}
