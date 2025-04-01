using UnityEngine;

public abstract class EngineBase : DamageableTileBase
{
    [Range(0f, 1f)]
    public float speedBoostPercent = 0.1f;
    [Range(0f, 1f)]
    public float turnBoostPercent = 0.1f;
    public float energyCostPerSecond = 1f;

    private bool registered = false;
    private SpaceshipController controller;
    private bool hasEnergy = true;

    protected virtual void Start()
    {
        TryRegisterEngine();
    }

    protected virtual void OnDestroy()
    {
        if (registered && controller != null)
        {
            controller.UnregisterEngine(this);
        }
    }

    public void TryRegisterEngine()
    {
        if (!registered)
        {
            controller = GetComponentInParent<SpaceshipController>();
            if (controller != null)
            {
                controller.RegisterEngine(this);
                registered = true;
            }
        }
    }

    public float GetSpeedContribution(float maxSpeed)
    {
        return hasEnergy ? maxSpeed * speedBoostPercent : 0f;
    }

    public float GetTurnContribution(float maxTurnSpeed)
    {
        return hasEnergy ? maxTurnSpeed * turnBoostPercent : 0f;
    }

    public void ConsumeEnergy(float deltaTime)
    {
        if (!registered || controller == null) return;

        float energyRequired = energyCostPerSecond * deltaTime;
        hasEnergy = controller.TryConsumeEnergy(energyRequired);
    }

    public virtual void ApplyThrust() {}
} 