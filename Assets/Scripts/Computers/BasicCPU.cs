using UnityEngine;

public class BasicCPU : ComputerBase
{
    public ParticleSystem bootupEffect;
    public AudioSource humSound;

    private bool isActive = true;

    void Update()
    {
        if (isActive && autoAimEnabled)
        {
            if (bootupEffect != null && !bootupEffect.isPlaying) bootupEffect.Play();
            if (humSound != null && !humSound.isPlaying) humSound.Play();
        }
        else
        {
            if (bootupEffect != null && bootupEffect.isPlaying) bootupEffect.Stop();
            if (humSound != null && humSound.isPlaying) humSound.Stop();
        }

        base.Update();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
