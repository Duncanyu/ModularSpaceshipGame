using UnityEngine;

public class BasicReactor : ReactorBase
{
    public ParticleSystem reactorEffect;
    public AudioSource reactorSound;

    private bool isActive = true;

    void Update()
    {
        if (isActive)
        {
            if (reactorEffect != null && !reactorEffect.isPlaying) reactorEffect.Play();
            if (reactorSound != null && !reactorSound.isPlaying) reactorSound.Play();
        }
        else
        {
            if (reactorEffect != null && reactorEffect.isPlaying) reactorEffect.Stop();
            if (reactorSound != null && reactorSound.isPlaying) reactorSound.Stop();
        }

        base.Update();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
