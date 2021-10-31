using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    ReferencesPool MyReferencesPool;
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;
    bool inCooldown = false;

    private void Awake()
    {
        MyReferencesPool = FindObjectOfType<ReferencesPool>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange && !inCooldown)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    if(transform.parent.GetComponent<Ghost>())gameEnding.CaughtPlayer();
                    foreach(Transform tr in MyReferencesPool.Ghosts.transform)
                    {
                        Ghost ghost = tr.GetComponent<Ghost>();

                        float dis = Vector3.Distance(ghost.transform.position, MyReferencesPool.MyProtagonist.transform.position);
                        if(dis <= 10f)
                        {
                            ghost.StartCoroutine(ghost.GotoLastPlayerPosition());
                            
                        }

                        
                    }

                    StartCoroutine(StartCooldown());
                }
            }
        }
    }

    IEnumerator StartCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(15f);
        inCooldown = false;
    }
}
