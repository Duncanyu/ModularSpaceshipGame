using UnityEngine;

public class BarrelModule : WeaponModuleBase
{
    [Header("Barrel Effects")]
    public float lifetimeMultiplier = 1.0f;
    public float accuracyBonus = 0.0f;
    public float fireRateMultiplier = 1.0f; // Multiplies fire rate (1.2 = slower)
    public float projectileSpeedMultiplier = 1.0f; // Slows projectile (0.8 = slower)
    public float heatBonus = 0.0f;

    public float laserHeatMultiplier = 1.0f;
    public float laserRangeBonus = 0.0f;
    public float laserEnergyCostBonus = 0.0f;

    public float railgunSpeedMultiplier = 1.0f;
    public float railgunEnergyBonus = 0.0f;
    public float railgunFireRateMultiplier = 1.0f;
    public float railgunLifetimeMultiplier = 1.0f;

    private bool applied = false;

    public override void ApplyModuleEffect()
    {
        if (parentWeapon == null || applied) return;

        switch (parentWeapon.weaponName.ToLower())
        {
            case "laser":
                parentWeapon.ModifyHeatMultiplier(laserHeatMultiplier);
                parentWeapon.ModifyLifetime(laserRangeBonus);
                parentWeapon.ModifyEnergyCostPerSecond(laserEnergyCostBonus);
                parentWeapon.ModifyEnergyCostPerShot(laserEnergyCostBonus);
                break;

            case "railgun":
                parentWeapon.ModifyProjectileSpeed(railgunSpeedMultiplier);
                parentWeapon.ModifyEnergyCostPerShot(railgunEnergyBonus);
                parentWeapon.ModifyFireRate(railgunFireRateMultiplier);
                parentWeapon.ModifyLifetime(railgunLifetimeMultiplier);
                break;

            default:
                parentWeapon.ModifyLifetime(lifetimeMultiplier);
                parentWeapon.ModifyAccuracy(accuracyBonus);
                parentWeapon.ModifyFireRate(fireRateMultiplier);
                parentWeapon.ModifyProjectileSpeed(projectileSpeedMultiplier);
                parentWeapon.ModifyHeatFlat(heatBonus);
                break;
        }

        Debug.Log("[BarrelModule] Applied modifiers to " + parentWeapon.weaponName);
        applied = true;
    }

    public override void RemoveModuleEffect()
    {
        // implement later it gets fucking hard
    }
} 
