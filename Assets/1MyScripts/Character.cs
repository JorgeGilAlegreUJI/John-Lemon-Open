using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObject
{
    public Waypoint CurrentWaypoint;

    public virtual Waypoint GetClosestWaypoint()
    {
        float MinDis = float.MaxValue;
        Waypoint closest = null;

        foreach (Transform t in MyReferencesPool.MyPathFindindMaster.WaypointsParent)
        {
            float dis = Vector3.Distance(transform.position, t.position);
            if (dis < MinDis)
            {
                MinDis = dis;
                closest = t.GetComponent<Waypoint>();
            }

        }

        return closest;
    }
}
