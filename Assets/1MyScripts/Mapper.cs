using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : Utilities
{
    public float CubeScale = 1f;
    public GameObject VisualCubePrefab;
    public Material BlackMat;
    public Material YellowMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        
    }


    [ContextMenu("Map")]
    void CreateMap()
    {
        Transform AreaQuad = transform.Find("AreaQuad");
        MeshFilter MF = AreaQuad.GetComponent<MeshFilter>();

        Matrix4x4 localToWorld = AreaQuad.localToWorldMatrix;
        Vector3[] Bounds = new Vector3[MF.mesh.vertices.Length];

        for (int i = 0; i < MF.mesh.vertices.Length; ++i)
        {
            Vector3 world_v = localToWorld.MultiplyPoint3x4(MF.mesh.vertices[i]);
            Bounds[i] = world_v;
        }

        Transform WaypointsParent = transform.Find("Waypoints");
        StartCoroutine(Createwaypoints(0,Bounds[0],Bounds,WaypointsParent));

    }



    IEnumerator Createwaypoints(int i ,Vector3 Position,Vector3[] Bounds, Transform WaypointsParent)
    {
        RaycastHit hit;
        float Height = 100f;
        Vector3 point = Position + Vector3.up*Height;

        if (Physics.Raycast(point, Vector3.down, out hit, Height + 10f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.tag == "Floor")
            {
                GameObject Cube = Instantiate(VisualCubePrefab, Position, Quaternion.identity);
                Cube.transform.name += WaypointsParent.childCount;
                Cube.transform.localScale = Vector3.one * CubeScale;
                Cube.transform.SetParent(WaypointsParent);
                if (i % 2 == 0) Cube.GetComponent<Renderer>().material = BlackMat;
                else Cube.GetComponent<Renderer>().material = YellowMat;
            }
            else Debug.Log(hit.transform.name);


        }






        Vector3 NextPosition = Position - Vector3.right * CubeScale;
        if (NextPosition.x < Bounds[1].x)
        {
            NextPosition = Position + Vector3.back * CubeScale;
            NextPosition.x = Bounds[0].x;
        }

        if (NextPosition.z < Bounds[3].z)
        {
            yield return null;
            Debug.Log("Mapping finished");
        }
        else
        {
            i++;
            yield return new WaitForSeconds(Time.deltaTime/8);
            StartCoroutine(Createwaypoints(i,NextPosition, Bounds, WaypointsParent));
        }

        

    }

}
