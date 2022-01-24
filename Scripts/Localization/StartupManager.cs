using System.Collections;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }
    }

}
