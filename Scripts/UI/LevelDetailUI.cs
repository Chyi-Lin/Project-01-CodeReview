using TMPro;
using UnityEngine;

public class LevelDetailUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text maxComboText;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private TMP_Text ringsText;

    [SerializeField]
    private TMP_Text coinText;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        maxComboText.SetText(GameScoreCombo.currentMaximumCombo.ToString());
        timeText.SetText(GameManager.currentTime.ToString());
        ringsText.SetText(GameStat.currentRing.ToString());
        coinText.SetText(GameStat.currentGetCoin.ToString());
    }

}
