using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    
    public enum Language { en, ch}

    public Language defaultLanguage = Language.ch;

    public string defaultFileNameFormat = "localizeText_{0}.json";

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";
    private string fileName;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Init();
    }
    
    private void Init()
    {
        fileName = GetSaveFileName();

        StartCoroutine(AsyncLoadLocalizedText(fileName));
    }

    private void ReflushAll()
    {
        LocalizedText[] localizedTexts = FindObjectsOfType<LocalizedText>();
        for (int i = 0; i < localizedTexts.Length; i++)
        {
            localizedTexts[i].Reflush();
        }
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot file file!");
        }

        isReady = true;
    }

    public string GetSaveFileName()
    {
        fileName = PlayerPrefs.GetString(GameKey.OPTION_LANGUAGE, "");
        if (fileName == "")
        {
            if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
                fileName = string.Format(defaultFileNameFormat, defaultLanguage.ToString());
            else
                fileName = string.Format(defaultFileNameFormat, Language.en.ToString());
        }

        return fileName;
    }

    public void StartLoadLocalizedText(string fileName)
    {
        this.fileName = fileName;
        PlayerPrefs.SetString(GameKey.OPTION_LANGUAGE, this.fileName);
        PlayerPrefs.Save();

        StartCoroutine(AsyncLoadLocalizedText(this.fileName));
    }

    public IEnumerator AsyncLoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        UnityWebRequest request = UnityWebRequest.Get(filePath);
        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        string dataAsJson = request.downloadHandler.text;

        if (!string.IsNullOrEmpty(dataAsJson))
        {
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
#if UNITY_EDITOR
            Debug.Log("<color=white>Data loaded, dictionary contains: " + localizedText.Count + " entries</color>");
#endif
        }
        else
        {
            Debug.LogError("Cannot file file!");
        }

        isReady = true;

        ReflushAll();
    }

    public string GetLoaclizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

}
