using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManagerEditor : OdinMenuEditorWindow
{
    [OnValueChanged("StateChange")]
    [LabelText("Manager View")]
    [LabelWidth(100f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private ManagerState managerState;
    private int enumIndex = 0;
    private bool treeRebuild = false;

    // 資料
    private DrawSelected<CustomizeColorData> drawCustomizeItem = new DrawSelected<CustomizeColorData>();
    private DrawSelected<IArchievementData> drawArchievementItem = new DrawSelected<IArchievementData>();
    private DrawSelected<GameLevel> drawLevels = new DrawSelected<GameLevel>();
    private DrawSelected<PowerUpsItem> drawPowerUpItems = new DrawSelected<PowerUpsItem>();

    // 資料路徑
    private string customizePath = "Assets/RingToss/ScriptableObjects/CustomizeColor";
    private string archievementPath = "Assets/RingToss/ScriptableObjects/GameArchievement";
    private string levelPath = "Assets/RingToss/ScriptableObjects/GameLevels";
    private string powerupsPath = "Assets/RingToss/ScriptableObjects/Shop";

    [MenuItem("Tools/Game Manager Window")]
    public static void OpenWindow()
    {
        GetWindow<GameManagerEditor>().Show();
    }

    /// <summary>
    /// 狀態改變
    /// </summary>
    private void StateChange()
    {
        treeRebuild = true;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Initialize()
    {
        // 設定路徑
        drawCustomizeItem.SetPath(customizePath);
        drawArchievementItem.SetPath(archievementPath);
        drawLevels.SetPath(levelPath);
        drawPowerUpItems.SetPath(powerupsPath);
    }

    /// <summary>
    /// 繪製GUI
    /// </summary>
    protected override void OnGUI()
    {
        // 如果資料變更且是當前佈局
        if(treeRebuild && Event.current.type == EventType.Layout)
        {
            // 更新菜單
            ForceMenuTreeRebuild();
            treeRebuild = false;
        }

        // 設定標題與副標
        SirenixEditorGUI.Title("Game Manager Window", "More Toss More Rings", TextAlignment.Center, true);
        // 間隔
        EditorGUILayout.Space();

        // 繪製編輯器
        switch (managerState)
        {
            case ManagerState.CustomizeColor:
            case ManagerState.Archievement:
            case ManagerState.Level:
            case ManagerState.Shop:
                DrawEditor(enumIndex);
                break;
            default:
                break;
        }
        // 間隔
        EditorGUILayout.Space();

        base.OnGUI();
    }

    /// <summary>
    /// 繪製編輯器
    /// </summary>
    protected override void DrawEditors()
    {
        // 繪製選擇的 ScriptableObject
        switch (managerState)
        {
            case ManagerState.CustomizeColor:
                //DrawEditor(enumIndex);
                break;
            case ManagerState.Archievement:
                drawArchievementItem.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.Level:
                drawLevels.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.Shop:
                drawPowerUpItems.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            default:
                break;
        }

        // 繪製 DrawSelected的 ScriptableObject
        DrawEditor((int)managerState);
    }

    /// <summary>
    /// 繪製菜單樹選擇
    /// </summary>
    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();
        targets.Add(null);
        targets.Add(drawArchievementItem);
        targets.Add(drawLevels);
        targets.Add(drawPowerUpItems);
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }

    /// <summary>
    /// 繪製菜單
    /// </summary>
    protected override void DrawMenu()
    {
        switch (managerState)
        {
            case ManagerState.CustomizeColor:
            case ManagerState.Archievement:
            case ManagerState.Level:
            case ManagerState.Shop:
                base.DrawMenu();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 建立菜單樹，將所有資源添加路徑
    /// </summary>
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        switch (managerState)
        {
            case ManagerState.CustomizeColor:
                tree.AddAllAssetsAtPath("Customize Data", customizePath, typeof(CustomizeColorData));
                break;
            case ManagerState.Archievement:
                tree.AddAllAssetsAtPath("Archievement Data", archievementPath, typeof(IArchievementData));
                break;
            case ManagerState.Level:
                tree.AddAllAssetsAtPath("Level Data", levelPath, typeof(GameLevel));
                break;
            case ManagerState.Shop:
                tree.AddAllAssetsAtPath("Shop Data", powerupsPath, typeof(PowerUpsItem));
                break;
            default:
                break;
        }

        return tree;
    }

    /// <summary>
    /// 管理器狀態
    /// </summary>
    public enum ManagerState
    {
        CustomizeColor,
        Archievement,
        Level,
        Shop,
    }
}

public class DrawSelected<T> where T : ScriptableObject
{
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T selected;

    [LabelWidth(100)]
    [PropertyOrder(-1)]
    [ColorGroupAttribute("CreateNew", 1f, 1f, 1f)]
    [HorizontalGroup("CreateNew/Horizontal")]
    public string nameForNew;

    private string path;

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(.7f, .7f, 1f)]
    [Button]
    public void CreateNew()
    {
        if (nameForNew == "")
            return;

        T newItem = ScriptableObject.CreateInstance<T>();
        //newItem.name = "New " + typeof(T).ToString();

        if (path == "")
            path = "Assets/";

        AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
        AssetDatabase.SaveAssets();

        nameForNew = "";
    }

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(1f, 0.7f, 0.7f)]
    [Button]
    public void DeleteSelected()
    {
        if(selected != null)
        {
            string _path = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.DeleteAsset(_path);
            AssetDatabase.SaveAssets();
        }
    }

    public void SetSelected(object item)
    {
        var attempt = item as T;
        if (attempt != null)
            this.selected = attempt;
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}

public class ColorGroupAttribute : PropertyGroupAttribute
{
    public float R, G, B;

    public ColorGroupAttribute(string path) : base(path)
    {

    }

    public ColorGroupAttribute(string path, float r, float g, float b) : base(path)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }
}

public class DrawSceneObject<T> where T : MonoBehaviour
{
    [Title("Universe Creator")]
    [ShowIf("@myObject != null")]
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T myObject;

    public void FindMyObject()
    {
        if (myObject == null)
            myObject = GameObject.FindObjectOfType<T>();
    }

    [ShowIf("@myObject != null")]
    [GUIColor(.7f, 1f, .7f)]
    [ButtonGroup("Top Button", -1000)]
    private void SelectSceneObject()
    {
        if (myObject != null)
            Selection.activeGameObject = myObject.gameObject;
    }

    [ShowIf("@myObject == null")]
    [Button]
    private void CreateManagerObject()
    {
        GameObject newManager = new GameObject();
        newManager.name = "New " + typeof(T).ToString();
        myObject = newManager.AddComponent<T>();
    }
}
