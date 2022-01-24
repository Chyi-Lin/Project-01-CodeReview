using System;
using UnityEngine;

public abstract class PointsOfInterest : MonoBehaviour
{
    public static event Action<IArchievementData> PointsOfInterestEvent;

    [SerializeField]
    private IArchievementData archievementData;

    public void Notify()
    {
        PointsOfInterestEvent?.Invoke(archievementData);
    }

}
