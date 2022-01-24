using TMPro;
using UnityEngine;

public class FeverTimeUI : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField]
    private TMP_Text feverTimeText;

    [Header("Change Animation")]
    [SerializeField]
    private float changeDuraction = .25f;

    [SerializeField]
    private Vector3 changeScale = Vector3.one * 1.25f;

    private void OnEnable()
    {
        feverTimeText.enabled = true;
        TweenUI.TweenScale(feverTimeText.rectTransform, changeScale, changeDuraction, loops: -1);
    }

    private void OnDisable()
    {
        TweenUI.CheckSameSequenceIsPlayAndDone(feverTimeText.rectTransform);
        feverTimeText.enabled = false;
    }
}
