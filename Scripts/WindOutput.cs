using UnityEngine;

public class WindOutput : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem bubbleEffect;

    [SerializeField]
    private ForceMove[] forceMoves;

    public void CreateForceObj()
    {
        bubbleEffect.Play();

        for (int i = 0; i < forceMoves.Length; i++)
        {
            GameObject forceObj = Instantiate(forceMoves[i].forceObject, transform.position, Quaternion.identity);
            
            RouteMovement movement = forceObj.GetComponent<RouteMovement>();
            movement.SetRoutePositions(forceMoves[i].moveRoute.GetRoute);
            movement.SetMoveTime(forceMoves[i].moveTime);
            movement.StartMove();
        }
    }

}
