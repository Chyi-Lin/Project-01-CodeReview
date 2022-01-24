using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingTarget : MonoBehaviour
{
    public static event Action OnEnterEvent;

    public static event Action<Component, Vector2, int> OnChangeScoreUIEvent;

    public static event Action<int> OnChangeScoreEvent;

    public static event Action<int> OnChangeRingAmountEvent;

    public static event Action OnCompleteTargetEvent;

    public static event Action<AudioClip> OnPlayAudioEffect;

    [Header("Request Target")]
    [SerializeField]
    private bool isComplete = false;

    [SerializeField]
    private int completeScore = 20;

    [SerializeField]
    private int targetRing = 5;

    [SerializeField]
    private TMP_Text targetRingText;

    [SerializeField]
    private TriggerEvent triggerEvent;

    [SerializeField]
    private AudioClip triggerAudioEffect;

    [SerializeField]
    private Renderer targetRenderer;
    private MaterialPropertyBlock propertyBlock;

    [Header("Explosion Effect")]
    [SerializeField]
    private float explostionDelayTime = 1.5f;

    [SerializeField]
    private float explosionForce = 2.5f;

    [SerializeField]
    private float explosionRadius = 1f;

    [SerializeField]
    private float explosionUpwards = 1f;

    [SerializeField]
    private ForceMode explosionMode;

    [SerializeField]
    private Vector3 explosionOffset;

    [SerializeField]
    private ParticleSystem explosionEffect;

    [SerializeField]
    private AudioClip explosionAudioEffect;

    private CameraShake cameraShake;

    private GameSpawner gameSpawner;

    private List<Ring> enterRings = new List<Ring>();

    private void Start()
    {
        Init();

        ChangeTargetText();
    }

    private void Init()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        gameSpawner = FindObjectOfType<GameSpawner>();

        if (gameSpawner.GetCurrnetLevel == null)
            return;

        propertyBlock = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", gameSpawner.GetCurrnetLevel.GetTargetColor);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }

    private void OnEnable()
    {
        triggerEvent.OnTriggerEnterEvent += EnterListener; 
        triggerEvent.OnTriggerExitEvent += ExitListener;
    }

    private void OnDisable()
    {
        triggerEvent.OnTriggerEnterEvent -= EnterListener;
        triggerEvent.OnTriggerExitEvent -= ExitListener;
    }

    private void EnterListener(Collider collider)
    {
        if (GameManager.gameIsOver)
            return;

        if (isComplete)
            return;

        Ring ring = collider.GetComponentInParent<Ring>();

        if (ring == null)
            return;

        if (CheckRingIsTrigger(ring))
            return;

        OnEnterEvent?.Invoke();

        Vector3 fixedPosition = transform.position;
        fixedPosition.y = collider.transform.position.y;
        ring.SetPosition(fixedPosition);
        ring.HitTarget = true;

        if (targetRing > 0)
            targetRing--;

        // Fever Ring
        if (ring.IsFever)
        {
            explosionEffect.Play();
            cameraShake.Shake(0.1f, 0.1f);
        }

        GameStat.currentRing--;

        // Calculate Score
        int score = ring.CurrentScore * ring.GetScoreDoubleLevel;
        score = GameScoreCombo.GetCalculateDoubleScore(score);
        GameStat.currentScore += score;


        if (targetRing == 0)
        {
            isComplete = true;
            GameManager.playerIsGetReward = true;
            GameStat.currentTarget -= 1;
            Invoke("CompleteTarget", explostionDelayTime);
        }

        ChangeTargetText();

        // Event
        OnChangeRingAmountEvent?.Invoke(GameStat.currentRing);
        OnChangeScoreEvent?.Invoke(GameStat.currentScore);
        OnChangeScoreUIEvent?.Invoke(this, transform.position, score);

        // Audio Event
        OnPlayAudioEffect.Invoke(triggerAudioEffect);
    }

    private void ExitListener(Collider collider)
    {
        
    }

    private bool CheckRingIsTrigger(Ring ring)
    {
        if (enterRings.Contains(ring))
            return true;

        enterRings.Add(ring);
        return false;
    }

    private void CompleteTarget()
    {
        cameraShake.Shake(0.1f, 0.1f);

        CompleteReward();

        ParticleSystem explostionEffect = Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        explostionEffect.Play();
        Destroy(explostionEffect.gameObject, 5f);

        gameObject.SetActive(false);
        GameManager.playerIsGetReward = false;

        // Recode
        if (PlayerStat.Instance != null)
            PlayerStat.Instance.completeTargets++;

        // Event
        OnPlayAudioEffect?.Invoke(explosionAudioEffect);
        OnCompleteTargetEvent?.Invoke();
        
    }

    private void CompleteReward()
    {
        // The current ring adds the saved ring
        GameStat.currentRing += enterRings.Count;
        //GameStat.currentScore += GameScoreCombo.GetCalculateDoubleScore(completeScore);

        // Reward rate
        bool hasReward = true;
        float randomChance = Random.Range(0f, 1f);
        if(PlayerStat.Instance != null)
            hasReward = PlayerStat.Instance.rewardRate >= randomChance;

        // The current ring adds the reward ring
        if (hasReward)
        {
            int rawardRing = GameStat.currentRewardRing;
            for (int i = 0; i < rawardRing; i++)
            {
                gameSpawner.SpawnRing(transform.position, Quaternion.identity, (obj) =>
                {
                    Ring ring = obj.Result.GetComponent<Ring>();
                    ring.AddExplosionForce(transform.position + explosionOffset, explosionForce, explosionRadius, explosionUpwards, explosionMode);

                    OnChangeRingAmountEvent?.Invoke(GameStat.currentRing);
                });
            }
        }
        

        // Release enter rings
        for (int i = 0; i < enterRings.Count; i++)
        {
            enterRings[i].HitTarget = false;
            enterRings[i].AddExplosionForce(transform.position + explosionOffset, explosionForce, explosionRadius, explosionUpwards, explosionMode);
            enterRings[i].AddScoreDoubleLevel();
        }

        enterRings.Clear();

        // Event 
        OnChangeRingAmountEvent?.Invoke(GameStat.currentRing);
        OnChangeScoreEvent?.Invoke(GameStat.currentScore);
        //OnChangeScoreUIEvent?.Invoke(this, transform.position, completeScore);
    }

    private void ChangeTargetText()
    {  
        if(targetRing > 0)
        {
            targetRingText.SetText(targetRing.ToString());
            TweenUI.TweenScale(targetRingText.rectTransform, Vector3.one * 2f, .5f);
        }
        else
        {
            targetRingText.SetText("");
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + explosionOffset, explosionRadius);
    }

}
