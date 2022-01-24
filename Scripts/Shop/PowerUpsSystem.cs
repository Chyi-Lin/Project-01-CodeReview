using System;
using UnityEngine;

public class PowerUpsSystem : MonoBehaviour
{
    public static event Action<int> OnReflashCoinsEvent;

    [SerializeField]
    private PowerUpsItem[] powerUpsItems;

    public PowerUpsItem[] GetPowerUpsItems => powerUpsItems;

    private PlayerStat player;

    private void Start()
    {
        if (PlayerStat.Instance != null)
            player = PlayerStat.Instance;

        for (int i = 0; i < powerUpsItems.Length; i++)
        {
            SetupPowerUps(powerUpsItems[i]);
        }
    }

    private void SetupPowerUps(PowerUpsItem item)
    {
        if (player == null)
            return;

        int currentLevel = GetPowerUpsLevel(item.GetID);

        switch (item.GetPowerUpsType)
        {
            case PowerUpsType.IncreaseAmount:
                player.increaseAmount = (int)item.GetIncreaseDatas[currentLevel].increaseValue;

                break;
            case PowerUpsType.HighLevelRate:
                player.highLevelRate = item.GetIncreaseDatas[currentLevel].increaseValue;

                break;
            case PowerUpsType.RewardRate:
                player.rewardRate = item.GetIncreaseDatas[currentLevel].increaseValue;

                break;
            case PowerUpsType.EnergyRecoverySpeed:
                player.energyRecoverySpeed = item.GetIncreaseDatas[currentLevel].increaseValue;

                break;
            default:
                break;
        }

    }

    private void SavePowerUpsLevel(int id, int level)
    {
        PlayerPrefs.SetInt(GetPowerUpsKey(id), level);
        PlayerPrefs.Save();
    }

    private string GetPowerUpsKey(int id)
    {
        return $"{GameKey.PLAYER_POWERUPS} - {id}";
    }

    public int GetPowerUpsLevel(int id)
    {
        return PlayerPrefs.GetInt(GetPowerUpsKey(id), 0);
    }


    public void BuyPowerUps(PowerUpsItem item, int level)
    {
        // Double check
        if (item.GetIncreaseDatas[level].cost <= player.coin)
        {
            player.coin -= item.GetIncreaseDatas[level].cost;

            SavePowerUpsLevel(item.GetID, level);
            SetupPowerUps(item);
        }

        // Event
        OnReflashCoinsEvent?.Invoke(player.coin);
    }

}
