using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHelper : MonoBehaviour
{
    [SerializeField] public List<GameObject> targetTiles; // List of A & B tiles
    
    private GameObject helper;
    private Dictionary<Vector3, GameObject> tilesDictionary = new Dictionary<Vector3, GameObject>();
    private List<GameObject> path = new List<GameObject>();

    private bool isMoving = false;  // Prevents input spam
    private float moveSpeed = 5f;

    private GameObject newStart;
    private GameObject newFinish;
    private float squareLength;
    private BedBinder bedBinder;
    private BedInteraction bedInteraction;

    //sprites and giggles
    private Sprite nathanSprite = null;
    private GameObject nathanGO = null;

    void Start()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] tilesBed = GameObject.FindGameObjectsWithTag("Tile Bed");
        GameObject tileMed = GameObject.FindGameObjectWithTag("Tile Medication");

        foreach (GameObject tile in tiles)
        {
            tilesDictionary[tile.transform.position] = tile;
        }
        foreach (GameObject tile in tilesBed)
        {
            tilesDictionary[tile.transform.position] = tile;
        }
        tilesDictionary[tileMed.transform.position] = tileMed;


        helper = this.gameObject;
        squareLength = targetTiles[0].GetComponent<SpriteRenderer>().bounds.size.x; // Assume uniform tile size

        bedBinder = targetTiles[0].GetComponent<BedBinder>();
        bedInteraction = bedBinder.bedRefGO.GetComponent<BedInteraction>();



        //nathanSprite = Resources.Load<Sprite>("dontOpen.jpg");
        nathanGO = GameObject.FindGameObjectWithTag("NATHAN");
        nathanSprite = nathanGO.GetComponent<SpriteManager>().spriteNNN;
    }

    void Update()
    {
        if (bedInteraction.isPatientOnBed && !isMoving && !bedInteraction.isHealing)
        {
            StartCoroutine(CycleAB());
        }
        //else if (bedInteraction.isPatientOnBed && isMoving && bedInteraction.isHealing)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(CycleBA());
        //}

        if (Input.GetKeyDown(KeyCode.N))
        {
            // sprite and scale
            Debug.Log(nathanSprite);
            this.transform.localScale = new Vector3(0.14f, 0.14f, 0.14f);
            GetComponent<SpriteRenderer>().sprite = nathanSprite;
        }
    }

    IEnumerator CycleAB()
    {
        if (targetTiles.Count < 2) yield break;

        isMoving = true;

        // Move A → B
        newStart = targetTiles[0];
        newFinish = targetTiles[1];
        aStar(newStart, newFinish);
        yield return MoveAlongPath();

        // Move B → A
        aStar(newFinish, newStart);
        yield return MoveAlongPath();

        //start medicating patience -> acll functoin
        //IUF ANYBODY SEES THIS AND THIS SHIT IS STUILL EMPTY FILL ITY WIOTH THER PWEFDJASHDOLIGFVASOIKLDFGYHIAOSEJDCOLSI PROPER INSTRUCTION TO HEAL THE PATIENCE BINDFED  TO THE FIRST ELEMENT OF THE TARGET TILE
        Debug.Log("Patient Healing by Worker");
        bedInteraction.Interact(gameObject);
        
        isMoving = false; // Allows another press after full cycle
    }

    IEnumerator CycleBA()
    {
        if (targetTiles.Count < 2) yield break;

        isMoving = false;
        GameObject newObj = new GameObject();
        newObj.transform.position = transform.position;

        // Move B → A
        aStar(newObj, bedBinder.gameObject);
        yield return MoveAlongPath();

        Destroy(newObj);
    }

    IEnumerator MoveAlongPath()
    {
        foreach (GameObject tile in path)
        {
            Vector3 targetPos = tile.transform.position;

            while (Vector3.Distance(helper.transform.position, targetPos) > 0.1f)
            {
                helper.transform.position = Vector3.MoveTowards(helper.transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
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

            if (current == finishTile)
            {
                ReconstructPath(cameFrom, finishTile);
                return;
            }

            foreach (GameObject neighbor in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + GetCostPlusDistance(neighbor, finishTile);

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    priorityQueue.EnqueueOrUpdate(neighbor, newCost);
                }
            }
        }
    }

    private float GetCostPlusDistance(GameObject tile, GameObject finishTile)
    {
        return 10 + Vector3.Distance(tile.transform.position, finishTile.transform.position);
    }

    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = floorTile.transform.position;

        Vector3[] directions = new Vector3[]
        {
            new Vector3(squareLength, 0, 0),
            new Vector3(-squareLength, 0, 0),
            new Vector3(0, squareLength, 0),
            new Vector3(0, -squareLength, 0)
        };

        foreach (Vector3 dir in directions)
        {
            if (tilesDictionary.TryGetValue(pos + dir, out var neighbor))
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


        //Debug

        //for (int i = 0; i < path.Count; i++)
        //{
        //    path[i].GetComponent<SpriteRenderer>().color = Color.green;
        //}
        
    }

    // New Method to assign a task where the start is the player's position, and finish is from targetTiles
    public void AssignNewTaskFromPlayer(GameObject newFinishTile)
    {
        // Set the player's position as the new start point
        newStart = GetClosestTileToHelper();

        // Assign the new finish point from the provided argument
        newFinish = newFinishTile;

        // Start A* with the new start and finish
        aStar(newStart, newFinish);
        StartCoroutine(MoveAlongPath());
    }

    // Method to get the closest tile to the player's current position
    private GameObject GetClosestTileToHelper()
    {
        GameObject closestTile = null;
        float minDistance = Mathf.Infinity;

        // Iterate through each target tile and find the closest one to the helper's current position
        foreach (GameObject tile in targetTiles)
        {
            float distance = Vector3.Distance(helper.transform.position, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTile = tile;
            }
        }

        return closestTile;
    }
}