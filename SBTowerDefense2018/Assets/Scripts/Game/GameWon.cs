using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameWon : MonoBehaviour
{
    public CanvasGroup panel;

    public AudioClip VictorySound;
    [Range(0.0f, 1.0f)]
    public float SoundVolume;

    private AudioSource source;

    private void Start()
    {
        panel.gameObject.SetActive(false);
        source = GetComponent<AudioSource>();
        GameManager.OnGameWon += Won;
        panel.alpha = 0f;
    }

    void Won()
    {
        GameManager.OnGameWon -= Won;
        panel.gameObject.SetActive(true);
        source.PlayOneShot(VictorySound, SoundVolume);
        panel.DOFade(1f, 1f).OnComplete(() => panel.interactable = true);
    }
}
