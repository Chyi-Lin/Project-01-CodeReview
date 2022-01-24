using UnityEngine;

/// <summary>
/// Recode game level stat
/// </summary>
public class GameStat : MonoBehaviour
{
    //[SerializeField]
    //private int startLevel = 0;
    //public static int currentLevel;
    [SerializeField]
    private BoundedInt levelData;

    [SerializeField]
    private int startScore = 0;
    public static int currentScore;

    [SerializeField]
    private int startHighscore = 0;
    public static int currentHighscore;

    [SerializeField]
    private int startTarget = 3;
    public static int currentTarget;

    [SerializeField]
    private int startRing = 5;
    public static int currentSpawnRing;
    public static int currentRing;

    [Header("Reward")]
    [SerializeField]
    private int startRewardRing = 1;
    public static int currentRewardRing;

    public static int currentGetCoin;

    private void Awake()
    {
        InitStat();

        InitPlayerStat();
    }

    private void OnDestroy()
    {
        RecodeStst();

        // Avoid static stat to cause achievements
        InitStat();
    }

    private void RecodeStst()
    {
        //PlayerPrefs.SetInt($"{GameKey.LEVEL_HIGHSCORE}-{currentLevel}", currentHighscore);
        PlayerPrefs.SetInt($"{GameKey.LEVEL_HIGHSCORE}-{levelData.Value}", currentHighscore);
        PlayerPrefs.Save();
    }

    private void InitStat()
    {
        levelData.InitValue();

        //currentLevel = startLevel;
        currentScore = startScore;
        currentHighscore = startHighscore;
        currentTarget = startTarget;
        currentRing = 0;
        currentSpawnRing = startRing;

        // Reward
        currentRewardRing = startRewardRing;

        // Recode
        currentGetCoin = 0;
    }

    private void InitPlayerStat()
    {
        if (PlayerStat.Instance == null)
            return;

        PlayerStat playerStat = PlayerStat.Instance;

        // Rings
        currentSpawnRing = playerStat.amount + playerStat.increaseAmount;

        // Reward
        currentRewardRing = playerStat.rewardRing;
    }

}
