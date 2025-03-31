using UnityEngine;

public class StandardThruster : EngineBase
{
    public float thrustSpeedPercent = 0.1f;
    public float turnSpeedPercent = 0.1f;
    public ParticleSystem thrustEffect;
    public AudioSource thrustSound;

    private bool isThrusting;

    public override float GetSpeedContribution(float maxSpeed)
    {
        return maxSpeed * thrustSpeedPercent;
    }

    public override float GetTurnContribution(float maxTurnSpeed)
    {
        return maxTurnSpeed * turnSpeedPercent;
    }

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
