using UnityEngine;

/// <summary>
/// Plays sound effects when interacting with the UI.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class UISoundPlayer : Singleton<UISoundPlayer>
{

    public AudioClip ClickButtonSFX;
    public AudioClip AlertSFX;
    public AudioClip TowerBuiltSFX;

    [Range(0.0f, 1.0f)]
    public float SFXVolume;

    private AudioSource source;

    private UISoundPlayer() { }

    /// <summary>
    /// Plays a click sound effect (for instance, when pressing a button)
    /// </summary>
    public void PlayClickSound()
    {
        source.PlayOneShot(ClickButtonSFX, SFXVolume);
    }

    /// <summary>
    /// Plays an alert sound effect when we need to alert the player (e.g. not enough
    /// resources to build tower)
    /// </summary>
    public void PlayAlertSound()
    {
        source.PlayOneShot(AlertSFX, SFXVolume);
    }
    
    /// <summary>
    /// Play a sound when a tower has been succesfully built.
    /// </summary>
    public void PlayTowerBuiltSound()
    {
        source.PlayOneShot(TowerBuiltSFX, SFXVolume);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

}
