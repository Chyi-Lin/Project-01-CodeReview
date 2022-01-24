using TMPro;
using UnityEngine;

public class PlayerCoinUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;

    private void Start()
    {
        InitPlayerStat();
    }

    private void OnEnable()
    {
        PowerUpsSystem.OnReflashCoinsEvent += SetCoin;
    }

    private void OnDisable()
    {
        PowerUpsSystem.OnReflashCoinsEvent -= SetCoin;
    }

    private void InitPlayerStat()
    {
        if (PlayerStat.Instance == null)
            return;

        SetCoin(PlayerStat.Instance.coin);
    }

    private void SetCoin(int value)
    {
        coinText.SetText(value.ToString());
    }

    public void Reflesh()
    {
        InitPlayerStat();
    }

}
