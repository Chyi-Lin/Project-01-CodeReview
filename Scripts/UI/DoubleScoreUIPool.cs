using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DoubleScoreUIPool : MonoBehaviour
{
    public struct UIData
    {
        public GameObject parent;
        public DoubleScoreUI doubleScoreUI;
    }

    [SerializeField]
    private Transform targetParent;

    [SerializeField]
    private AssetReference doubleScoreReference;

    [SerializeField]
    private int initCount = 5;

    private Queue<UIData> pool = new Queue<UIData>();

    private IEnumerator Start()
    {
        for (int i = 0; i < initCount; i++)
        {
            yield return InstantiateUIData();
        }
    }

    private IEnumerator InstantiateUIData()
    {
        AsyncOperationHandle<GameObject> handle = doubleScoreReference.InstantiateAsync(targetParent);
        handle.Completed += (handleData) =>
        {
            handleData.Result.SetActive(false);
            DoubleScoreUI doubleScore = handleData.Result.GetComponentInChildren<DoubleScoreUI>();

            UIData uIData = new UIData()
            {
                parent = handleData.Result,
                doubleScoreUI = doubleScore,
            };

            uIData.parent.SetActive(false);
            pool.Enqueue(uIData);
        };

        yield return handle;
    }

    public UIData DequeueUI()
    {
        UIData doubleScore = pool.Dequeue();
        doubleScore.parent.SetActive(true);

        pool.Enqueue(doubleScore);
        return doubleScore;
    }

}
