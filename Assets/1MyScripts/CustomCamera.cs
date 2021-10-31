using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : BaseScript
{
    public Vector3 Offset;

    public void Update()
    {
        base.Update();
        Movement();
    }

    void Movement()
    {
        Vector3 NewPos = MyReferencesPool.MyProtagonist.transform.position + Offset;
        transform.position = NewPos;
        



    }
}
