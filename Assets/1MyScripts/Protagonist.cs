using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : Character
{


    public void Start()
    {
        base.Start();
    }

    public void Update()
    {
        base.Update();
        Waypoint closest = GetClosestWaypoint();

        if(closest != CurrentWaypoint)
        {
            CurrentWaypoint = closest;

            Ghost FirstGhost = MyReferencesPool.Ghosts.transform.GetChild(0).GetComponent<Ghost>();
            if (FirstGhost.CurrentState == Ghost.State.Chase)
            {
                foreach(Transform tr in MyReferencesPool.Ghosts.transform)
                {
                    tr.GetComponent<Ghost>().GotoCurrentPlayerPosition();
                }
            }

        }





    }





}
