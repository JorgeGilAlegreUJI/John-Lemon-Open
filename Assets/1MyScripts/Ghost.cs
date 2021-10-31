using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{

    public enum State { Patrol, Investigate, Chase }
    public State CurrentState = State.Patrol;
    public float MinTreshHold;
    public float MovementSpeed;
    public float RotationSpeed;

    public List<Waypoint> PatrolWaypoints = new List<Waypoint>();
    int PatrolIndex = 0; 

    int CurrentPathIndex = 0;
    List<Waypoint> CurrentPath = new List<Waypoint>();

    void LoadPath(List<Waypoint> path)
    {
        CurrentPathIndex = 0;
        CurrentPath = path;
    }


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        CurrentWaypoint = GetClosestWaypoint();
        PatrolWaypoints = MyReferencesPool.MyPatrolingRoutes.GetRoute(transform.GetSiblingIndex()).GetWaypointsList();
        LoadPath(MyReferencesPool.MyPathFindindMaster.getPath(CurrentWaypoint, PatrolWaypoints[0]));
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        MoveInPath();
    }

    void MoveInPath()
    {
        if (CurrentPath == null) return;
        Waypoint TargetWP = CurrentPath[CurrentPathIndex];
        Rotation(TargetWP);
        float dis = Vector3.Distance(transform.position, TargetWP.FeetPos());

        if (dis >= MinTreshHold)//move
        {
            transform.position += (TargetWP.FeetPos() - transform.position).normalized * MovementSpeed * Time.deltaTime;
        }
        else
        {
            CurrentWaypoint = TargetWP;
            CurrentPathIndex++;
            if(CurrentPathIndex>= CurrentPath.Count)//ha llegado al final de la ruta
            {
                if(CurrentState == State.Patrol)
                {
                    PatrolIndex++;
                    if (PatrolIndex >= PatrolWaypoints.Count) PatrolIndex = 0;
                    LoadPath(MyReferencesPool.MyPathFindindMaster.getPath(CurrentWaypoint, PatrolWaypoints[PatrolIndex]));
                }
                else if(CurrentState == State.Investigate || CurrentState == State.Chase)
                {
                    CurrentPath = null;
                }

                
            }
        }

    }


    public IEnumerator GotoLastPlayerPosition()
    {
        CurrentState = State.Investigate;
        LoadPath(MyReferencesPool.MyPathFindindMaster.getPath(CurrentWaypoint, MyReferencesPool.MyProtagonist.CurrentWaypoint));
        yield return new WaitForSeconds(10f);
        LoadPath(MyReferencesPool.MyPathFindindMaster.getPath(CurrentWaypoint, PatrolWaypoints[0]));
        CurrentState = State.Patrol;

    }

    public void GotoCurrentPlayerPosition()
    {
        LoadPath(MyReferencesPool.MyPathFindindMaster.getPath(CurrentWaypoint, MyReferencesPool.MyProtagonist.CurrentWaypoint));
    }


    void Rotation(Waypoint TargetWP)
    {
        Quaternion NewRot =  Quaternion.LookRotation((TargetWP.FeetPos() - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, NewRot, Time.deltaTime * RotationSpeed);
    }
}
