public class OnceArchievementEvent : PointsOfInterest
{
    private void OnEnable()
    {
        GameManager.OnCompleteLevel += Notify;
    }

    private void OnDisable()
    {
        GameManager.OnCompleteLevel -= Notify;
    }
}
