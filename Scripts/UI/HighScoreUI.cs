using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [Header("High Score UI")]
    [SerializeField]
    private TMP_Text scoreText;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = .25f;

    [SerializeField]
    private Vector3 changeScale = Vector3.one * 1.25f;

    private GameSpawner gameSpawner;

    private void Awake()
    {
        gameSpawner = FindObjectOfType<GameSpawner>();
    }

    private void Start()
    {
        SetScoreText(0);
    }

    private void OnEnable()
    {
        gameSpawner.OnLevelInitCompleteEvent += InitHighScore;
        RingTarget.OnChangeScoreEvent += SetScoreText;
    }

    private void OnDisable()
    {
        RingTarget.OnChangeScoreEvent -= SetScoreText;
        gameSpawner.OnLevelInitCompleteEvent -= InitHighScore;
    }

    private void InitHighScore()
    {
        SetScoreText(GameStat.currentHighscore);
    }

    private void SetScoreText(int score)
    {
        if (GameStat.currentHighscore > score)
            return;
        
        GameStat.currentHighscore = score;
        scoreText.SetText(GameStat.currentHighscore.ToString());

        // Scale Animation
        TweenUI.TweenScale(scoreText.rectTransform, changeScale, changeDuraction);
    }
}
