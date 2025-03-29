using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHelper : MonoBehaviour
{
     [SerializeField] private GameObject startTile;
     [SerializeField] private GameObject finishTile;
     [SerializeField] private GameObject helper;
     
     //path to return
     List<GameObject> path = new List<GameObject>();
     //all tiles
     Dictionary<Vector3, GameObject> tilesDictionary = new Dictionary<Vector3, GameObject>();
     
     private int currentTargetIndex = 0;
     private float moveSpeed = 20f;

     void Start()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        int i = 0;
        
        //save all the tiles in a dictionary
        foreach (GameObject tile in tiles)
        {
            //Vector3 roundedPos = RoundPosition(tile.transform.position, 1.0f);
            tilesDictionary.Add(tile.transform.position, tile);
        }
        
        Debug.Log(tilesDictionary.Count);
        
        BFS();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        if (currentTargetIndex >= path.Count) return; 

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;
        
        helper.transform.position = Vector3.MoveTowards(helper.transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(helper.transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++; 
        }
    }

    private void BFS()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(startTile);
        cameFrom[startTile] = null;

        int i = 0;

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            current.GetComponent<SpriteRenderer>().color = Color.black;

            Debug.Log("CURRENT = " + current.name);

            if (GameObject.ReferenceEquals(current, finishTile))
            {
                Debug.Log("BFS Path Found!");
                
                //to do
                ReconstructPath(cameFrom, finishTile);
                
                return;
            }

            //shpuld print neightboursdfbasdjfvOASBFADSKJGKFAJD
            List<GameObject> neighbors = GetNeighbors(current);

            foreach (GameObject neighbor in neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;

                    // Instantiate the label at the neighbor�s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    // numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    // numLabel.GetComponent<TextMesh>().text = i.ToString();

                    // Ensure the label always faces the camera

                    i++; // Increment visit order counter
                }
            }
        }

        Debug.Log("No BFS path found!");
    }
    
    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = floorTile.transform.position;

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

            if (tilesDictionary.TryGetValue(neighborPos, out var neighbor))
            {
                //Debug.Log($"Found neighbor {neighborPos}");
                neighbors.Add(neighbor);    
            }
        }

        return neighbors;
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
            tile.GetComponent<SpriteRenderer>().color = Color.green; 
        }
    }

}
