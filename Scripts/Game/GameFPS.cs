using UnityEngine;

public class GameFPS : MonoBehaviour
{
    [SerializeField]
    private int frameRate = 60;

    void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
