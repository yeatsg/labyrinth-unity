using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFPSController : MonoBehaviour
{
    public float moveSpeed = 5f;        

    // Update is called once per frame
    void Update()
    {
        // Detect player input
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveForward();
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
