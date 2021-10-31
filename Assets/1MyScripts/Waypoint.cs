using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : Utilities
{
    public List<Waypoint> Adjacents = new List<Waypoint>();

    private void Awake()
    {
        base.Awake();
        
    }

    public void AssignAdjacents()
    {
        Adjacents.Clear();
        float Scale = transform.localScale.x;

        for (int i = 0; i < 8; i++)
        {
            switch (i)
            {
                case 0:
                    CheckSide(transform.position + Vector3.forward*Scale + Vector3.left * Scale);
                    break;
                case 1:
                    CheckSide(transform.position + Vector3.forward * Scale);
                    break;
                case 2:
                    CheckSide(transform.position + Vector3.forward * Scale + Vector3.right * Scale);
                    break;
                case 3:
                    CheckSide(transform.position + Vector3.left * Scale);
                    break;
                case 4:
                    CheckSide(transform.position + Vector3.right * Scale);
                    break;
                case 5:
                    CheckSide(transform.position + Vector3.forward*-1 * Scale + Vector3.left * Scale);
                    break;
                case 6:
                    CheckSide(transform.position + Vector3.forward*-1 * Scale);
                    break;
                case 7:
                    CheckSide(transform.position + Vector3.forward*-1 * Scale + Vector3.right * Scale);
                    break;
                default:
                    break;
            }
        }



    }

    void CheckSide(Vector3 Target)
    {
        RaycastHit hit;
        float Height = 2f;
        Vector3 point = transform.position + Vector3.up * Height;
        Vector3 dir = Target - point;

        if (Physics.Raycast(point, dir.normalized, out hit, Height*2, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Waypoint wp = hit.transform.GetComponent<Waypoint>();
            if (wp) Adjacents.Add(wp);


        }
    }

    public Vector3 FeetPos() { return new Vector3(transform.position.x, 0f, transform.position.z); }

}