using UnityEngine;

public class GameLost : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        GameManager.OnGameOver += EndGame;
    }

    void EndGame()
    {
        GameManager.OnGameOver -= EndGame;
        Debug.Log("GAME OVER!");
        gameOverPanel.SetActive(true);
    }
}
