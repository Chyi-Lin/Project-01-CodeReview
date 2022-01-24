using UnityEngine;
using UnityEngine.UI;

public class GameButtonAudioEffect : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip buttonClickEffect;

    private Button[] buttons;
    private int buttonsLength;

    private void Awake()
    {
        buttons = FindObjectsOfType<Button>();
        buttonsLength = buttons.Length;
    }

    private void OnEnable()
    {
        for (int i = 0; i < buttonsLength; i++)
        {
            buttons[i].onClick.AddListener(OnPlayAudioEffect);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < buttonsLength; i++)
        {
            buttons[i].onClick.RemoveListener(OnPlayAudioEffect);
        }
    }

    private void OnPlayAudioEffect()
    {
        _audioSource.PlayOneShot(buttonClickEffect);
    }
}
