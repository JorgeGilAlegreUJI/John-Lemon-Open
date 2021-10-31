using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [HideInInspector] public ReferencesPool MyReferencesPool;

    public virtual void AssingReferencesPool ()
    {
        MyReferencesPool = FindObjectOfType<ReferencesPool>();
    }

    public void Awake()
    {
        AssingReferencesPool();
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

}
