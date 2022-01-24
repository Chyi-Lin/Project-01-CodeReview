using UnityEngine;
using Sirenix.OdinInspector;

public abstract class IArchievementData : ScriptableObject
{
    [System.Serializable]
    public enum Type
    {
        NONE = 0,
        ONCE = 5,
        COLLECTION = 6,
    }

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField]
    private int id;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField]
    private string title;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField, TextArea]
    private string introduction;

    [PreviewField(100), HideLabel]
    [HorizontalGroup("Split", 60)]
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int rewardCoins;

    [SerializeField]
    private string title_key;

    [SerializeField]
    private string description_key;

    [Space(16)]

    [SerializeField]
    private Type type;

    public int GetID { get { return id; } }

    public string GetTitle { get { return title; } }

    public string GetIntro { get { return introduction; } }

    public Sprite GetIcon { get { return icon; } }

    public int GetRewardCoins { get { return rewardCoins; } }

    public Type GetArchievementType { get { return type; } }

    public string GetTitleKey { get { return title_key; } }

    public string GetDescriptionkey { get { return description_key; } }

    public abstract bool IsReached();

    public abstract bool IsReceiveReward();

    public abstract void ReceiveReward();
}
