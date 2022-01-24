using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArchievementItemUI : MonoBehaviour
{
    private IArchievementData data;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text introductionText;

    [SerializeField]
    private TMP_Text rewardText;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private CanvasGroupUI completeUI;

    [Header("Progress Bar")]
    [SerializeField]
    private GameObject progressBar;

    [SerializeField]
    private Image progress;

    [SerializeField]
    private TMP_Text progressText;

    public void SetContent(IArchievementData archievementData)
    {
        data = archievementData;

        //titleText.SetText(data.GetTitle);
        //introductionText.SetText(data.GetIntro);

        titleText.GetComponent<LocalizedText>().key = data.GetTitleKey;
        titleText.GetComponent<LocalizedText>().Reflush();
        introductionText.GetComponent<LocalizedText>().key = data.GetDescriptionkey;
        introductionText.GetComponent<LocalizedText>().Reflush();

        rewardText.SetText(data.GetRewardCoins.ToString());
        iconImage.sprite = data.GetIcon;

        rewardButton.onClick.AddListener(data.ReceiveReward);
        rewardButton.onClick.AddListener(GetReward);
    }

    public void AddRewardButtonEvent(UnityAction unityAction)
    {
        if (unityAction == null)
            return;

        rewardButton.onClick.AddListener(unityAction);
    }

    public void RemoveRewardButtonEvent(UnityAction unityAction)
    {
        if (unityAction == null)
            return;

        rewardButton.onClick.RemoveListener(unityAction);
    }

    public void SetRewardButton(bool active)
    {
        rewardButton.gameObject.SetActive(active);
    }

    public void SetComplete(bool isComplete)
    {
        if (isComplete)
            completeUI.Show();
        else
            completeUI.Hide();
    }

    public void SetProgress(int value, int max)
    {
        value = value < max ? value : max;

        progress.fillAmount = value / (float)max;
        progressText.SetText($"{value}/{max}");
    }

    private void GetReward()
    {
        if (data == null)
            return;

        if (!data.IsReceiveReward())
            return;

        if (PlayerStat.Instance == null)
            return;

        PlayerStat.Instance.coin += data.GetRewardCoins;

        SetComplete(true);
    }

}
