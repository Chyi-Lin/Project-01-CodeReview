using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private CustomizeColorData customizeColorData;

    private Renderer _renderer;

    private MaterialPropertyBlock _propBlock;

    public Color PropertyColor { get; set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();

        PropertyColor = (customizeColorData != null)? GetCustomizeColor() : GetRendomColor();
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", PropertyColor);
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void InitCustomizeSetting()
    {
        _propBlock.SetColor("_Color", PropertyColor);
        _renderer.SetPropertyBlock(_propBlock);
    }

    private Color GetRendomColor()
    {
        return Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
    }

    private Color GetCustomizeColor()
    {
        int count = customizeColorData.colorDatas.Count;

        return customizeColorData.colorDatas[Random.Range(0, count)].color;
    }
}
