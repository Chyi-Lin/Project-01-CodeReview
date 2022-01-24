using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMeun : MonoBehaviour
{
    [SerializeField]
    private string mainMenuName = "MainMenu";

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private BoundedInt levelData;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Toggle();
        GameSpawner.selectLevelIndex = levelData.Value;
        LoadingSceneSystem.LoadScene(SceneManager.GetActiveScene());
    }

    public void Menu()
    {
        Toggle();
        LoadingSceneSystem.LoadScene(mainMenuName);
    }

}
