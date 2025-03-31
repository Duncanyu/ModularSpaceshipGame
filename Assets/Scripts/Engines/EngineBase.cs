using UnityEngine;

public abstract class EngineBase : DamageableTileBase
{
    [Range(0f, 1f)]
    public float speedBoostPercent = 0.1f;
    [Range(0f, 1f)]
    public float turnBoostPercent = 0.1f;
    public float energyCostPerSecond = 1f;

    private bool registered = false;

    protected virtual void Start()
    {
        TryRegisterEngine();
    }

    protected virtual void OnDestroy()
    {
        if (registered && PlayerController.Instance != null)
        {
            PlayerController.Instance.UnregisterEngine(this);
        }
    }

    public void TryRegisterEngine()
    {
        if (!registered && PlayerController.Instance != null)
        {
            PlayerController.Instance.RegisterEngine(this);
            registered = true;
        }
    }
} 
