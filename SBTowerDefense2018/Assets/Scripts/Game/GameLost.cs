using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class GameLost : MonoBehaviour
{
    public GameObject gameOverPanel;
    

    public AudioClip GameOverSound;
    [Range(0.0f, 1.0f)]
    public float SoundVolume;

    private AudioSource source;

    void Start()
    {
        gameOverPanel.SetActive(false);
        source = GetComponent<AudioSource>();
        GameManager.OnGameOver += EndGame;
        gameOverPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    void EndGame()
    {
        GameManager.OnGameOver -= EndGame;
        gameOverPanel.SetActive(true);
        source.PlayOneShot(GameOverSound, SoundVolume);
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => Showgameover());
    }
    
    void Showgameover()
    {
        gameOverPanel.GetComponent<CanvasGroup>().interactable = true;
    }
}
