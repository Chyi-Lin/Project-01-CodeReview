using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFever : MonoBehaviour
{
    public event Action OnFeverStartEvent;

    public event Action OnFeverStopEvent;

    [Header("Ring")]
    [SerializeField]
    private GameSpawner gameSpawner;

    [Header("Fever Value")]
    [SerializeField]
    private bool isFever = false;

    [SerializeField]
    private int feverRingAmount = 3;

    [SerializeField]
    private float feverDuraction = 20f;

    [SerializeField]
    private float feverMaxValue = 100f;

    [SerializeField]
    private float value = 5f;

    [SerializeField]
    private float ComboValue = 10f;

    [SerializeField]
    private List<Ring> currentFeverRing = new List<Ring>();

    [Header("Environment")]
    [SerializeField]
    private Renderer backgroundRenderer;
    private MaterialPropertyBlock propertyBlock;

    [SerializeField]
    private Color feverColor;

    [Header("UI")]
    [SerializeField]
    private TimeUI timeUI;

    [SerializeField]
    private FeverTimeUI feverTimeUI;

    private float currentFever;
    private Coroutine feverCor;

    public bool IsFever => isFever;

    public float GetMaxValue => feverMaxValue;

    public float GetCurrentFever => currentFever;

    private void Start()
    {
        isFever = false;

        propertyBlock = new MaterialPropertyBlock();
        backgroundRenderer.GetPropertyBlock(propertyBlock);
    }

    private void OnEnable()
    {
        RingTarget.OnEnterEvent += AddFeverValue;
        gameSpawner.OnSaveRingEvent += AddRing;
    }

    private void OnDisable()
    {
        RingTarget.OnEnterEvent -= AddFeverValue;
        gameSpawner.OnSaveRingEvent -= AddRing;
    }

    private void AddFeverValue()
    {
        if (isFever)
            return;

        if(GameScoreCombo.currentCombo > 1)
            currentFever += ComboValue;
        else
            currentFever += value;

        if(currentFever >= feverMaxValue)
        {
            currentFever = feverMaxValue;
            isFever = true;

            if (feverCor != null)
                StopCoroutine(feverCor);

            Fever(isFever);
            feverCor = StartCoroutine(FeverCountdown());
        }
    }

    private void Fever(bool toggle)
    {
        GameManager.playerIsFever = toggle;
        timeUI.enabled = !toggle;
        feverTimeUI.enabled = toggle;


        if (toggle)
        {
            List<Ring> rings = gameSpawner.GetRingList;
            for (int i = 0; i < rings.Count; i++)
            {
                //if (rings[i].HitTarget)
                //    continue;

                rings[i].SetFeverRing(true);
                currentFeverRing.Add(rings[i]);

                //if (currentFeverRing.Count >= feverRingAmount)
                //    break;
            }

            OnFeverStartEvent?.Invoke();
        }
        else
        {
            for (int i = 0; i < currentFeverRing.Count; i++)
            {
                currentFeverRing[i].SetFeverRing(false);
            }

            currentFeverRing.Clear();

            OnFeverStopEvent?.Invoke();
        }
        
    }

    private IEnumerator FeverEnvironment(Color originColor, Color targetColor)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float normalize = 0f;

        while (normalize < 1f)
        {
            normalize += Time.deltaTime;
            propertyBlock.SetColor("_Color", Color.Lerp(originColor, targetColor, normalize));
            backgroundRenderer.SetPropertyBlock(propertyBlock);

            yield return waitForEndOfFrame;
        }

        propertyBlock.SetColor("_Color", Color.Lerp(originColor, targetColor, 1f));
        backgroundRenderer.SetPropertyBlock(propertyBlock);
    }

    private IEnumerator FeverCountdown()
    {
        if (gameSpawner.GetCurrnetLevel != null)
            yield return FeverEnvironment(gameSpawner.GetCurrnetLevel.GetBackgroundColor, feverColor);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        float feverTime = feverDuraction;

        while (feverTime >= 0)
        {
            if (GameManager.gameIsOver)
                yield break;

            feverTime -= Time.deltaTime;
            currentFever = feverMaxValue * (feverTime / feverDuraction);
            yield return waitForEndOfFrame;
        }

        if (gameSpawner.GetCurrnetLevel != null)
            yield return FeverEnvironment(feverColor, gameSpawner.GetCurrnetLevel.GetBackgroundColor);

        currentFever = 0f;

        feverCor = null;
        isFever = false;

        Fever(isFever);
    }

    public void AddRing(Ring ring)
    {
        if (!isFever)
            return;

        ring.SetFeverRing(true);
        currentFeverRing.Add(ring);
    }
}
