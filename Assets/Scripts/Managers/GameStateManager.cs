using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool paused { get; set; } = false;
    [SerializeField] GameObject spawnPatients;

    void Start()
    {
        //spawnPatients.SetActive(true);
    }
}
