using UnityEngine;

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
    }

    void EndGame()
    {
        GameManager.OnGameOver -= EndGame;
        gameOverPanel.SetActive(true);
        source.PlayOneShot(GameOverSound, SoundVolume);
        Time.timeScale = 0;
    }
}
