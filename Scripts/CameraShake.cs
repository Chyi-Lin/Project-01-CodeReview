using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originlPos;
    private Coroutine shakeCor;

    private void Awake()
    {
        originlPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeCor != null)
        {
            StopCoroutine(shakeCor);
            shakeCor = null;
        }

        shakeCor = StartCoroutine(ShakeEffect(duration, magnitude));
    }

    private IEnumerator ShakeEffect(float duration, float magnitude)
    {
        transform.localPosition = originlPos;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originlPos.z);

            elapsed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originlPos;
        shakeCor = null;
    }
}
