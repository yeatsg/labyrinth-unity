using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    //this is the physical size of our maze cells. Getting this wrong will result in overlapping or visible gaps between each cell.
    public float CellSize = 1f;

    private void Start()
    {
        //Get Maze Generator script to make us a maze.
        MazeCell[,] maze = mazeGenerator.GetMaze();
        //Loop through every cell in maze.
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                //Instantiate a new maze cell prefab as a child of the MazeRenderer object.
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);
                    // Get a new refernce to the cell's MazeCellPrefab Script
                    MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                //Determine which walls need to be active.
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                //bottom and right walls deactivated by default, only visible when at edge of map.
                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
}
