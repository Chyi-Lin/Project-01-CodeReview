using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguageUI : MonoBehaviour
{
    [System.Serializable]
    public struct ToggleTag
    {
        public Toggle toggle;
        public string fileName;
        public TMP_Text titleText;
    }

    [SerializeField]
    private TMP_Text languageBtnText;

    [SerializeField]
    private ToggleTag[] toggleTags;

    private LocalizationManager manager;
    private string selectFileName;

    private void Start()
    {
        manager = LocalizationManager.instance;

        Reflush();
    }

    public void SelectFileName(string fileName)
    {
        selectFileName = fileName;
    }

    public void SetupLanguage()
    {
        if (manager == null)
            return;

        if (selectFileName == manager.GetSaveFileName())
            return;

        manager.StartLoadLocalizedText(selectFileName);
    }

    public void Reflush()
    {
        if (manager == null)
            return;

        string filenaem = manager.GetSaveFileName();
        for (int i = 0; i < toggleTags.Length; i++)
        {
            if (toggleTags[i].fileName == filenaem)
            {
                toggleTags[i].toggle.isOn = true;
                languageBtnText.SetText(toggleTags[i].titleText.text);
                return;
            }
        }
    }

}
