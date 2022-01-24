using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class SelectLevelUI : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField]
    private string gameSceneName = "GameScene";

    [Header("UI")]
    [SerializeField]
    private AssetReference buttonAssetReference;

    [SerializeField]
    private RectTransform buttonParent;

    [Header("Level")]
    [SerializeField]
    private AssetLabelReference levelLabel;

    private int levelCount;
    private int levelIndex;

    private IEnumerator Start()
    {
        // Get match key resource locations
        AsyncOperationHandle<IList<IResourceLocation>> handle =
            Addressables.LoadResourceLocationsAsync(levelLabel.labelString);
        yield return handle;

        levelCount = handle.Result.Count;
        if(levelCount > PlayerPrefs.GetInt(GameKey.LEVEL_COUNT, 0))
        {
            PlayerPrefs.SetInt(GameKey.LEVEL_COUNT, levelCount);
            PlayerPrefs.Save();
        }

        yield return CreateLevelButton(levelCount);

        Addressables.Release(handle);
    }

    private IEnumerator CreateLevelButton(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AsyncOperationHandle<GameObject> handle = buttonAssetReference.InstantiateAsync(buttonParent);
            handle.Completed += InitLevelButton;

            yield return null;
        }
    }

    private void InitLevelButton(AsyncOperationHandle<GameObject> obj)
    {
        levelIndex++;

        LevelButton levelButton = obj.Result.GetComponent<LevelButton>();
        levelButton.SetId(levelIndex);
        levelButton.SetText(levelIndex.ToString());
        levelButton.SetOnClickEvent(SelectLevel);

        int unlickedLevel = PlayerPrefs.GetInt(GameKey.LEVEL_UNLOCKED, 1);
        if(levelIndex == unlickedLevel)
        {
            // Level 1 always enable
            levelButton.SetInteractable = true;
        }
        else if (unlickedLevel <= levelIndex)
        {
            levelButton.SetInteractable = false;
        }
    }

    private void SelectLevel(int id)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=white>SELECT LEVEL {id}</color>");
#endif
        GameSpawner.selectLevelIndex = id;

        LoadingSceneSystem.LoadScene(gameSceneName);
    }
}
