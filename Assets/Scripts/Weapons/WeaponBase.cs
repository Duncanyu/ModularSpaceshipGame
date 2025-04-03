using UnityEngine;

public abstract class WeaponBase : DamageableTileBase
{
    public string weaponName;
    public float baseDamage;
    public float fireRateRPM;
    public float heatPerShot;

    public int ammoCapacity;
    public float cooldownRate;

    public bool isEnergyBased;
    public float energyCostPerShot = 5f;
    public float energyCostPerSecond = 1f;
    public float resourceCostPerSecond = 0f;

    protected int currentAmmo;
    protected float currentHeat;
    protected bool hasPower = true;

    protected WeaponModifierModule[] modules;
    protected SpaceshipController controller;

    protected float projectileLifetimeMultiplier = 1f;
    protected float projectileSpeedMultiplier = 1f;
    protected float weaponSpread = 0f;
    protected float heatMultiplier = 1f;

    protected virtual void Awake()
    {
        modules = GetComponents<WeaponModifierModule>();
        currentAmmo = ammoCapacity;
    }

    protected virtual void Start()
    {
        controller = GetComponentInParent<SpaceshipController>();
        ScanForModules();
    }

    protected virtual void Update()
    {
        CoolDownWeapon();
    }

    public void Fire()
    {
        if (CanFire())
        {
            if (isEnergyBased && controller != null)
            {
                if (!controller.TryConsumeEnergy(energyCostPerShot)) return;
            }

            float finalDamage = CalculateDamage();
            Shoot(finalDamage);
            ApplyHeat();
            ConsumeAmmo();
        }
    }

    public void DrainPassiveEnergy(float deltaTime)
    {
        if (controller != null && energyCostPerSecond > 0f)
        {
            hasPower = controller.TryConsumeEnergy(energyCostPerSecond * deltaTime);
        }
    }

    protected abstract void Shoot(float damage);

    protected bool CanFire()
    {
        return hasPower && (isEnergyBased || currentAmmo > 0);
    }

    protected float CalculateDamage()
    {
        float damage = baseDamage;
        foreach (var mod in modules)
        {
            damage = mod.ModifyDamage(damage);
        }
        return damage;
    }

    protected void ApplyHeat()
    {
        currentHeat += heatPerShot * heatMultiplier;
        foreach (var mod in modules)
        {
            currentHeat = mod.ModifyHeat(currentHeat);
        }
    }

    protected void ConsumeAmmo()
    {
        if (!isEnergyBased)
            currentAmmo -= 1;
    }

    protected void CoolDownWeapon()
    {
        currentHeat = Mathf.Max(currentHeat - cooldownRate * Time.deltaTime, 0);
    }

    protected void ScanForModules()
    {
        WeaponModuleBase[] nearbyModules = GetComponentsInChildren<WeaponModuleBase>();
        foreach (var module in nearbyModules)
        {
            module.ApplyModuleEffect();
        }
    }

    public virtual void ModifyRange(float multiplier) {}

    public virtual void ModifyAccuracy(float amount)
    {
        weaponSpread = Mathf.Max(weaponSpread - amount, 0f);
    }

    public virtual void ModifyFireRate(float multiplier)
    {
        fireRateRPM *= multiplier;
    }

    public virtual void ModifyProjectileSpeed(float multiplier)
    {
        projectileSpeedMultiplier *= multiplier;
    }

    public virtual void ModifyLifetime(float multiplier)
    {
        projectileLifetimeMultiplier *= multiplier;
    }

    public virtual void ModifyHeatMultiplier(float multiplier)
    {
        heatMultiplier *= multiplier;
    }

    public virtual void ModifyHeatFlat(float amount)
    {
        heatPerShot += amount;
    }

    public virtual void ModifyEnergyCostPerSecond(float amount)
    {
        energyCostPerSecond += amount;
    }

    public virtual void ModifyEnergyCostPerShot(float amount)
    {
        energyCostPerShot += amount;
    }
} 
