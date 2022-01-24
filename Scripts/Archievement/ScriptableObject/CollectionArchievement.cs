using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archievement File", menuName = "Ring Toss Game/Create Archievement/Create New Collection Archievement", order = 51)]
public class CollectionArchievement : IArchievementData
{
    [System.Serializable]
    public enum Key
    {
        ClearTarget = 5,
    }


    [System.Serializable]
    public class CollectionItem
    {
        public Key key;
        public int value;
    }

    [SerializeField]
    private CollectionItem collectionItem;

    public override bool IsReached()
    {
        string achievementKey = $"{GameKey.ARCHIEVEMENT}-{GetID}";

        if (PlayerPrefs.GetInt(achievementKey, 0) == 1)
            return true;

        if (PlayerStat.Instance == null)
            return false;

        switch (collectionItem.key)
        {
            case Key.ClearTarget:
                if (collectionItem.value > PlayerStat.Instance.completeTargets)
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

    public Dictionary<string, int> GetMaxValueAndCurValue(PlayerStat player)
    {
        Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
        keyValuePairs.Add("Max", collectionItem.value);
        keyValuePairs.Add("Cur", player.completeTargets);

        return keyValuePairs;
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
