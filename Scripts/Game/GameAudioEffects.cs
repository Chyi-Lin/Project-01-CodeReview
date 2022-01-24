using UnityEngine;

public class GameAudioEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    private GameSpawner gameSpawner;
    private GameInput gameInput;

    private void Awake()
    {
        gameSpawner = FindObjectOfType<GameSpawner>();
        gameInput = FindObjectOfType<GameInput>();
    }

    private void OnEnable()
    {
        gameSpawner.OnPlayAudioEffect += OnPlayAudioEffect;
        gameInput.OnPlayAudioEffect += OnPlayAudioEffect;
        RingTarget.OnPlayAudioEffect += OnPlayAudioEffect;
        Coin.OnPlayAudioEffect += OnPlayAudioEffect;
    }
    private void OnDisable()
    {
        gameSpawner.OnPlayAudioEffect -= OnPlayAudioEffect;
        gameInput.OnPlayAudioEffect -= OnPlayAudioEffect;
        RingTarget.OnPlayAudioEffect -= OnPlayAudioEffect;
        Coin.OnPlayAudioEffect -= OnPlayAudioEffect;
    }

    private void OnPlayAudioEffect(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

}
