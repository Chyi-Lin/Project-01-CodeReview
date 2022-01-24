using System;
using TMPro;
using UnityEngine;

public class GameOverNotificationUI : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float showDuraction = 2.5f;

    [Header("UI")]
    [SerializeField]
    private CanvasGroupUI canvasGroupUI;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private Gradient levelWonColor;

    [SerializeField]
    private Gradient gameOverColor;

    private Action OnCompleteEvent;

    public void SetTest(string content, bool isWon)
    {
        titleText.SetText(content);

        // Color
        VertexGradient vertexGradient = titleText.colorGradient;
        if (isWon)
        {
            vertexGradient.topLeft = levelWonColor.colorKeys[0].color;
            vertexGradient.topRight = levelWonColor.colorKeys[0].color;
            vertexGradient.bottomLeft = levelWonColor.colorKeys[1].color;
            vertexGradient.bottomRight = levelWonColor.colorKeys[1].color;
        }
        else
        {
            vertexGradient.topLeft = gameOverColor.colorKeys[0].color;
            vertexGradient.topRight = gameOverColor.colorKeys[0].color;
            vertexGradient.bottomLeft = gameOverColor.colorKeys[1].color;
            vertexGradient.bottomRight = gameOverColor.colorKeys[1].color;
        }

        titleText.colorGradient = vertexGradient;
    }

    public void Show(float delayTime, Action completeEvent)
    {
        OnCompleteEvent = completeEvent;

        Invoke("ShowNotification", delayTime);
    }

    private void ShowNotification()
    {
        canvasGroupUI.Show();
        _animator.enabled = true;

        Invoke("CloseNotification", showDuraction);
    }

    private void CloseNotification()
    {
        _animator.SetTrigger("PopOut");
    }

    public void AnimationComplete()
    {
        OnCompleteEvent?.Invoke();
        canvasGroupUI.Hide();

        _animator.enabled = false;
    }

}
