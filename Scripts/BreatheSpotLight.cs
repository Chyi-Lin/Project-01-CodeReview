using UnityEngine;

public class BreatheSpotLight : MonoBehaviour
{
    [SerializeField]
    private Light _light;

    [SerializeField]
    private float speed = .5f;

    [SerializeField]
    private float maxIntensity = 1.5f;

    [SerializeField]
    private float minInstensity = 1f;

    private float currentInstensity;

    private void Start()
    {
        currentInstensity = _light.intensity;
    }

    private void Update()
    {
        currentInstensity += speed * Time.deltaTime;
        _light.intensity = currentInstensity;

        if (currentInstensity > maxIntensity)
        {
            speed = -Mathf.Abs(speed);
        }

        else if (currentInstensity < minInstensity)
        {
            speed = +Mathf.Abs(speed);
        }
    }
}
