using System.Collections;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private IEnumerator Start()
    {
        if (LocalizationManager.instance == null)
            yield break;

        if (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }

        Reflush();
    }

    public void Reflush()
    {
        text.SetText(LocalizationManager.instance.GetLoaclizedValue(key));
    }
}
