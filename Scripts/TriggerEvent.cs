using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public delegate void OnTriggerEvent(Collider collider);

    public event OnTriggerEvent OnTriggerEnterEvent;

    public event OnTriggerEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }

}
