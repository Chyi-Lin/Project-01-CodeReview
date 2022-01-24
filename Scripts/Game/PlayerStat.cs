using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Player")]
    public int coin = 0;

    [Header("Game")]
    public int amount = 5;

    public int rewardRing = 1;

    [Header("Power UP's")]
    public int increaseAmount;

    public float highLevelRate;

    public float rewardRate;

    public float energyRecoverySpeed;

    [Header("Recode")]
    public int completeTargets = 0;

    [Header("Customize")]
    public ColorType selectedColors;

    #region Singleton Pattern

    public static PlayerStat Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        ReadPlayerStat();
    }

    #endregion // Singleton Pattern

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            return;

        SavePlayerStat();
    }

    private void OnApplicationQuit()
    {
        SavePlayerStat();
    }

    private void ReadPlayerStat()
    {
        if (!PlayerPrefs.HasKey(GameKey.PLAYER_STAT))
            return;

        string json = PlayerPrefs.GetString(GameKey.PLAYER_STAT);
        PlayerStatData data = JsonUtility.FromJson<PlayerStatData>(json);

        coin = data.coin;
        amount = data.amount;
        rewardRing = data.rewardRing;
        completeTargets = data.completeTargets;
        selectedColors = (ColorType)data.selectedColors;
    }

    public void SavePlayerStat()
    {
        PlayerStatData statData = new PlayerStatData();
        statData.coin = coin;
        statData.amount = amount;
        statData.rewardRing = rewardRing;
        statData.completeTargets = completeTargets;
        statData.selectedColors = selectedColors.GetHashCode();

        string json = JsonUtility.ToJson(statData);
        //Debug.Log(json);

        PlayerPrefs.SetString(GameKey.PLAYER_STAT, json);
        PlayerPrefs.Save();
    }


    [ContextMenu("Delete All")]
    private void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
