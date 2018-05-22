using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayProjectileSounds : MonoBehaviour
{
    // Sounds that are played when a projectile is spawned.
    public AudioClip[] SoundsOnSpawn;
    // Sound that is played when a projectile is destroyed.
    public AudioClip[] SoundsOnDestroy;

    private AudioSource source;

    // Volume bounds
    private float volLoBound = 0.75f;
    private float volHiBound = 1.0f;

    // Pitch bounds
    private float pitchLoBound = 0.9f;
    private float pitchHiBound = 1.1f;

    public void PlaySound(SoundType type)
    {
        float vol = Random.Range(volLoBound, volHiBound);
        float pitch = Random.Range(pitchLoBound, pitchHiBound);
        source.pitch = pitch;
        int sndIndex;

        switch(type)
        {
            case SoundType.ProjectileSpawn:
                sndIndex = Random.Range(0, SoundsOnSpawn.Length);
                source.PlayOneShot(SoundsOnSpawn[sndIndex], vol);
                break;
            case SoundType.ProjectileDestroy:
                sndIndex = Random.Range(0, SoundsOnDestroy.Length);
                source.PlayOneShot(SoundsOnDestroy[sndIndex], vol);
                DestroySoundPlayer(SoundsOnDestroy[sndIndex].length);
                break;
            default:
                Debug.LogError("Type of sound effect not applicable for projectile! Pausing game ...");
                Debug.Break();
                break;
        }
    }

    private void DestroySoundPlayer(float soundLength)
    {
        transform.parent = null;
        Destroy(gameObject, soundLength);
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
}
