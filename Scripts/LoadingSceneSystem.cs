using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingSceneSystem : MonoBehaviour
{
    private static string LoadingSceneName = "LoadingScene";

    private static string nextScene = "MainMenu";

    [System.Serializable]
    public class Progress : UnityEvent<float> { }

    public Progress progress;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene(LoadingSceneName);
    }

    public static void LoadScene(Scene scene)
    {
        nextScene = scene.name;

        SceneManager.LoadScene(LoadingSceneName);
    }

    private IEnumerator Start()
    {
        progress?.Invoke(0f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        while (!operation.isDone)
        {
            progress?.Invoke(operation.progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
