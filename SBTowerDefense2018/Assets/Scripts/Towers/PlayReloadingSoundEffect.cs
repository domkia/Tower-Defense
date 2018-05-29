using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayReloadingSoundEffect : MonoBehaviour
{
    public AudioClip SoundEffect;

    [Range(0.0f, 1.0f)]
    public float SoundVolume;

    private AudioSource source;

    public void PlayReloadingSound()
    {
        source.PlayOneShot(SoundEffect, SoundVolume);
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
}