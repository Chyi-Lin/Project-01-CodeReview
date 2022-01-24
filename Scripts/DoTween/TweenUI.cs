using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TweenUI
{
    /// <summary>
    /// Recode is play sequences.
    /// </summary>
    private static Dictionary<Component, Sequence> sequences = new Dictionary<Component, Sequence>();

    /// <summary>
    /// Tween rect transform scale.
    /// </summary>
    public static void TweenScale(RectTransform rectTransform, Vector3 endVelue, float duration, Ease ease = Ease.OutBounce, int loops = 0)
    {
        CheckSameSequenceIsPlayAndDone(rectTransform);

        Vector3 originScale = rectTransform.localScale;

        Sequence sequence = DOTween.Sequence()
            .Append(rectTransform.DOScale(endVelue, duration).SetEase(ease))
            .Append(rectTransform.DOScale(originScale, duration))
            .SetLoops(loops)
            .OnKill(() =>
            {
                rectTransform.localScale = originScale;
            });

        sequences.Add(rectTransform, sequence);

    }

    /// <summary>
    /// Tween image color.
    /// </summary>
    public static void TweenImageColor(Image image, Color endColor, float duration, Ease ease = Ease.Linear, int loops = 0)
    {
        CheckSameSequenceIsPlayAndDone(image);

        Color originColor = image.color;

        Sequence sequence = DOTween.Sequence()
            .Append(image.DOColor(endColor, duration).SetEase(ease))
            .Append(image.DOColor(originColor, duration).SetEase(ease))
            .SetLoops(loops)
            .OnKill(() =>
            {
                image.color = originColor;
            });

        sequences.Add(image, sequence);

    }

    /// <summary>
    /// Tween text color.
    /// </summary>
    public static void TweenTextColor(TMP_Text text, Color endColor, float duration, Ease ease = Ease.Linear, int loops = 0)
    {
        CheckSameSequenceIsPlayAndDone(text);

        Color originColor = text.color;

        Sequence sequence = DOTween.Sequence()
            .Append(text.DOColor(endColor, duration).SetEase(ease))
            .Append(text.DOColor(originColor, duration).SetEase(ease))
            .SetLoops(loops)
            .OnKill(() =>
            {
                text.color = originColor;
            });

        sequences.Add(text, sequence);

    }

    public static void CheckSameSequenceIsPlayAndDone(Component component)
    {
        if (sequences.ContainsKey(component))
        {
            if(sequences[component] != null)
                sequences[component].Kill();

            sequences.Remove(component);
        }
    }

    public static void StopAllSequence()
    {
        sequences.Clear();
        DOTween.KillAll();
    }

}
