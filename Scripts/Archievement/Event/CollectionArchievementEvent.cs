public class CollectionArchievementEvent : PointsOfInterest
{
    private void OnEnable()
    {
        RingTarget.OnCompleteTargetEvent += Notify;
    }

    private void OnDisable()
    {
        RingTarget.OnCompleteTargetEvent -= Notify;
    }

}
