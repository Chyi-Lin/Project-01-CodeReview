using System.IO;
using UnityEditor;
using UnityEngine;

public class LocalizedTextEditor : EditorWindow
{
    public string fileName;
    public LocalizationData localizationData;
    public Vector2 scrollViewPosition;

    [MenuItem("Window/Localized Text Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(LocalizedTextEditor)).Show();
    }

    private void OnGUI()
    {
        Vector2 size = EditorWindow.GetWindow(typeof(LocalizedTextEditor)).maxSize;
        scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);

        if (localizationData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);

            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.textColor = Color.white;
            style.fontSize = 18;
            SerializedProperty fileNameProperty = serializedObject.FindProperty("fileName");
            if(string.IsNullOrEmpty(fileName))
                EditorGUILayout.LabelField("New data.json", style);
            else
            EditorGUILayout.LabelField(fileNameProperty.stringValue, style);

            SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();

            if(!string.IsNullOrEmpty(fileName))
                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }

            if (GUILayout.Button("Save new data"))
            {
                SaveGameNewData();
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }

        if (GUILayout.Button("Create new data"))
        {
            CreateNewData();
        }

        GUILayout.EndScrollView();
    }

    private void LoadGameData()
    {
        string filePath = EditorUtility.OpenFilePanel("Selelct localization data file", Application.streamingAssetsPath, "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            fileName = Path.GetFileName(filePath);

            string dataAsJson = File.ReadAllText(filePath);

            localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        }
    }

    private void SaveGameNewData()
    {
        string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            fileName = Path.GetFileName(filePath);

            string dataAsJson = JsonUtility.ToJson(localizationData);

            File.WriteAllText(filePath, dataAsJson);
        }
    }

    private void SaveGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = JsonUtility.ToJson(localizationData);

            File.WriteAllText(filePath, dataAsJson);
        }
    }

    private void CreateNewData()
    {
        localizationData = new LocalizationData();

        fileName = null;
    }
}
