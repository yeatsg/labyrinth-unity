using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 5, mazeHeight = 5; //default dimensions of Maze.
    public int startX, startY; // The position the algorithm starts from.
    MazeCell[,] maze; //An Array of maze cells representing the maze grid.

    Vector2Int currentCell; //The maze cell we are currently looking at.

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        //Call CarvePath function and begin process.
        CarvePath(startX, startY);

        return maze;
    }

    //Creates List of Enum Direction so that List methods can be used in GetRandomDirections
    List<Direction> directions = new List<Direction> {

        Direction.Up, Direction.Down, Direction.Left, Direction.Right
    };

    //Function takes List of Directions (set at 4 currently) and spits them back in a random order.
    List<Direction> GetRandomDirections()
    {
        //Makes a copy of our directions list that can me read and modified.
        List<Direction> dir = new List<Direction>(directions);

        //Makes a directions list to put our randomised directions into.
        List<Direction> rndDir = new List<Direction>();

        while (dir.Count > 0)
        {
            int rnd = Random.Range(0, dir.Count); //Get random index in list.
            rndDir.Add(dir[rnd]); // Add the random direction to our list.
            dir.RemoveAt(rnd);  // Remove that direction so we can't choose it again.
        }

        return rndDir;
    }

    //Checks if the cell is out of the map or has already been visited ==> marked as valid.
    bool IsCellValid (int x, int y)
    {

        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false;
        else return true;

    }

    //Checks if neighbor is valid, and either proceeds to first neighbor, or returns currentCell value for dead end.
    Vector2Int CheckNeighbors ()
    {
        List<Direction> rndDir = GetRandomDirections();
        // For loop through the random direction array annd check neighbor each direction 
        for (int i = 0; i < rndDir.Count; i++)
        {
            //Set neighbor coordinates to current cell for now.
            Vector2Int neighbor = currentCell;
            // Switch case for each possible direction
            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbor.y++;
                    break;
                case Direction.Down:
                    neighbor.y--;
                    break;
                case Direction.Left:
                    neighbor.x--;
                    break; 
                case Direction.Right:
                    neighbor.x++;
                    break;
            }

            // If the neighbor being checked is valid, we can return that neighbor. If not, we go again.
            if(IsCellValid(neighbor.x, neighbor.y)) return neighbor; 

        }

        //If we tried all directions and didn't find a valid neighbor, we return currentCell values.

        return currentCell;
    }

    //Takes in two maze cell coordinates and sets cells accordingly.
    void BreakWalls (Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x) //Greater x-value means Primary cell is to the right of secondary cell.
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        } else if (primaryCell.x < secondaryCell.x) // Greater x-value means Secondary cell is to the right of primary cell.
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        } else if (primaryCell.y < secondaryCell.y) // Greater y-value for Secondary cell means Primary Cell is below Secondary
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        } else if (primaryCell.y > secondaryCell.y) // Greater y-value for Primary means Secondary cell is below Primary 
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    //Starting at x and y passed in, carves a path through the maze until it encounters a dead end
    void CarvePath (int x, int y)
    {
        //Check to make sure start position is within map boundaries. Default to 0 if out of bounds.
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds.");
        }

        //Set current cell to starting positions passed in.
        currentCell = new Vector2Int(x, y);

        //A list to keep track of our current path
        List<Vector2Int> path = new List<Vector2Int>();

        //Loop until we encounter a dead end
        bool deadEnd = false;
        while (!deadEnd) {
            //Call Check Neighbors function to get value of next cell
            Vector2Int nextCell = CheckNeighbors();
            //If at dead end, Check Neighbors will return value of currentCell and trigger conditional below.
            if (nextCell == currentCell)
            {
                //Loop back along the path in reverse and look for valid neighbors that were missed 
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbors();

                    if (nextCell != currentCell) break;
                }

                // If that cell has no valid neighbors, set dead to true so we break out of loop
                if (nextCell == currentCell)
                {
                    deadEnd = true;
                }

            } else
            {
                //If valid neighbor, run Break Walls function and continue through loop.
                BreakWalls(currentCell, nextCell); //Set wall flags on these two cells
                maze[currentCell.x, currentCell.y].visited = true; //Set cell to visited before moving on.
                currentCell = nextCell; //Set the current cell to the valid neighbor we found.
                path.Add(currentCell); //Add this cell to our path list.

                if (path.Count == 1)
                    maze[currentCell.x, currentCell.y].startingCell = true; 
            }

        }
    }

};

// Enum for List for GetRandomDirections function
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    public bool visited;
    public bool startingCell;
    public bool endingCell;
    public bool ally;

    public int x, y;

    public bool topWall;
    public bool leftWall;

    // Retur x and y as a Vector2Int for convenience sake.

    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell (int x, int y)
    {

        //Represents coordinates of individual cell in the Maze Grid.
        this.x = x;
        this.y = y;

        //Whether the algorithm has visited this cell or not.
        visited = false;

        // All walls are present until the algorithm removes them.
        topWall = leftWall = true;

    }
};