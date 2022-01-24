using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ArchievementUI : MonoBehaviour
{
    [SerializeField]
    private List<IArchievementData> archievementDatas;

    [SerializeField]
    private AssetReference itemAsset;

    [SerializeField]
    private RectTransform items;

    [SerializeField]
    private PassEvent OnRefreshEvent;

    private IEnumerator Start()
    {
        yield return CreateItem();
    }

    private IEnumerator CreateItem()
    {
        for (int i = 0; i < archievementDatas.Count; i++)
        {

            AsyncOperationHandle<GameObject> handle = itemAsset.InstantiateAsync(items);

            yield return handle;

            ArchievementItemUI ui = handle.Result.GetComponent<ArchievementItemUI>();
            InitItem(ui, archievementDatas[i]);
        }
    }

    private void InitItem(ArchievementItemUI ui, IArchievementData data)
    {
        ui.SetContent(data);

        bool reached = false;
        int maxValue = 1, needValue = 0;

        switch (data.GetArchievementType)
        {
            case IArchievementData.Type.ONCE:
                OnceArchievement oa = (OnceArchievement)data;
                reached = oa.IsReached();

                maxValue = 1;
                needValue = reached ? 1 : 0;

                break;
            case IArchievementData.Type.COLLECTION:
                CollectionArchievement ca = (CollectionArchievement)data;
                reached = ca.IsReached();

                Dictionary<string, int> recode = ca.GetMaxValueAndCurValue(PlayerStat.Instance);
                maxValue = recode["Max"];
                needValue = recode["Cur"];

                break;
            case IArchievementData.Type.NONE:
            default:
                break;
        }

        

        if (data.IsReceiveReward())
        {
            ui.SetRewardButton(false);
        }
        else
        {
            ui.SetRewardButton(reached);
            ui.AddRewardButtonEvent(OnRefreshEvent.Invoke);
        }
        
        ui.SetComplete(data.IsReceiveReward());
        ui.SetProgress(needValue, maxValue);
    }
}
