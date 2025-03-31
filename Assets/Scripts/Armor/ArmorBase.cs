using UnityEngine;

public abstract class ArmorBase : DamageableTileBase
{
    [Header("Armor Stats")]
    public float regenRate = 0f;
    public float regenCooldown = 5f;
    public float energyCostPerSecond = 0f;

    [Header("Optional Energy Generation")]
    public bool generatesEnergy = false;
    public float energyGenerationRate = 0f;

    private float regenTimer = 0f;

    protected virtual void Update()
    {
        regenTimer += Time.deltaTime;

        if (regenRate > 0f && durability < maxDurability && regenTimer >= regenCooldown)
        {
            durability += regenRate * Time.deltaTime;
            durability = Mathf.Min(durability, maxDurability);
        }

        if (generatesEnergy)
        {
            GenerateEnergy();
        }
    }

    protected virtual void GenerateEnergy()
    {
        Debug.Log($"[ArmorBase] Generated {energyGenerationRate * Time.deltaTime} energy.");
    }
}
