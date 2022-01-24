using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUps File", menuName = "Ring Toss Game/Create Shop Item/Create New PowerUps Item", order = 51)]
public class PowerUpsItem : ScriptableObject
{
    [System.Serializable]
    public struct IncreaseData
    {
        public int level;
        public float increaseValue;
        public int cost;
    }

    [SerializeField]
    private int id;

    [SerializeField]
    private string title;

    [SerializeField, TextArea]
    private string description;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private PowerUpsType type;

    [SerializeField]
    private IncreaseData[] increaseDatas;

    [SerializeField]
    private string title_key;

    [SerializeField]
    private string description_key;

    public int GetID => id;

    public string GetTitle => title;

    public string GetDescription => description;

    public Sprite GetIcon => icon;

    public PowerUpsType GetPowerUpsType => type;

    public IncreaseData[] GetIncreaseDatas => increaseDatas;

    public string GetTitleKey { get { return title_key; } }

    public string GetDescriptionkey { get { return description_key; } }
}
