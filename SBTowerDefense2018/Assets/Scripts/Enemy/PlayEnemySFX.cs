using UnityEngine;

/// <summary>
/// Plays sound effects for enemies when they are hit, hurt, killed.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayEnemySFX : MonoBehaviour
{
    // Sound effects played when an enemy gets hit by a projectile.
    // These sound effects play every single time.
    public AudioClip[] EnemyHitSFX;
    // Sound effects played when an enemy is hurt but not killed.
    // These sound effects don't play every single time. Instead,
    // they play based on random chance.
    public AudioClip[] EnemyPainSFX;
    // Sound effects played when an enemy is killed.
    // These sound effects play every single time.
    public AudioClip[] EnemyDeathSFX;

    // How often a pain sound effect is played.
    [Range(0.0f, 1.0f)]
    public float PainChance;

    private AudioSource source;

    // Volumes are slightly randomized.
    private float volLoBound = 0.5f;
    private float volHiBound = 1.0f;

    public void Play(SoundType type)
    {
        // First, choose a random hit sound effect and play it.
        int hitSFXindex = Random.Range(0, EnemyHitSFX.Length);
        float vol = Random.Range(volLoBound, volHiBound);
        source.PlayOneShot(EnemyHitSFX[hitSFXindex], vol);

        if(type == SoundType.EnemyPain)
        {
            bool soundWillPlay = Random.Range(0.0f, 1.0f) < PainChance;
            if(soundWillPlay)
            {
                int painSFXindex = Random.Range(0, EnemyPainSFX.Length);
                source.PlayOneShot(EnemyPainSFX[painSFXindex], vol);
            }
        } else
        {
            int deathSFXindex = Random.Range(0, EnemyDeathSFX.Length);
            source.PlayOneShot(EnemyDeathSFX[deathSFXindex], vol);
            OnDeath();
        }
    }

    private void OnDeath()
    {
        transform.parent = null;
        Destroy(gameObject, 1.5f);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
}