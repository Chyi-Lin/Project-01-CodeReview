using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    public enum WayTag
    {
        Low, Normal, High
    }

    [System.Serializable]
    public class WayDetail
    {
        public WayTag wayTag;
        public Transform[] points;
    }

    [SerializeField]
    private Transform[] shareWayPoints;

    [SerializeField]
    private WayDetail[] wayDetails;

    private Dictionary<WayTag, List<Transform>> wayPoints = new Dictionary<WayTag, List<Transform>>();

    public Transform[] GetWayPoints(WayTag wayTag)
    {
        return wayPoints[wayTag].ToArray();
    }

    // 路徑位置
    //public static Transform[] points;

    private void Awake()
    {
        //points = new Transform[transform.childCount];
        //for (int i = 0; i < points.Length; i++)
        //{
        //    points[i] = transform.GetChild(i);
        //}

        
        for (int i = 0; i < wayDetails.Length; i++)
        {
            List<Transform> transforms = new List<Transform>();
            transforms.AddRange(wayDetails[i].points);
            transforms.AddRange(shareWayPoints);

            wayPoints.Add(wayDetails[i].wayTag, transforms);
        }
    }
}
