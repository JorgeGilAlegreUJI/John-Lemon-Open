using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : BaseScript
{
    [HideInInspector]public float BornTime = 0;


    public void Awake()
    {
        base.Awake();
        BornTime = Time.time;
    }

    public virtual float LifeTime() { return Time.time - BornTime; }


}
