using UnityEngine;

public class BarrelModule : WeaponModuleBase
{
    [Header("Barrel Effects")]
    public float lifetimeMultiplier = 1.0f;
    public float accuracyBonus = 0.0f;
    public float fireRateMultiplier = 1.0f;
    public float projectileSpeedMultiplier = 1.0f;
    public float heatBonus = 0.0f;

    public float laserHeatMultiplier = 1.0f;
    public float laserRangeBonus = 0.0f;
    public float laserEnergyCostBonus = 0.0f;

    public float railgunSpeedMultiplier = 1.0f;
    public float railgunEnergyBonus = 0.0f;
    public float railgunFireRateMultiplier = 1.0f;
    public float railgunLifetimeMultiplier = 1.0f;

    public override void ApplyModuleEffect()
    {
        if (parentWeapon == null)
        {
            Debug.LogWarning($"[BarrelModule] parentWeapon is null on {gameObject.name}, skipping effect.");
            return;
        }

        Debug.Log($"[BarrelModule] Applying to: {parentWeapon.weaponName}");

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
                Debug.Log("[BarrelModule] Applying default projectile weapon effects");
                parentWeapon.ModifyLifetime(lifetimeMultiplier);
                parentWeapon.ModifyAccuracy(accuracyBonus);
                parentWeapon.ModifyFireRate(fireRateMultiplier);
                parentWeapon.ModifyProjectileSpeed(projectileSpeedMultiplier);
                parentWeapon.ModifyHeatFlat(heatBonus);
                break;
        }

        Debug.Log($"[BarrelModule] Modifiers successfully applied to {parentWeapon.weaponName}");
    }

    public override void RemoveModuleEffect()
    {
        // Implement later it gets fucking hard
    }
}