using UnityEngine;

public class PulseColor : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.5f;

    [SerializeField]
    private Color maxPulse;

    [SerializeField]
    private Color minPulse;

    private Renderer _renderer;

    private MaterialPropertyBlock _propBlock;

    private float normalize = 0;

    private Color originColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        _renderer.GetPropertyBlock(_propBlock);
        originColor = _propBlock.GetColor("_EmissionColor");
    }

    private void OnDisable()
    {
        _propBlock.SetColor("_EmissionColor", originColor);
        _renderer.SetPropertyBlock(_propBlock);
    }

    private void Update()
    {
        normalize += speed * Time.deltaTime;

        if (normalize > 1f)
        {
            speed = -Mathf.Abs(speed);
        }

        if(normalize < 0f)
        {
            speed = +Mathf.Abs(speed);
        }

        _propBlock.SetColor("_EmissionColor", Color.Lerp(minPulse, maxPulse, normalize));
        _renderer.SetPropertyBlock(_propBlock);
    }
}
