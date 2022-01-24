using UnityEngine;
using UnityEngine.UI;

public class PlayerControlUI : MonoBehaviour
{
    [Header("Left Control")]
    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private WindOutput leftWindOutput;

    [Header("Right Control")]
    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private WindOutput rightWindOutput;

    private void Start()
    {
        leftButton.onClick.AddListener(leftWindOutput.CreateForceObj);
        rightButton.onClick.AddListener(rightWindOutput.CreateForceObj);
    }

}
