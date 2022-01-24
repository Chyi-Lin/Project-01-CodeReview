using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMovement : MonoBehaviour
{

    [SerializeField]
    private ForceHandler forceHandler;

    private float moveTime = 1f;

    private List<Vector3> positions;
    private Coroutine moveCor;

    private Transform _transform;

    public void SetRoutePositions(List<Vector3> positions) => this.positions = positions;

    public void SetMoveTime(float time) => this.moveTime = time;

    private void Awake()
    {
        _transform = transform;
    }


    public void StartMove()
    {
        if (positions.Count == 0)
            return;

        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }

        StartCoroutine(Move(positions, moveTime));

    }

    private IEnumerator Move(List<Vector3> positions, float time) 
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        _transform.position = positions[0];
        float fragmentTime = time / positions.Count;
        float currentTime = 0f;

        for (int i = 1; i < positions.Count; i++)
        {
            forceHandler.SetForceDir = (positions[i] - _transform.position).normalized;

            while (currentTime < fragmentTime)
            {
                _transform.position = Vector3.Lerp(_transform.position, positions[i], currentTime / fragmentTime);
                currentTime += Time.deltaTime;

                yield return waitForEndOfFrame;
            }

            currentTime = 0;
        }

        Destroy(gameObject);
        yield return null;
    }

}
