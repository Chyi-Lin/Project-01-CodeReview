using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Action<AudioClip> OnPlayAudioEffect;

    [SerializeField]
    private int amount = 1;

    [SerializeField]
    private GameObject coinModel;

    [SerializeField]
    private AudioClip coinAudioEffect;

    [SerializeField]
    private ParticleSystem coinEffect;

    private bool isTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
            return;
        isTrigger = true;

        if (PlayerStat.Instance != null)
        {
            PlayerStat.Instance.coin += amount;

            GameStat.currentGetCoin += amount;
        }

        coinModel.SetActive(false);
        OnPlayAudioEffect?.Invoke(coinAudioEffect);
        coinEffect.Play();
        enabled = false;
    }
}
