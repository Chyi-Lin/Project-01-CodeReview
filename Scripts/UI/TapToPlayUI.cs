using TMPro;
using UnityEngine;

public class TapToPlayUI : MonoBehaviour
{
    [Header("TapToPlay UI")]
    [SerializeField]
    private CanvasGroupUI groupUI;

    [SerializeField]
    private TMP_Text _text;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = 1.15f;

    [SerializeField]
    private Vector3 changesScale = Vector3.one * 1.25f;

    [SerializeField]
    private Color changeColor = Color.white;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = FindObjectOfType<GameInput>();
    }

    private void Start()
    {
        PlayAnimation();
    }

    private void OnEnable()
    {
        gameInput.OnTouchDown += StopAnimation;
    }

    private void OnDisable()
    {
        gameInput.OnTouchDown -= StopAnimation;
    }

    private void OnDestroy()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(_text.rectTransform);
        TweenUI.CheckSameSequenceIsPlayAndDone(_text);
    }

    private void PlayAnimation()
    {
        TweenUI.TweenScale(_text.rectTransform, changesScale, changeDuraction, DG.Tweening.Ease.Linear, -1);
        TweenUI.TweenTextColor(_text, changeColor, changeDuraction, DG.Tweening.Ease.Linear, -1);
    }

    private void StopAnimation()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(_text.rectTransform);
        TweenUI.CheckSameSequenceIsPlayAndDone(_text);

        groupUI.Hide();
        enabled = false;

        // Read
        GameManager.playerIsReady = true;
    }

}
