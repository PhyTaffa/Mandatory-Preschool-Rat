using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private GameObject stateManager;
    // Start is called before the first frame update
    void Start()
    {
        stateManager = FindObjectOfType<GameStateManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            stateManager.GetComponent<GameStateManager>().paused = true;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        stateManager.GetComponent<GameStateManager>().paused = false;
    }

    public void MainMenu()
    {
        stateManager.GetComponent<GameStateManager>().paused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
