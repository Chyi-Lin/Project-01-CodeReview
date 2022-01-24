using UnityEngine;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField]
    private PowerUpsSystem powerUpsSystem;

    [SerializeField]
    private PowerUpsItemUI[] powerUpsItemUIs;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < powerUpsItemUIs.Length; i++)
        {
            powerUpsItemUIs[i].SetDetail(powerUpsSystem.GetPowerUpsItems[i], 
                powerUpsSystem.GetPowerUpsLevel(powerUpsSystem.GetPowerUpsItems[i].GetID));
            powerUpsItemUIs[i].OnBuyEvent += ShopUI_OnBuyEvent;
        }
    }

    public void RefleshItem()
    {
        if (PlayerStat.Instance == null)
            return;

        for (int i = 0; i < powerUpsItemUIs.Length; i++)
        {
            powerUpsItemUIs[i].SetDetail(powerUpsSystem.GetPowerUpsItems[i],
                powerUpsSystem.GetPowerUpsLevel(powerUpsSystem.GetPowerUpsItems[i].GetID));
            powerUpsItemUIs[i].CheckPlayerCanBuy(PlayerStat.Instance);
        }
    }


    private void ShopUI_OnBuyEvent(PowerUpsItem item, int level)
    {
        powerUpsSystem.BuyPowerUps(item, level);

        RefleshItem();
    }

}
