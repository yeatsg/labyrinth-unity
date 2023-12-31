using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{


    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 1f);
        }

        Gizmos.color = Color.red;
        //this for loop will go through the list of children of the waypoint in a simple numeric order.
        // Will need to discover a more intelligent way of doing this.
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    //This is designed to cycle through each Waypoint, and then loop back to child 0 at end

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex()  + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
    }
}
