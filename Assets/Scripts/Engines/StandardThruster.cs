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
            if (!thrustEffect.isPlaying) thrustEffect.Play();
            if (!thrustSound.isPlaying) thrustSound.Play();
        }
        else
        {
            if (thrustEffect.isPlaying) thrustEffect.Stop();
            if (thrustSound.isPlaying) thrustSound.Stop();
        }

        isThrusting = false; // reset every frame
    }

    public override void ApplyThrust()
    {
        base.ApplyThrust();
        isThrusting = true;
    }
}
