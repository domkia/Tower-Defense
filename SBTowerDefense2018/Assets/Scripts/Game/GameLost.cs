using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameLost : MonoBehaviour
{
    public GameObject gameOverPanel;
    

    void Start()
    {
        gameOverPanel.SetActive(false);
        GameManager.OnGameOver += EndGame;
        gameOverPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    void EndGame()
    {
        GameManager.OnGameOver -= EndGame;
        Debug.Log("GAME OVER!");
        
        //gameOverPanel.SetActive(true);
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => Showgameover());
       // Time.timeScale = 0;

    }
    void Showgameover()
    {
        gameOverPanel.GetComponent<CanvasGroup>().interactable = true;
    }
}
