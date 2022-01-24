using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private string mainMenuName = "MainMenu";

    [SerializeField]
    private BoundedInt levelData;

    public void Retry()
    {
        GameSpawner.selectLevelIndex = levelData.Value;
        LoadingSceneSystem.LoadScene(SceneManager.GetActiveScene());
    }

    public void Menu()
    {
        LoadingSceneSystem.LoadScene(mainMenuName);
    }

}
