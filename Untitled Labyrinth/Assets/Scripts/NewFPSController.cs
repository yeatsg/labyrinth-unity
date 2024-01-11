using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFPSController : MonoBehaviour
{
    public waypoints GameObject;
    public float moveSpeed = 5f;
    public float distanceThreshold = 0.1f;



    private Transform currentWaypoint;




    void Start()
    {
        //Set initial position to the first waypoint
        currentWaypoint = waypoints.position;
        transform.LookAt(currentWaypoint);
    }

 

    // Update is called once per frame
    void Update()
    {
        // Detect player input
        if (Input.GetKeyDown(KeyCode.W))
        {
            //MoveForward();
            Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        }
    }

    void MoveForward()
    {
        // Calculate the next position based on the direction the player is facing
        Vector3 nextPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

        // Check if the next cell is part of the path
       // if (IsCellOnPath(nextPosition))
        {
            // Move the player forward
            transform.position = nextPosition;
        }
        // else: Do nothing or provide feedback to the player (e.g., hit a wall)
    }
}
