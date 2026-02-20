using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Cinemachine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] walls = new bool[4]; 
    }

    public Vector2 size;
    public int startPosition = 0;
    public GameObject roomPrefab;
    public Vector2 offset;

    List<Cell> board;

    void Start()
    {
        MazeGenerator();
    }

    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var roomInstance = Instantiate(roomPrefab, new Vector3(i * offset.x, 0, -j * offset.y), 
                    Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                roomInstance.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].walls);

                roomInstance.name += " " + i + "-" + j;
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x * size.y; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPosition;

        Stack<int> cellStack = new Stack<int>();

        int keepingTrack = 0;

        while (keepingTrack < 1000)
        {
            keepingTrack++;
            board[currentCell].visited = true;

            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (cellStack.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = cellStack.Pop();
                }
            }
            else
            {
                cellStack.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].walls[2] = true;
                        currentCell = newCell;
                        board[currentCell].walls[3] = true;
                    }
                    else
                    {
                        board[currentCell].walls[1] = true;
                        currentCell = newCell;
                        board[currentCell].walls[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].walls[3] = true;
                        currentCell = newCell;
                        board[currentCell].walls[2] = true;
                    }
                    else
                    {
                        board[currentCell].walls[0] = true;
                        currentCell = newCell;
                        board[currentCell].walls[0] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        // Check top neighbour
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        // Check down neighbour
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }
        // Check right neighbour
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }
        // Check left neighbour
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }
}
