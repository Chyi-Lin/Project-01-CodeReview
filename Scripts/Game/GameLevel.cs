using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Level", menuName = "Ring Toss Game/Create New Level", order = 51)]
public class GameLevel : ScriptableObject
{
    [SerializeField]
    private int id;

    [Header("Level")]
    [SerializeField]
    private int levelIndex;

    [SerializeField]
    private int levelTime = 120;

    [Header("Traget")]
    [SerializeField]
    private int targetAmount;

    [SerializeField]
    private int targetScore = 1;

    [SerializeField]
    private int targetCompleteScore = 5;

    [Header("Environment")]
    [SerializeField]
    private AssetReference levelPrefab;

    [SerializeField]
    private Color levelBackgroundColor;

    [SerializeField]
    private Color levelTargetColor;

    public int GetLevelIndex { get { return levelIndex; } }

    public int GetLevelTime { get { return levelTime; } }

    public int GetTargetAmount { get { return targetAmount; } }

    public int GetTargetScore { get { return targetScore; } }

    public int GetTargetCompleteScore { get { return targetCompleteScore; } }

    public AssetReference GetLevelPrefab { get { return levelPrefab; } }

    public Color GetBackgroundColor { get { return levelBackgroundColor; } }

    public Color GetTargetColor { get { return levelTargetColor; } }
}
