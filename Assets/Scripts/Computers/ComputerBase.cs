using UnityEngine;

public class ComputerBase : DamageableTileBase
{
    [Header("AI Assistance")]
    public bool autoAimEnabled = false;
    public float autoAimRange = 10f;
    public float targetUpdateRate = 0.5f;
    public float energyCostPerSecond = 1f;

    protected SpaceshipController controller;
    protected float lastUpdateTime = 0f;
    private bool hasPower = true;

    protected virtual void Start()
    {
        controller = GetComponentInParent<SpaceshipController>();
    }

    protected virtual void Update()
    {
        if (!autoAimEnabled || controller == null) return;

        float energyRequired = energyCostPerSecond * Time.deltaTime;
        hasPower = controller.TryConsumeEnergy(energyRequired);
        if (!hasPower) return;

        if (Time.time - lastUpdateTime > targetUpdateRate)
        {
            AcquireAndAssistWeapons();
            lastUpdateTime = Time.time;
        }
    }

    protected virtual void AcquireAndAssistWeapons()
    {
        // Placeholder for auto-aim logic
        // In the future: find nearest enemy within range and send direction to turrets
    }

    public void ToggleAutoAim()
    {
        autoAimEnabled = !autoAimEnabled;
    }
} 
