using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameWon : MonoBehaviour
{
    public Text text;

    public AudioClip VictorySound;
    [Range(0.0f, 1.0f)]
    public float SoundVolume;

    private AudioSource source;

    private void Start()
    {
        text.gameObject.SetActive(false);
        source = GetComponent<AudioSource>();
        GameManager.OnGameWon += Won;
    }

    void Won()
    {
        text.gameObject.SetActive(true);
        source.PlayOneShot(VictorySound, SoundVolume);
        GameManager.OnGameWon -= Won;
        Time.timeScale = 0;
    }
}
