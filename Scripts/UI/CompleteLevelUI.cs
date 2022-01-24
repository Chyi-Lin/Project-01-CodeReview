using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevelUI : MonoBehaviour
{
    [SerializeField]
    private string mainMenuName = "MainMenu";

    public void Continue()
    {
        LoadingSceneSystem.LoadScene(SceneManager.GetActiveScene());
    }

    public void Menu()
    {
        LoadingSceneSystem.LoadScene(mainMenuName);
    }
}
