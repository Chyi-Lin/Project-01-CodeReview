using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnCompleteLevel;

    public static event Action<int> OnTimeCountdown;

    public static bool gameIsReady = false;
    public static bool gameIsOver = false;

    public static bool playerIsReady = false;
    public static bool playerIsFever = false;
    public static bool playerIsGetReward = false;
    public static bool playerIsRelived = false;

    private bool isWon = false;
    private bool hasNextLevel = false;

    [Header("Time")]
    [SerializeField]
    private int startTime = 120;
    public static int currentTime;

    [Header("Datas")]
    [SerializeField]
    private BoundedInt levelData;

    [Header("Game UI")]
    [SerializeField]
    private GameObject pauseButton;

    [SerializeField]
    private GameOverNotificationUI gameOverNotificationUI;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject completeLevelUI;

    [SerializeField]
    private GameObject congratulationUI;

    [SerializeField]
    private GameObject reliveMenuUI;

    [Header("Localization Notify")]
    [SerializeField]
    private string completeKey;

    [SerializeField]
    private string timeUpKey;

    [SerializeField]
    private string noMoreRingKey;

    private Coroutine countdownCor;

    private void Awake()
    {
        gameIsOver = false;
        gameIsReady = false;
        playerIsReady = false;
        playerIsFever = false;
        playerIsGetReward = false;
        currentTime = startTime;
        countdownCor = null;
    }

    private void Start()
    {
        countdownCor = StartCoroutine(TimeCountdown());
    }

    private void Update()
    {
        if (!gameIsReady || !playerIsReady)
            return;

        if (gameIsOver)
            return;

        if (GameStat.currentTarget <= 0)
        {
            if(LocalizationManager.instance != null)
                WinGame(LocalizationManager.instance.GetLoaclizedValue(completeKey));
            else
                WinGame("COMPLETE");

            if (countdownCor != null)
                StopCoroutine(countdownCor);
        }

        if (GameStat.currentRing <= 0 && !playerIsGetReward)
        {
            if (LocalizationManager.instance != null)
                EndGame(LocalizationManager.instance.GetLoaclizedValue(noMoreRingKey));
            else
                EndGame("NO MORE RING");

            if (countdownCor != null)
                StopCoroutine(countdownCor);
        }
    }

    private IEnumerator TimeCountdown()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while (!gameIsReady || !playerIsReady)
        {
            // No time limit
            if (currentTime == -1)
            {
                OnTimeCountdown?.Invoke(currentTime);
                currentTime = 0;
                countdownCor = null;
                yield break;
            }

            yield return null;
        }

        while (currentTime > 0)
        {
            while (playerIsFever)
                yield return null;

            // Admob ads
            while (!playerIsRelived)
                yield return null;

            currentTime--;
            OnTimeCountdown?.Invoke(currentTime);

            yield return waitForSeconds;
        }

        countdownCor = null;

        gameIsOver = true;

        if (LocalizationManager.instance != null)
            EndGame(LocalizationManager.instance.GetLoaclizedValue(timeUpKey));
        else
            EndGame("TIME UP");
    }

    private void OnDestroy()
    {
        TweenUI.StopAllSequence();
    }

    private void EndGame(string endNotification)
    {
        // Close UI
        pauseButton.SetActive(false);

        gameIsOver = true;
        isWon = false;

        gameOverNotificationUI.SetTest(endNotification, isWon);
        gameOverNotificationUI.Show(0f, NotificationComplete);
    }

    private void WinGame(string winNotification)
    {
        // Close UI
        pauseButton.SetActive(false);

        // Event
        OnCompleteLevel?.Invoke();

        gameIsOver = true;
        isWon = true;

        hasNextLevel = HasNextLevel();
        gameOverNotificationUI.SetTest(winNotification, isWon);
        gameOverNotificationUI.Show(2f, NotificationComplete);
    }

    private void NotificationComplete()
    {
        if (isWon)
        {
            if (hasNextLevel)
                completeLevelUI.SetActive(true);
            else
                congratulationUI.SetActive(true);
        }
        else
        {
            gameOverUI.SetActive(true);
        }
    }

    private bool HasNextLevel()
    {
        int levelCount = PlayerPrefs.GetInt(GameKey.LEVEL_COUNT, 0);
        //int levelToUnlock = GameStat.currentLevel + 1;
        int levelToUnlock = levelData.Value + 1;

        if (levelToUnlock <= levelCount)
        {
            
            GameSpawner.selectLevelIndex = levelToUnlock;

            // Save level reached
            int levelReached = PlayerPrefs.GetInt(GameKey.LEVEL_UNLOCKED, 1);
            if (levelToUnlock > levelReached)
            {
                PlayerPrefs.SetInt(GameKey.LEVEL_UNLOCKED, levelToUnlock);
#if UNITY_EDITOR
                Debug.Log($"<color=white>UNLOCK LEVEL : {levelToUnlock}</color>");
#endif
            }

            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("<color=white>COMPLETED ALL LEVEL!</color>");
#endif
            GameSpawner.selectLevelIndex = levelData.Value;

            return false;
        }
    }

#if DEBUG

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = 65;
        style.normal.textColor = Color.white;
        if (GUI.Button(new Rect(25, 125, 550, 100), "WIN GAME", style))
        {
            WinGame("COMPLETE");
        }

        if (GUI.Button(new Rect(25, 225, 550, 100), "END GAME", style))
        {
            EndGame("TIME OUT");
        }

    }

#endif

}
