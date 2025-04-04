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

    public override void ApplyModuleEffect()
    {
        if (parentWeapon == null)
        {
            parentWeapon = GetComponentInParent<WeaponBase>();
            if (parentWeapon == null)
            {
                Debug.LogWarning($"[BarrelModule] Could not find WeaponBase in parent of {name}");
                return;
            }
        }

        Debug.Log($"[BarrelModule] parentWeapon is set on {gameObject.name}");

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
                Debug.Log("[BarrelModule] Default path used for standard weapon");
                parentWeapon.ModifyLifetime(lifetimeMultiplier);
                parentWeapon.ModifyAccuracy(accuracyBonus);
                parentWeapon.ModifyFireRate(fireRateMultiplier);
                parentWeapon.ModifyProjectileSpeed(projectileSpeedMultiplier);
                parentWeapon.ModifyHeatFlat(heatBonus);
                break;
        }

        Debug.Log($"[BarrelModule] Applied modifiers to {parentWeapon.weaponName}");
    }

    public override void RemoveModuleEffect()
    {
        // Implement later it gets fucking hard
    }
}
