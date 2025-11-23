using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // =======================
    // SERIALIZED FIELDS
    // =======================
    [Header("SFX")]
    public AudioSource audioSource;                // For sound effects
    public AudioClip polarityImpactSound;          // When player attaches to platform
    public AudioClip collectibleSound;             // Checkpoint collectible
    public AudioClip triggerCollectibleSound;      // Trigger-type collectible
    public AudioClip deathSound;                   // Player death sound
    public AudioClip levelCompleteSound;           // Level complete / win sound

    [Header("Background Music")]
    public AudioSource bgMusicSource;              // For background music
    public AudioClip bgMusicClip;

    // =======================
    // INITIALIZATION
    // =======================
    private void Start()
    {
        // Setup background music
        if (bgMusicSource != null && bgMusicClip != null)
        {
            bgMusicSource.clip = bgMusicClip;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }

        // Ensure SFX audio source does not loop
        if (audioSource != null)
        {
            audioSource.loop = false;
        }
    }

    // =======================
    // SFX PLAYBACK METHODS
    // =======================

    /// <summary>Play polarity impact sound when player attaches to platform</summary>
    public void PlayPolarityImpact()
    {
        if (audioSource != null && polarityImpactSound != null)
            audioSource.PlayOneShot(polarityImpactSound);
    }

    /// <summary>Play checkpoint collectible sound</summary>
    public void PlayCollectibleSound()
    {
        if (audioSource != null && collectibleSound != null)
            audioSource.PlayOneShot(collectibleSound);
    }

    /// <summary>Play trigger collectible sound</summary>
    public void PlayTriggerSound()
    {
        if (audioSource != null && triggerCollectibleSound != null)
            audioSource.PlayOneShot(triggerCollectibleSound);
    }

    /// <summary>Play player death sound</summary>
    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    /// <summary>Play level complete / win sound</summary>
    public void PlayLevelCompleteSound()
    {
        if (audioSource != null && levelCompleteSound != null)
            audioSource.PlayOneShot(levelCompleteSound);
    }
}
