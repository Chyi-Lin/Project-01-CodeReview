using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private PassEvent OnShowEvent;

    [SerializeField]
    private PassEvent OnHideEvent;

    [ContextMenu("Show")]
    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        OnShowEvent?.Invoke();
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        OnHideEvent?.Invoke();
    }
}
