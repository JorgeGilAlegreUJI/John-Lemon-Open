using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathfindingMaster : BaseScript
{
	public Dictionary<Waypoint, List<Waypoint>> graph = new Dictionary<Waypoint, List<Waypoint>>();
	public Transform WaypointsParent;


    [ContextMenu("AssignAdjacents")]
    public void AssignAdjacents()
    {
        foreach(Transform t in WaypointsParent)
        {
            if(t.GetComponent<Collider>().enabled == false)
            {
                Debug.LogError("Cant assign adjacents if colliders are disabled");
                return;
            }
            Waypoint w = t.GetComponent<Waypoint>();
            w.AssignAdjacents();
        }

        Debug.Log("Adjacents assigned");
    }

    [ContextMenu("CreateGraph")]
    void CreateGraph()
    {
        graph.Clear();
        // Construct the graph
        foreach (Transform t in WaypointsParent)
        {
            Waypoint w = t.GetComponent<Waypoint>();
            List<Waypoint> edges = new List<Waypoint>();


            foreach (Waypoint e in w.Adjacents)
            {
                edges.Add(e);
            }
            graph.Add(w, edges);
        }

        Debug.Log("Graph constructed");

    }

    [ContextMenu("SetWaypointsVisibilityAndPhysics")]
    void SetWaypointsVisibilityAndPhysics()
    {
        foreach(Transform t in WaypointsParent)
        {
            t.GetComponent<MeshRenderer>().enabled = !t.GetComponent<MeshRenderer>().enabled;
            t.GetComponent<Collider>().enabled = !t.GetComponent<Collider>().enabled;
        }

        Debug.Log("Visibility and Physics switched");
    }

    public void SetWaypointsVisibilityAndPhysics(bool value)
    {
        foreach (Transform t in WaypointsParent)
        {
            t.GetComponent<MeshRenderer>().enabled = value;
            t.GetComponent<Collider>().enabled = value;
        }

        Debug.Log("Visibility and Physics switched");
    }


    [ContextMenu("FindIsles")]
    void FindIsles() // En caso de necesitar encontar Islas
    {
        //List<GameObject> ToDestroy = new List<GameObject>();

        //foreach (Transform t in WaypointsParent)
        //{
        //    Waypoint w = t.GetComponent<Waypoint>();
        //    List<Waypoint> path = getPath(w, WaypointsParent.GetChild(3100).GetComponent<Waypoint>(), ref ToDestroy);
        //    if (path != null) Debug.Log(t.name + " OK ");


        //}

        //for (int i = ToDestroy.Count - 1; i >= 0; i--)
        //{
        //    DestroyImmediate(ToDestroy[i]);
        //}
    }

    private void Awake()
    {
        base.Awake();
        SetWaypointsVisibilityAndPhysics(true);
        CreateGraph();
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
        SetWaypointsVisibilityAndPhysics(false);
    }

    // Calculates the path towards the goal waypoints, using A* search.
    public List<Waypoint> getPath(Waypoint start, Waypoint goal)
    {
        List<Waypoint> path = new List<Waypoint>();

        if (start == goal)
        {
            path.Add(start);
            return path;
        }

        SortedList<float, Waypoint> frontier = new SortedList<float, Waypoint>();
        Dictionary<Waypoint, Waypoint> visitedFrom = new Dictionary<Waypoint, Waypoint>();
        Dictionary<Waypoint, float> g = new Dictionary<Waypoint, float>(); // costsFromStart

        visitedFrom.Add(start, null);
        g.Add(start, 0);
        frontier.Add(Vector3.Distance(start.transform.position, goal.transform.position), start);

        while (frontier.Count > 0)
        {
            Waypoint current = frontier.Values[0];
            frontier.RemoveAt(0);

            if (current == goal)
                break;

            //if (!graph.ContainsKey(current)) { Debug.LogError("1"); return null; } se ejecuta cuando el grafo no se ha elaborado de manera correcta
            foreach (Waypoint next in graph[current])
            {
                float newG = g[current] + Vector3.Distance(next.transform.position, current.transform.position);
                if (!g.ContainsKey(next) || newG < g[next])
                {
                    if (frontier.ContainsValue(next))
                    {
                        frontier.RemoveAt(frontier.IndexOfValue(next)); // se ejecuta cuando el path es imposible
                    }

                    float NewKey = newG + Vector3.Distance(next.transform.position, goal.transform.position);


                    if (frontier.ContainsKey(NewKey)) continue; // si existen dos rutas con el mismo coste, la segunda es ignorada

                    frontier.Add(NewKey, next);

                    if (visitedFrom.ContainsKey(next))
                    {
                        visitedFrom.Remove(next);
                    }
                    visitedFrom.Add(next, current);

                    if (g.ContainsKey(next))
                        g.Remove(next);
                    g.Add(next, newG);
                }
            }
        }

        // Return the path to the goal
        Waypoint w = goal;
        path.Add(goal);

        //if (!visitedFrom.ContainsKey(w)) { Debug.LogError(start.transform.name + " Isle"); /*ToDestroy.Add(start.gameObject);*/ return null; } // Para detectar islas
        while (visitedFrom[w] != null)
        {
            path.Add(visitedFrom[w]);
            w = visitedFrom[w];
        }
        path.Reverse();
        return path;
    }
}