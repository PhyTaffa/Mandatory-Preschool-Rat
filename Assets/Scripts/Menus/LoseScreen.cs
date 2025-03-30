using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    private GameObject stateManager;
    // Start is called before the first frame update
    void Start()
    {
        stateManager = FindObjectOfType<GameStateManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        stateManager.GetComponent<GameStateManager>().paused = true;
    }

    public void MainMenu()
    {
        stateManager.GetComponent<GameStateManager>().paused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
