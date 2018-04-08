using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject ingameButtons;

    bool paused = false;
    void Start()
    {
        pausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            ingameButtons.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            ingameButtons.SetActive(true);
        }
    }
}
