using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = System.Random;

public class ManageHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetTiles; // List of waypoints (1, 2, 3, ...)
    [SerializeField] private GameObject helper;

    private Dictionary<Vector3, GameObject> tilesDictionary = new Dictionary<Vector3, GameObject>();
    private List<GameObject> path = new List<GameObject>();

    private int currentTargetIndex = 0;  // Tracks movement along the path
    private int currentCycleIndex = 0;   // Tracks which tile in targetTiles is the start
    private float moveSpeed = 5f;
    private HelperStates helperState = HelperStates.None;

    private GameObject newStart = null;
    private GameObject newFinish = null;
    
    
    
    private HelperInventory helpInventory;
    [SerializeField] private  List<GameObject> medicationLocation = new List<GameObject>();
    [SerializeField] private GameObject bedGO = null;
    
    [SerializeField] private GameObject entranceGO = null;
    
    
    public enum HelperStates
    {
        None,
        Wondering,
        Helping,
        FetchingResources,
    }
    
    void Start()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        // Save all tiles in dictionary
        foreach (GameObject tile in tiles)
        {
            
            tilesDictionary[tile.transform.position] = tile;
        }

        helper = this.gameObject;

        //initialize the stete
        helperState = HelperStates.Wondering;
        
        //Hardcodes the first tile to be the one underneath the player's feet
        // //targetTiles[0] = tilesDictionary[this.transform.position];
        
        
        helpInventory = this.gameObject.GetComponent<HelperInventory>();

        if (targetTiles.Count >= 2)
        {
            StartMoving();
        }
    }

    void Update()
    {
        // //checks the states:
        // switch (helperState)
        // {
        //     case HelperStates.Wondering:
        //         //Debug.Log($"Current State: {helperState}");
        //         
        //         WonderTowardsPoints();
        //         break;
        //     case HelperStates.Helping:
        //         //Debug.Log($"Current State: {helperState}");
        //         
        //         break;
        //     case HelperStates.FetchingResources:
        //         Debug.Log("medicaton location list: " + medicationLocation[0]);
        //         MoveTowardsMedication(medicationLocation[0]);
        //         
        //         //Debug.Log($"Current State: {helperState}");
        //         break;
        //     case HelperStates.None:
        //         Debug.Log("No States");
        //         break;
        // }
        

        //debug shit
        if (Input.GetKeyDown(KeyCode.U))
        {
            helperState = HelperStates.None;
            Debug.Log($"Current State: {helperState}");
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            helperState = HelperStates.Wondering;
            Debug.Log($"Current State: {helperState}");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            helperState = HelperStates.Helping;
            Debug.Log($"Current State: {helperState}");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            helperState = HelperStates.FetchingResources;
            Debug.Log($"Current State: {helperState}");
        }

        
        if (path.Count > 0)
        {
            MoveAlongPath();
        }
        else
        {
            CheckForPatientOrFetchMedicine();
        }
    }
 
    void MoveAlongPath()
    {
        if (currentTargetIndex >= path.Count) return;

        Vector3 targetPos = path[currentTargetIndex].transform.position;
        helper.transform.position = Vector3.MoveTowards(helper.transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(helper.transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++;

            if (currentTargetIndex >= path.Count)
            {
                if (helperState == HelperStates.Wondering)
                {
                    CycleToNextTarget();
                }
                else
                {
                    CheckForPatientOrFetchMedicine();
                }
            }
        }
    }

    void CheckForPatientOrFetchMedicine()
    {
        bool hasPatient = bedGO.GetComponent<GenericBed>().hasPatient;

        if (hasPatient)  // If there's a patient, go to the bed
        {
            Debug.Log("Patient found, moving to heal.");
            MoveToBed();  // Move helper to heal the patient
        }
        else
        {
            int medicationAmount = helpInventory.GetMedicationAmount();  // Check the amount of medication
            Debug.Log($"Current Medication Amount: {medicationAmount}");

            if (medicationAmount < 1)  // If there's no patient and we need more medicine, fetch medicine
            {
                Debug.Log("No patient, but medicine is needed. Moving to fetch medicine.");
                GoFetchMedicine();
            }
            else
            {
                Debug.Log("No patient, and medicine is sufficient. Waiting or wandering.");
                MoveToBed();
                // Handle wandering or other behavior when medicine is sufficient
            }
        }
    }




    void MoveToBed()
    {
        helpInventory.AddMedicine(-1);
        aStar(GetCurrentTile(), bedGO);  // Move helper to the bed
        bedGO.GetComponent<GenericBed>().hasPatient = false;
    }

    IEnumerator HealPatientCoroutine()
    {
        Debug.Log("Starting healing process...");
        float healTime = 5f; // Time in seconds to heal the patient
        yield return new WaitForSeconds(healTime);

        Debug.Log("Patient healed!");
        bedGO.GetComponent<GenericBed>().hasPatient = false; // Mark patient as healed
        GoFetchMedicine(); // After healing, fetch medicine
    }
    void GoFetchMedicine()
    {
        if (medicationLocation.Count > 0)
        {
            helpInventory.AddMedicine(1);
            aStar(GetCurrentTile(), medicationLocation[0]);
        }
    }


    private void MoveTowardsMedication(GameObject neededMedicationLocation)
    {
        aStar(newStart, neededMedicationLocation);
    }

    void WonderTowardsPoints()
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
        currentCycleIndex = (currentCycleIndex + 1) % targetTiles.Count;

        int startIndex = currentCycleIndex;
        int finishIndex = (currentCycleIndex + 1) % targetTiles.Count;

        newStart = targetTiles[startIndex];
        newFinish = targetTiles[finishIndex];

        aStar(newStart, newFinish);  
    }

    GameObject GetCurrentTile()
    {
        float minDistance = Mathf.Infinity;
        GameObject closestTile = null;

        foreach (var tile in tilesDictionary.Values)
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

    void StartMoving()
    {
        currentCycleIndex = 0; // Start at first tile in the list
        aStar(targetTiles[0], targetTiles[1]);
    }
}
