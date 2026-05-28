using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited = false;
        public bool[] walls = new bool[4] {true, true, true, true};
    }

    public DoorLift doorLift;

    public Vector2Int size;
    public int startPosition = 0;
    public GameObject roomPrefab;
    public GameObject bossRoomPrefab;
    public Vector2 offset;

    public Vector3 spawnOrigin;

    private bool hasStarted = false;
    List<Cell> board;

    void Start()
    {
        MazeGenerator();
        hasStarted = true;
        if (hasStarted && doorLift != null)
            StartCoroutine(doorLift.DescendDoor());
        else
        {
            Debug.LogWarning("DoorLift reference is missing. Please assign it in the inspector.");
        }

        Debug.Log("DoorLift reference: " + (doorLift != null ? doorLift.name : "null"));
    }

    int FindFurthestCellFromStart()
    {
        Queue<int> queue = new Queue<int>();
        int[] dist = new int[board.Count];
        bool[] visited = new bool[board.Count];

        queue.Enqueue(startPosition);
        visited[startPosition] = true;

        int furthest = startPosition;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            foreach (int next in GetConnectedNeighbors(current))
            {
                if (!visited[next])
                {
                    visited[next] = true;
                    dist[next] = dist[current] + 1;
                    queue.Enqueue(next);

                    if (dist[next] > dist[furthest])
                        furthest = next;
                }
            }
        }

        return furthest;
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                if (currentCell.visited)
                {
                    var roomInstance = Instantiate(roomPrefab, new Vector3(spawnOrigin.x + i * offset.x, spawnOrigin.y, spawnOrigin.z - j * offset.y),
                    Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    roomInstance.UpdateRoom(currentCell.walls);

                    roomInstance.name += " " + i + "-" + j;
                }
            }
        }

        int bossIndex = FindFurthestCellFromStart();

        int x = bossIndex % size.x;
        int y = bossIndex / size.x;

        Vector3 bossPosition = new Vector3(
            spawnOrigin.x + x * offset.x,
            spawnOrigin.y,
            spawnOrigin.z - y * offset.y
        );

        Instantiate(bossRoomPrefab, bossPosition, Quaternion.identity, transform);
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x * size.y; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPosition;
        int visitedCells = 1;
        board[currentCell].visited = true;

        Stack<int> cellStack = new Stack<int>();

        while (visitedCells < board.Count)
        {
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (cellStack.Count > 0)
                {
                    currentCell = cellStack.Pop();
                    continue;
                }
                else
                {
                    break;
                }
            }

            cellStack.Push(currentCell);

            Shuffle(neighbours);
            int newCell = neighbours[0];

            if (!board[newCell].visited)
            {
                board[newCell].visited = true;
                visitedCells++;
            }

            RemoveWall(currentCell, newCell);

            currentCell = newCell;
        }

        // 0 = top, 1 = right, 2 = bottom, 3 = left

        board[startPosition].visited = true;

        bool fullyClosed = board[startPosition].walls.All(wall => wall);

        if (fullyClosed)
        {
            int below = startPosition + size.x;

            if (below < board.Count)
            {
                RemoveWall(startPosition, below);
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        int x = cell % size.x;
        int y = cell / size.x;

        // top
        if (y > 0)
        {
            int top = cell - size.x;
            if (!board[top].visited)
                neighbours.Add(top);
        }

        // bottom
        if (y < size.y - 1)
        {
            int bottom = cell + size.x;
            if (!board[bottom].visited)
                neighbours.Add(bottom);
        }

        // right
        if (x < size.x - 1)
        {
            int right = cell + 1;
            if (!board[right].visited)
                neighbours.Add(right);
        }

        // left
        if (x > 0)
        {
            int left = cell - 1;
            if (!board[left].visited)
                neighbours.Add(left);
        }

        return neighbours;
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    List<int> GetConnectedNeighbors(int cell)
    {
        List<int> result = new List<int>();

        int x = cell % size.x;
        int y = cell / size.x;

        // top
        if (!board[cell].walls[0] && cell - size.x >= 0)
            result.Add(cell - size.x);

        // right
        if (!board[cell].walls[1] && (cell + 1) % size.x != 0)
            result.Add(cell + 1);

        // bottom
        if (!board[cell].walls[2] && cell + size.x < board.Count)
            result.Add(cell + size.x);

        // left
        if (!board[cell].walls[3] && cell % size.x != 0)
            result.Add(cell - 1);

        return result;
    }

    void RemoveWall(int currentCell, int newCell)
    {
        int dx = newCell % size.x - currentCell % size.x;
        int dy = newCell / size.x - currentCell / size.x;

        // moving right
        if (dx == 1)
        {
            board[currentCell].walls[1] = false;
            board[newCell].walls[3] = false;     
        }
        // moving left
        else if (dx == -1)
        {
            board[currentCell].walls[3] = false;
            board[newCell].walls[1] = false;
        }
        // moving up
        else if (dy == -1)
        {
            board[currentCell].walls[0] = false;
            board[newCell].walls[2] = false;
        }
        // moving down
        else if (dy == 1)
        {
            board[currentCell].walls[2] = false;
            board[newCell].walls[0] = false;
        }
    }
}