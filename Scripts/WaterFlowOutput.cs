using UnityEngine;

public class WaterFlowOutput : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bubbleEffect;
    private ParticleSystem.MainModule mainModule;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = FindObjectOfType<GameInput>();
        mainModule = bubbleEffect.main;
    }

    private void OnEnable()
    {
        gameInput.OnTouchDown += PlayEffect;
        gameInput.OnTouchingOverTime += StopEffect;
        gameInput.OnTouchUp += StopEffect;
        GameManager.OnCompleteLevel += StopEffect;
    }

    private void OnDisable()
    {
        gameInput.OnTouchDown -= PlayEffect;
        gameInput.OnTouchingOverTime -= StopEffect;
        gameInput.OnTouchUp -= StopEffect;
        GameManager.OnCompleteLevel -= StopEffect;
    }

    private void PlayEffect()
    {
        mainModule.loop = true;
        bubbleEffect.Play();
    }

    private void StopEffect()
    {
        mainModule.loop = false;
    }
}
