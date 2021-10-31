using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingRoutes : BaseScript
{
    private void Start()
    {
        base.Start();
        StartCoroutine(ActivateChaseMode());
    }

    public Route GetRoute(int id)
    {
        Route r = transform.GetChild(id).GetComponent<Route>();
        return r;
    }

    IEnumerator ActivateChaseMode()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("chase mode ON");
        foreach(Transform tr in MyReferencesPool.Ghosts.transform)
        {
            Ghost g = tr.GetComponent<Ghost>();
            g.CurrentState = Ghost.State.Chase;
            g.GotoCurrentPlayerPosition();

        }

    }
}
