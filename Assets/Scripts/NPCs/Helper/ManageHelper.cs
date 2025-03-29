using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetTiles; // List of waypoints (A, B, C, ...)
    private GameObject helper = null;

    private Dictionary<Vector3, GameObject> tilesDictionary = new Dictionary<Vector3, GameObject>();
    private List<GameObject> path = new List<GameObject>();

    private int currentTargetIndex = 0;
    private float moveSpeed = 5f;

    void Start()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        // Save all tiles in dictionary
        foreach (GameObject tile in tiles)
        {
            tilesDictionary[tile.transform.position] = tile;
        }

        helper = this.gameObject;
        
        if (targetTiles.Count >= 2)
        {
            BFS(targetTiles[currentTargetIndex], targetTiles[(currentTargetIndex + 1) % targetTiles.Count]);
        }
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (path.Count == 0) return;

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;
        helper.transform.position = Vector3.MoveTowards(helper.transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(helper.transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++;

            if (currentTargetIndex >= path.Count)
            {
                // Move to the next tile in the sequence
                CycleToNextTarget();
            }
        }
    }

    void CycleToNextTarget()
    {
        // Advance to the next pair in the cycle
        int startIndex = (currentTargetIndex) % targetTiles.Count;
        int finishIndex = (startIndex + 1) % targetTiles.Count;

        GameObject newStart = targetTiles[startIndex];
        GameObject newFinish = targetTiles[finishIndex];

        BFS(newStart, newFinish);
    }

    private void BFS(GameObject startTile, GameObject finishTile)
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(startTile);
        cameFrom[startTile] = null;

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            //current.GetComponent<SpriteRenderer>().color = Color.black;

            if (GameObject.ReferenceEquals(current, finishTile))
            {
                ReconstructPath(cameFrom, finishTile);
                return;
            }

            List<GameObject> neighbors = GetNeighbors(current);
            foreach (GameObject neighbor in neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        Debug.Log("No BFS path found!");
    }

    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = floorTile.transform.position;

        Vector3[] directions = new Vector3[]
        {
            new Vector3(1, 0, 0),  
            new Vector3(-1, 0, 0), 
            new Vector3(0, 1, 0),  
            new Vector3(0, -1, 0),  
        };

        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = pos + dir;
            if (tilesDictionary.TryGetValue(neighborPos, out var neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    void ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        path.Clear();
        while (current != null)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        currentTargetIndex = 0; // Reset movement index

        foreach (GameObject tile in path)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}