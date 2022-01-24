using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text levelText;

    [SerializeField]
    private BoundedInt levelData;

    private void Start()
    {
        levelText.SetText(levelData.Value.ToString());
    }

}
