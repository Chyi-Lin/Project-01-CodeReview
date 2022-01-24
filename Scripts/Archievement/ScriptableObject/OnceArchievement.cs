using UnityEngine;

[CreateAssetMenu(fileName = "New Archievement File", menuName = "Ring Toss Game/Create Archievement/Create New Once Archievement", order = 51)]
public class OnceArchievement : IArchievementData
{
    [System.Serializable]
    public enum Key
    {
        ReachedLevel = 5,
    }

    [System.Serializable]
    public class CollectionItem
    {
        public Key key;
        public int value;
    }

    [SerializeField]
    private BoundedInt levelData;

    [SerializeField]
    private CollectionItem collectionItem;

    public override bool IsReached()
    {
        string achievementKey = $"{GameKey.ARCHIEVEMENT}-{GetID}";

        if (PlayerPrefs.GetInt(achievementKey, 0) == 1)
            return true;

        switch (collectionItem.key)
        {
            case Key.ReachedLevel:
                if (collectionItem.value != levelData.Value)
                    return false;
                break;
            default:
                break;
        }

        PlayerPrefs.SetInt(achievementKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"<color=white>UNLOCKED - ACHIEVEMENT - {GetTitle}</color>");

        return true;
    }

    #region Reward

    public override void ReceiveReward()
    {
        string achievementKey = $"{GameKey.ARCHIEVEMENT_REWARD}-{GetID}";

        if (PlayerPrefs.GetInt(achievementKey, 0) == 1)
            return;

        PlayerPrefs.SetInt(achievementKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"<color=white>RECEIVE REWARD - ACHIEVEMENT - {GetTitle}</color>");
    }

    public override bool IsReceiveReward()
    {
        string achievementKey = $"{GameKey.ARCHIEVEMENT_REWARD}-{GetID}";

        if (PlayerPrefs.GetInt(achievementKey, 0) == 1)
            return true;
        else
            return false;
    }

    #endregion // Reward
}
