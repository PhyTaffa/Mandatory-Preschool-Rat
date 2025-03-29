using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool paused { get; set; } = false;
    [SerializeField] GameObject spawnPatients;
    // Start is called before the first frame update
    void Start()
    {
        //spawnPatients.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
