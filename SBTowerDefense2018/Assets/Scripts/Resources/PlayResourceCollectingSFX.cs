using UnityEngine;

/// <summary>
/// Plays sound effects when interacting with a resource interactable.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayResourceCollectingSFX : MonoBehaviour
{
    // Sound effects from a particular type are chosen at random so we prevent
    // the sounds from being too repetitive.

    // Sound effects to play when a player is in progress of collecting a resource.
    public AudioClip[] resourceCollectingSFX;
    // Sound effects to play when a player has collected some resource but the resource
    // is not depleted yet.
    public AudioClip[] resourceCollectedSFX;
    // Sound effects to play when a player depletes a resource.
    public AudioClip[] resourceDepletedSFX;

    // Volume of sound effects are slightly randomized.
    private float volLoBound = 0.5f;
    private float volHiBound = 1.0f;

    private AudioSource source;

    /// <summary>
    /// Plays a sound effect once. Returns the length of the sound effect to be played.
    /// </summary>
    /// <param name="type">Type of sound effect to play.</param>
    /// <returns>Returns length of sound effect to be played (in seconds).</returns>
    public float PlaySound(SoundType type)
    {
        float vol = Random.Range(volLoBound, volHiBound);
        AudioClip sound = ChooseSound(type);
        source.PlayOneShot(sound, vol);
        return sound.length;
    }

    /// <summary>
    /// Chooses a sound effect at random, based on the sound type.
    /// </summary>
    /// <param name="type">Type of sound effect to be chosen.</param>
    /// <returns>Returns the sound that was chosen.</returns>
    private AudioClip ChooseSound(SoundType type)
    {
        int sndIndex;
        AudioClip sound;
        switch (type)
        {
            case SoundType.ResourceCollecting:
                sndIndex = Random.Range(0, resourceCollectingSFX.Length);
                sound = resourceCollectingSFX[sndIndex];
                break;
            case SoundType.ResourceCollected:
                sndIndex = Random.Range(0, resourceCollectedSFX.Length);
                sound = resourceCollectedSFX[sndIndex];
                break;
            case SoundType.ResourceDepleted:
                sndIndex = Random.Range(0, resourceDepletedSFX.Length);
                sound = resourceDepletedSFX[sndIndex];
                break;
            default:
                Debug.LogError("Type of sound effect not applicable for the resource interactable! Pausing game...");
                Debug.Break();
                sound = null;
                break;
        }
        return sound;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
}