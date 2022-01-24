using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceHandler : MonoBehaviour
{
    [SerializeField]
    private float force = 50f;

    [SerializeField]
    private string targetTag;

    private Vector3 forceDir;

    public Vector3 SetForceDir { set { this.forceDir = value; } }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Ring ring = other.GetComponent<Ring>();
            ring.AddForce(forceDir, force);
        }
    }

}
