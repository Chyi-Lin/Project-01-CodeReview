using UnityEngine;

public class GameAudioManager : MonoBehaviour
{

    public AudioSource audioSource { get; private set; }

    #region Singleton Pattern

    public static GameAudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Init();
    }

    #endregion // Singleton Pattern

    private void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
