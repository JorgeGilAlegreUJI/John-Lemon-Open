using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : BaseScript
{
    public Color RouteColor;
    private void OnDrawGizmos()
    {
        foreach(Transform tr in transform)
        {
            Gizmos.color = RouteColor;
            Gizmos.DrawSphere(tr.position, 0.25f);
            Vector3 EndPoint = transform.GetChild(0).position; 

            if (tr.GetSiblingIndex() != transform.childCount - 1) EndPoint = transform.GetChild(tr.GetSiblingIndex() + 1).position;

            Gizmos.DrawLine(tr.position,EndPoint );
        }
    }

    public List<Waypoint> GetWaypointsList()
    {
        List<Waypoint> results = new List<Waypoint>();

        foreach (Transform tr in transform)
        {
            Vector3 point = tr.position;
            point.y = 0f;
            RaycastHit hit;
            if (Physics.Raycast(tr.position + Vector3.up, Vector3.down, out hit, 2f, LayerMask.GetMask("Waypoints"), QueryTriggerInteraction.Ignore))
            {
                Waypoint wp = hit.transform.GetComponent<Waypoint>();
                if (wp) results.Add(wp);
            }
            else
            {
                Debug.LogError(name + " Has a point out of the map ");
            }
        }

        return results;
    }
}
