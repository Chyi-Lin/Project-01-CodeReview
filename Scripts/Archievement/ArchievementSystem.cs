using UnityEngine;

public class ArchievementSystem : MonoBehaviour
{

    private void OnEnable()
    {
        PointsOfInterest.PointsOfInterestEvent += PointsOfInterestWithEvent;
    }

    private void OnDisable()
    {
        PointsOfInterest.PointsOfInterestEvent -= PointsOfInterestWithEvent;
    }

    private void PointsOfInterestWithEvent(IArchievementData data)
    {
        data.IsReached();
    }
}
