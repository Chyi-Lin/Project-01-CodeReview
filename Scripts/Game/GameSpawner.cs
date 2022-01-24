using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GameStat))]
public class GameSpawner : MonoBehaviour
{
    public event Action<int> OnRingAmountChangeEvent;

    public event Action<Ring> OnSaveRingEvent;

    public event Action OnLevelInitCompleteEvent;

    public event Action<AudioClip> OnPlayAudioEffect;

    public static int selectLevelIndex = -1;

    [Header("Level")]
    [SerializeField]
    private GameLevel startLevel;

    [SerializeField]
    private string formatLevelKey = "Level_{0}.asset";

    [SerializeField]
    private Transform levelSpawnTarget;

    [SerializeField]
    private Renderer levelBackgroundRenderer;
    private MaterialPropertyBlock propertyBlock;

    [Header("Datas")]
    [SerializeField]
    private BoundedInt levelData;

    [Header("Ring")]
    [SerializeField]
    private AssetReference ringPrefab;

    [SerializeField]
    private float spawnDelayTime = .5f;

    [SerializeField]
    private float ringSpawnRotationX = 36f;

    [SerializeField]
    private Transform ringSpawnPosition;

    [SerializeField]
    private Transform ringSpawnTarget;

    private List<Ring> ringList = new List<Ring>();

    [Header("Customize")]
    [SerializeField]
    private CustomizeColorData customizeColorData;

    [SerializeField] 
    private List<Color> selectedColor;

    private int totalColorLength;

    private int currentColorIndex = -1;

    [Header("Effect")]
    [SerializeField]
    private ParticleSystem ringSpawnEffect;
    private ParticleSystemRenderer spawnEffectRenderer;

    [SerializeField]
    private AudioClip inputAudioEffect;

    public GameLevel GetCurrnetLevel => startLevel;

    public List<Ring> GetRingList => ringList;

    private void Awake()
    {
        spawnEffectRenderer = ringSpawnEffect.GetComponent<ParticleSystemRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        levelBackgroundRenderer.GetPropertyBlock(propertyBlock);

        InitPlayerSetting();

        StartCoroutine(InitLevel());
    }

    private void InitPlayerSetting()
    {
        if (PlayerStat.Instance == null)
            return;
        
        selectedColor = customizeColorData.GetSelectedColors(PlayerStat.Instance.selectedColors);
        totalColorLength = selectedColor.Count;
    }

    private IEnumerator InitLevel()
    {
        if(selectLevelIndex != -1)
        {
            yield return LoadLevelResourceAsync(selectLevelIndex);
            selectLevelIndex = -1;
        }
            

        if (startLevel == null)
        {
            Debug.Log("<color=white>START LEVEL IS NULL!</color>");
            yield break;
        }

        levelData.Value = startLevel.GetLevelIndex;

        GameManager.currentTime = startLevel.GetLevelTime;
        //GameStat.currentLevel = startLevel.GetLevelIndex;
        GameStat.currentTarget = startLevel.GetTargetAmount;
        GameStat.currentHighscore = PlayerPrefs.GetInt($"{GameKey.LEVEL_HIGHSCORE}-{levelData.Value}", 0);

        // Level init 
        OnLevelInitCompleteEvent?.Invoke();

        // Level background color
        propertyBlock.SetColor("_Color", startLevel.GetBackgroundColor);
        levelBackgroundRenderer.SetPropertyBlock(propertyBlock);

        yield return LevelSpawn();

        yield return RingSpawn();

        GameManager.gameIsReady = true;
    }

    private IEnumerator LoadLevelResourceAsync(int level)
    {
        // Get match key resource locations
        string levelKey = string.Format(formatLevelKey, level);
        AsyncOperationHandle<IList<IResourceLocation>> handle =
            Addressables.LoadResourceLocationsAsync(levelKey);

        yield return handle;

        //Debug.Log("result count:" + handle.Result.Count);
        AsyncOperationHandle<GameLevel> levelHandle = Addressables.LoadAssetAsync<GameLevel>(handle.Result[0]);
        yield return levelHandle;

        startLevel = levelHandle.Result;

        Addressables.Release(handle);
    }

    private IEnumerator LevelSpawn()
    {
        AsyncOperationHandle<GameObject> levelObject = Addressables.InstantiateAsync(startLevel.GetLevelPrefab, levelSpawnTarget.position, Quaternion.identity, levelSpawnTarget);
        yield return levelObject;
    }

    private IEnumerator RingSpawn()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnDelayTime);
        for (int i = 0; i < GameStat.currentSpawnRing; i++)
        {
            SpawnRing(ringSpawnPosition.position, Quaternion.Euler(ringSpawnRotationX, 0f, 0f));
            yield return waitForSeconds;
        }
    }

    public void SpawnRing(Vector3 position, Quaternion rotation, Action<AsyncOperationHandle<GameObject>> asyncOperation = null)
    {
        AsyncOperationHandle<GameObject> ringObject = ringPrefab.InstantiateAsync(position, rotation, ringSpawnTarget);
        ringObject.Completed += asyncOperation;
        ringObject.Completed += SpawnEffect;
        ringObject.Completed += SaveRingToList;
        ringObject.Completed += SpawnHighLevelRing;

        GameStat.currentRing++;
        OnRingAmountChangeEvent?.Invoke(GameStat.currentRing);
    }

    private void SpawnHighLevelRing(AsyncOperationHandle<GameObject> handle)
    {
        if (PlayerStat.Instance == null)
            return;

        GameObject spawnObj = handle.Result;
        Ring ring = spawnObj.GetComponent<Ring>();

        // Highlevel rate
        float randomChance = Random.Range(0f, 1f);
        bool hasHighlevel = PlayerStat.Instance.highLevelRate >= randomChance;

        if (hasHighlevel)
            ring.AddScoreDoubleLevel();
    }

    private void SpawnEffect(AsyncOperationHandle<GameObject> handle)
    {
        GameObject spawnObj = handle.Result;
        ChangeColor changeColor = spawnObj.GetComponent<ChangeColor>();
        Ring ring = spawnObj.GetComponent<Ring>();

        // Init customize Setting
        if(totalColorLength > 0)
            changeColor.PropertyColor = GetRingCustomizeColor();
        changeColor.InitCustomizeSetting();
        ring.InitCustomizeSetting();

        MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
        spawnEffectRenderer.GetPropertyBlock(materialProperty);
        materialProperty.SetColor("_Color", changeColor.PropertyColor);
        spawnEffectRenderer.SetPropertyBlock(materialProperty);

        ringSpawnEffect.transform.position = spawnObj.transform.position;
        ringSpawnEffect.Play();

        OnPlayAudioEffect?.Invoke(inputAudioEffect);
    }

    private void SaveRingToList(AsyncOperationHandle<GameObject> handle)
    {
        GameObject spawnObj = handle.Result;
        Ring ring = spawnObj.GetComponent<Ring>();

        ringList.Add(ring);

        OnSaveRingEvent?.Invoke(ring);
    }

    private Color GetRingCustomizeColor()
    {
        currentColorIndex++;
        if (currentColorIndex > totalColorLength - 1)
            currentColorIndex = 0;

        return selectedColor[currentColorIndex];
    }

#if DEBUG

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = 65;
        style.normal.textColor = Color.white;
        if (GUI.Button(new Rect(25, 25, 550, 100), "RING SPAWN", style))
        {
            StartCoroutine(RingSpawn());
        }
    }

#endif

}
