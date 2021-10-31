using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesPool : BaseScript
{
    public Protagonist MyProtagonist;
    public PathfindingMaster MyPathFindindMaster;
    public PatrolingRoutes MyPatrolingRoutes;
    public GameObject Ghosts;


    public override void AssingReferencesPool() { MyReferencesPool = this; }
}
