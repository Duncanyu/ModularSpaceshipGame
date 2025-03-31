using UnityEngine;

public class StandardThruster : EngineBase
{
    public ParticleSystem thrustEffect;
    public AudioSource thrustSound;

    private bool isThrusting;

    void Update()
    {
        if (isThrusting)
        {
            if (thrustEffect != null && !thrustEffect.isPlaying) thrustEffect.Play();
            if (thrustSound != null && !thrustSound.isPlaying) thrustSound.Play();
        }
        else
        {
            if (thrustEffect != null && thrustEffect.isPlaying) thrustEffect.Stop();
            if (thrustSound != null && thrustSound.isPlaying) thrustSound.Stop();
        }

        isThrusting = false;
    }

    public override void ApplyThrust()
    {
        isThrusting = true;
    }
} 
