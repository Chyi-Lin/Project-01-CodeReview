using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsItemUI : MonoBehaviour
{
    [System.Serializable]
    public struct ItemData
    {
        public Image iconImage;
        public TMP_Text titleText;
        public TMP_Text descriptionText;
        public Image[] levelImage;
        public Color normalColor;
        public Color powerUpColor;
    }

    [System.Serializable]
    public struct ButtonData
    {
        public Button button;
        public Image icon;
        public TMP_Text costText;
        public Color enableBuyColor;
        public Color disableBuyColor;
    }

    /// <summary>
    /// Buy complete. 
    /// <para>T1:Power UpsItem</para>
    /// <para>T2:Current Level</para>
    /// </summary>
    public event Action<PowerUpsItem, int> OnBuyEvent;

    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private ButtonData itemButton;

    private PowerUpsItem powerUpsItem;
    private int currentLevel;

    private void OnEnable()
    {
        itemButton.button.onClick.AddListener(OnBuyListener);
    }

    private void OnDisable()
    {
        itemButton.button.onClick.RemoveListener(OnBuyListener);
    }

    public void SetDetail(PowerUpsItem item, int level)
    {
        powerUpsItem = item;
        currentLevel = level;

        itemData.iconImage.sprite = powerUpsItem.GetIcon;
        //itemData.titleText.SetText(powerUpsItem.GetTitle);
        //itemData.descriptionText.SetText(powerUpsItem.GetDescription);

        itemData.titleText.GetComponent<LocalizedText>().key = powerUpsItem.GetTitleKey;
        itemData.titleText.GetComponent<LocalizedText>().Reflush();
        itemData.descriptionText.GetComponent<LocalizedText>().key = powerUpsItem.GetDescriptionkey;
        itemData.descriptionText.GetComponent<LocalizedText>().Reflush();

        SetLevelSpot(currentLevel);

        CheckPowerUpsHasMoreLevel();
    }

    public void CheckPlayerCanBuy(PlayerStat player)
    {
        if (powerUpsItem == null)
            return;

        int nextLevel = currentLevel + 1;
        if (nextLevel >= powerUpsItem.GetIncreaseDatas.Length)
            return;

        if (powerUpsItem.GetIncreaseDatas[nextLevel].cost <= player.coin)
        {
            // Can buy
            itemButton.button.interactable = true;
            itemButton.icon.color = itemButton.enableBuyColor;
            itemButton.costText.color = itemButton.enableBuyColor;
        }
        else
        {
            // Can't buy
            itemButton.button.interactable = false;
            itemButton.icon.color = itemButton.disableBuyColor;
            itemButton.costText.color = itemButton.disableBuyColor;
        }
    }

    private void SetLevelSpot(int currentLevel)
    {
        for (int i = 0; i < itemData.levelImage.Length; i++)
        {
            if(i < currentLevel)
            {
                itemData.levelImage[i].color = itemData.powerUpColor;
            }
            else
            {
                itemData.levelImage[i].color = itemData.normalColor;
            }
        }
    }

    private void CheckPowerUpsHasMoreLevel()
    {
        if (currentLevel < powerUpsItem.GetIncreaseDatas.Length - 1)
        {
            int nextLevel = currentLevel + 1;
            itemButton.button.gameObject.SetActive(true);
            itemButton.costText.SetText(powerUpsItem.GetIncreaseDatas[nextLevel].cost.ToString());
        }
        else
        {
            itemButton.button.gameObject.SetActive(false);
        }
            
    }

    private void OnBuyListener()
    {
        currentLevel++;

        OnBuyEvent?.Invoke(powerUpsItem, currentLevel);

        SetLevelSpot(currentLevel);

        CheckPowerUpsHasMoreLevel();
    }

}
