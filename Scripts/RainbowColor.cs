using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    [SerializeField]
    private float speed = .25f;

    [SerializeField]
    private ParticleSystem rainbowColor;
    private ParticleSystemRenderer rainbowColorRenderer;

    private Renderer _renderer;

    private MaterialPropertyBlock _propBlock;

    private float colorValue;
    private Color originColor;

    public Color PropertyColor { get; set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propBlock);
        originColor = _propBlock.GetColor("_Color");

        rainbowColorRenderer = rainbowColor.GetComponent<ParticleSystemRenderer>();
    }

    private void OnEnable()
    {
        colorValue = Random.Range(0f, 1f);
    }

    private void OnDisable()
    {
        _propBlock.SetColor("_Color", originColor);
        _renderer.SetPropertyBlock(_propBlock);
    }

    private void Update()
    {
        colorValue += speed * Time.deltaTime;

        if(colorValue >= 1)
        {
            speed = -Mathf.Abs(speed);
        }

        if (colorValue <= 0)
        {
            speed = +Mathf.Abs(speed);
        }

        _propBlock.SetColor("_Color", Color.HSVToRGB(colorValue, 1f, 1f));
        _renderer.SetPropertyBlock(_propBlock);
        rainbowColorRenderer.SetPropertyBlock(_propBlock);

    }
}
