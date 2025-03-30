using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Search : MonoBehaviour
{
    public GameObject start;
    public GameObject finish;

    //all tiles in a dictionary
    Dictionary<Vector3, GameObject> tilesDictoinary = new Dictionary<Vector3, GameObject>();

    Queue<GameObject> queue = new Queue<GameObject>();
    List<GameObject> path = new List<GameObject>();
    private int currentTargetIndex = 0; 
    private float moveSpeed = 10f;

    

    void Start()
    {

        if (start == null || finish == null)
            Debug.Log("Start and/or Finish are not defined!");
        else
        {
            // change color
        }

        //stores all the tiles in a list
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Vector3 tilePos = tile.transform.position;
            tilesDictoinary[tilePos] = tile;
        }

        //Debug.Log(tilesDictoinary.Count);
        
        //DebugNeighbours();

        //BFS();

    }

    List<GameObject> GetNeighbors(GameObject tile)
    {
        //store all neighoburs to return
        List<GameObject> neighbors = new List<GameObject>();
        
        //cache game object position
        Vector3 pos = tile.transform.position;

        
        // Possible movement directions
        Vector3[] directions = new Vector3[]
        {
            new Vector3(1, 0, 0),  // Right
            new Vector3(-1, 0, 0), // Left
            new Vector3(0, 1, 0),  // Forward
            new Vector3(0, -1, 0), // Backward
        };
        
        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = pos + dir;

            if (tilesDictoinary.TryGetValue(neighborPos, out var neighbor))
            {
                Debug.Log($"Found neighbor: {neighbor}");
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    
    void DebugNeighbours()
    {
        List<GameObject> neighbours = GetNeighbors(start);
        foreach(GameObject n in neighbours)
        {
        }

    }

    void BFS()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(start);
        cameFrom[start] = null;

        int i = 0;

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            Debug.Log("CURRENT = " + current.name);

            if (GameObject.ReferenceEquals(current, finish))
            {
                Debug.Log("BFS Path Found!");
                //ReconstructPath(cameFrom, finish);
                return;
            }

            //should find all the four neighbours of a given tile
            List<GameObject> neighbors = GetNeighbors(current);

            
            foreach (GameObject neighbor in neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;

                    // Instantiate the label at the neighbor�s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    
                    i++; // Increment visit order counter
                }
            }
        }

        Debug.Log("No BFS path found!");
    }

    void ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        while (current != null) 
        {
            path.Add(current);
            current = cameFrom[current]; 
        }

        path.Reverse(); // Reverse to get start -> goal order
        
        Debug.Log("Path Length: " + path.Count);
        foreach (GameObject tile in path)
        {
            tile.GetComponent<Renderer>().material.color = Color.green; 
        }
    }

    void MoveAlongPath()
    {
        if (currentTargetIndex >= path.Count) return; 

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;
        targetPos.y += 1f; 

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++; 
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

}
