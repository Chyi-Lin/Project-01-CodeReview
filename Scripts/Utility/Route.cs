using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{

    [SerializeField]
    private Transform[] controlPoints;

    [SerializeField]
    private List<Vector3> savePositions;

    private Vector3 gizmosPosition;

    public List<Vector3> GetRoute { get { return this.savePositions; } }

    [ContextMenu("Save Route")]
    private void SaveRoute()
    {
        savePositions.Clear();

        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition =
                Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;

            savePositions.Add(gizmosPosition);
        }
    }

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t+= 0.05f)
        {
            gizmosPosition =
                Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawWireSphere(gizmosPosition, 0.15f);
        }

        Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);

        Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);

    }

}
