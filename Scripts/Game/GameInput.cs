using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event Action OnTouchingAnyScreen;

    public event Action OnTouchUp;

    public event Action OnTouchDown;

    public event Action OnTouchingOverTime;

    public event Action<AudioClip> OnPlayAudioEffect;

    public event Action<float> OnTouchStrengthEvent;

    [SerializeField]
    private float longTouchingInfluenceDuraction = 1f;
    private float totalTouchingTime;
    private float touchStrengthValue;

    [SerializeField]
    private float recoveryToucingSpeed = 0.5f;

    private bool isTouching = false;
    private bool isOverInfluence = false;

    public float GetInfluenceDuraction => longTouchingInfluenceDuraction;

    public float GetTouchingTime => totalTouchingTime;

    public float GetStrengthValue => touchStrengthValue;

    [SerializeField]
    private AudioClip inputAudioEffect;

    private void Start()
    {
        InitPlayerStat();
    }

    private void InitPlayerStat()
    {
        if (PlayerStat.Instance == null)
            return;

        recoveryToucingSpeed = PlayerStat.Instance.energyRecoverySpeed;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
            enabled = false;

#if UNITY_EDITOR
            OnMouseControl();
#else
            OnTouchControl();
#endif

    }

    private void OnMouseControl()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            isOverInfluence = false;
            touchStrengthValue = 0f;
            //totalTouchingTime = 0f;

            OnTouchDown?.Invoke();
            OnPlayAudioEffect?.Invoke(inputAudioEffect);
        }

        if (Input.GetMouseButton(0) && isTouching)
        {
            // Influence time
            if (longTouchingInfluenceDuraction > totalTouchingTime)
            {
                totalTouchingTime += Time.deltaTime;
                touchStrengthValue += Time.deltaTime;
            }
                

            // Is fever time
            //if (GameManager.playerIsFever)
            //    totalTouchingTime = 0f;

            if (totalTouchingTime <= longTouchingInfluenceDuraction)
            {
                OnTouchingAnyScreen?.Invoke();
            }

            // Influence Over  
            if (totalTouchingTime > longTouchingInfluenceDuraction && !isOverInfluence)
            {
                isOverInfluence = true;
                OnTouchingOverTime?.Invoke();
            }
        }
        else
        {
            if (totalTouchingTime > 0)
                totalTouchingTime -= recoveryToucingSpeed * Time.deltaTime;
            else
                totalTouchingTime = 0f;
        }

        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            isTouching = false;
            OnTouchUp?.Invoke();
            OnTouchStrengthEvent?.Invoke(touchStrengthValue / longTouchingInfluenceDuraction);
        }
    }

    private void OnTouchControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch1 = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
                return;

            if (touch1.phase == TouchPhase.Began)
            {
                isTouching = true;
                isOverInfluence = false;
                touchStrengthValue = 0f;
                //totalTouchingTime = 0f;

                OnTouchDown?.Invoke();
                OnPlayAudioEffect?.Invoke(inputAudioEffect);
            }

            if ((touch1.phase == TouchPhase.Stationary) || (touch1.phase == TouchPhase.Moved) 
                && isTouching)
            {
                // Influence time
                if (longTouchingInfluenceDuraction > totalTouchingTime)
                {
                    totalTouchingTime += Time.deltaTime;
                    touchStrengthValue += Time.deltaTime;
                }

                // Is fever time
                //if (GameManager.playerIsFever)
                //    totalTouchingTime = 0f;

                if (totalTouchingTime <= longTouchingInfluenceDuraction)
                {
                    OnTouchingAnyScreen?.Invoke();
                }

                // Influence Over  
                if (totalTouchingTime > longTouchingInfluenceDuraction && !isOverInfluence)
                {
                    isOverInfluence = true;
                    OnTouchingOverTime?.Invoke();
                }
            }

            if (touch1.phase == TouchPhase.Ended && isTouching)
            {
                isTouching = false;
                OnTouchUp?.Invoke();
                OnTouchStrengthEvent?.Invoke(touchStrengthValue / longTouchingInfluenceDuraction);
            }
        }
        else
        {
            if (totalTouchingTime > 0)
                totalTouchingTime -= recoveryToucingSpeed * Time.deltaTime;
            else
                totalTouchingTime = 0f;
        }

    }

}
