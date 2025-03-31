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

    public virtual float GetSpeedContribution(float maxSpeed)
    {
        return maxSpeed * speedBoostPercent;
    }

    public virtual float GetTurnContribution(float maxTurnSpeed)
    {
        return maxTurnSpeed * turnBoostPercent;
    }

    public virtual void ApplyThrust() {}
} 
