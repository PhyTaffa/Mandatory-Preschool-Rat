using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManageHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetTiles; // List of waypoints (1, 2, 3, ...)
    [SerializeField] private GameObject helper;

    private Dictionary<Vector3, GameObject> tilesDictionary = new Dictionary<Vector3, GameObject>();
    private List<GameObject> path = new List<GameObject>();

    private int currentTargetIndex = 0;  // Tracks movement along the path
    private int currentCycleIndex = 0;   // Tracks which tile in targetTiles is the start
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
            StartNewPath();
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
        // Move to the next start tile in the sequence (cycling through the list)
        currentCycleIndex = (currentCycleIndex + 1) % targetTiles.Count;

        int startIndex = currentCycleIndex;
        int finishIndex = (currentCycleIndex + 1) % targetTiles.Count;

        GameObject newStart = targetTiles[startIndex];
        GameObject newFinish = targetTiles[finishIndex];

        aStar(newStart, newFinish);
    }

    private void aStar(GameObject startTile, GameObject finishTile)
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> costSoFar = new Dictionary<GameObject, float>();
        

        PriorityQueue<GameObject> priorityQueue = new PriorityQueue<GameObject>();
        priorityQueue.Enqueue(startTile, 1f);
        cameFrom[startTile] = null;
        costSoFar[startTile] = 0f;

        while (priorityQueue.Count > 0)
        {
            GameObject current = priorityQueue.Dequeue();
            //current.GetComponent<SpriteRenderer>().color = Color.black;

            if (GameObject.ReferenceEquals(current, finishTile))
            {
                ReconstructPath(cameFrom, finishTile);
                return;
            }

            List<GameObject> neighbors = GetNeighbors(current);
            
            foreach (GameObject neighbor in neighbors)
            {
                float newCost = costSoFar[current] + GetCostPlusDistance(neighbor, finishTile);

                if (!cameFrom.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    cameFrom[neighbor] = current;
                    costSoFar[neighbor] = newCost;

                    priorityQueue.EnqueueOrUpdate(neighbor, 1f);
                    cameFrom[neighbor] = current;
                }
            }
        }

        Debug.Log("No BFS path found!");
    }
    private float GetCostPlusDistance(GameObject tile, GameObject finishTile)
    {
        float cost = 0;
        float distance = 0;
        
        if(tile.tag.Contains("Tile"))
        {
            cost = 10;
        }
        
        distance = Mathf.Sqrt(Mathf.Pow(finishTile.transform.position.x - tile.transform.position.x, 2) + Mathf.Pow(finishTile.transform.position.y - tile.transform.position.y, 2));
        
        return cost + distance;
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
            
            // new Vector3(1, 1, 0),  
            // new Vector3(-1, 1, 0), 
            // new Vector3(-1, -1, 0),  
            // new Vector3(1, -1, 0),  
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

    void StartNewPath()
    {
        currentCycleIndex = 0; // Start at first tile in the list
        aStar(targetTiles[0], targetTiles[1]);
    }
}
