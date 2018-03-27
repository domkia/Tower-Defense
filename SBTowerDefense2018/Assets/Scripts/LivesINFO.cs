using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LivesINFO : MonoBehaviour {
    public static int Lives;
    public int startLives = 12;
    private bool gameEnded = false;

    public GameObject gameOverPanel;
    public Text gameOverText;
    
    void Start()
    {
       // Debug.Log(Lives);
        Lives = startLives;
        // Debug.Log(Lives);
        gameOverPanel.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
       

        if (gameEnded)
            return;
        if (LivesINFO.Lives <= 0)
        {
            
            EndGame();
            StartTile.spawning = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void EndGame()
    {
        gameEnded = true;
        Debug.Log("GAME OVER!");
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
}
