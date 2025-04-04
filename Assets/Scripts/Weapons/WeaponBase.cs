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
    protected TileSlot weaponSlot;

    protected float projectileLifetimeMultiplier = 1f;
    protected float projectileSpeedMultiplier = 1f;
    protected float weaponSpread = 0f;
    protected float heatMultiplier = 1f;

    private float baseFireRateRPM;
    private float baseHeatPerShot;
    private float baseEnergyCostPerShot;
    private float baseEnergyCostPerSecond;

    private float scanTimer = 0f;
    private float scanInterval = 1f;

    protected virtual void Awake()
    {
        modules = GetComponents<WeaponModifierModule>();
        currentAmmo = ammoCapacity;

        baseFireRateRPM = fireRateRPM;
        baseHeatPerShot = heatPerShot;
        baseEnergyCostPerShot = energyCostPerShot;
        baseEnergyCostPerSecond = energyCostPerSecond;
    }

    protected virtual void Start()
    {
        controller = GetComponentInParent<SpaceshipController>();

        TileComponent tileComponent = GetComponent<TileComponent>();
        if (tileComponent != null)
        {
            weaponSlot = tileComponent.AssignedSlot;
        }

        ScanForModules();
    }

    protected virtual void Update()
    {
        CoolDownWeapon();

        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            scanTimer = 0f;
            ScanForModules();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("[WeaponBase] Manual module rescan triggered by key press.");
            ScanForModules();
        }
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

    public void ScanForModules()
    {
        // Reset modifiable values to base first
        fireRateRPM = baseFireRateRPM;
        heatPerShot = baseHeatPerShot;
        energyCostPerShot = baseEnergyCostPerShot;
        energyCostPerSecond = baseEnergyCostPerSecond;

        WeaponModuleBase[] nearbyModules = GetComponentsInChildren<WeaponModuleBase>();
        Debug.Log($"[WeaponBase] Scanning for modules... found {nearbyModules.Length}");
        foreach (var module in nearbyModules)
        {
            Debug.Log($"[WeaponBase] Applying module (child): {module.name}");
            module.ApplyModuleEffect();
        }

        if (weaponSlot == null)
        {
            Debug.LogWarning("[WeaponBase] No TileSlot assigned to weapon. Cannot scan adjacent modules.");
            return;
        }

        HullGridManager gridManager = FindObjectOfType<HullGridManager>();
        if (gridManager == null)
        {
            Debug.LogWarning("[WeaponBase] No HullGridManager found.");
            return;
        }

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            TileSlot neighbor = gridManager.GetSlotAt(weaponSlot.gridPosition + dir);
            if (neighbor != null && neighbor.currentComponent != null)
            {
                WeaponModuleBase mod = neighbor.currentComponent.GetComponent<WeaponModuleBase>();
                if (mod != null)
                {
                    Debug.Log($"[WeaponBase] Applying module (adjacent): {mod.name}");
                    mod.ApplyModuleEffect();
                }
            }
        }
    }

    public virtual void ModifyRange(float multiplier) {}

    public virtual void ModifyAccuracy(float amount)
    {
        weaponSpread = Mathf.Max(weaponSpread - amount, 0f);
        Debug.Log($"[WeaponBase] Accuracy modified. New spread: {weaponSpread}");
    }

    public virtual void ModifyFireRate(float multiplier)
    {
        Debug.Log($"[WeaponBase] Modifying fire rate. Old: {fireRateRPM}, Multiplier: {multiplier}");
        fireRateRPM *= multiplier;
        Debug.Log($"[WeaponBase] New fire rate: {fireRateRPM}");
    }

    public virtual void ModifyProjectileSpeed(float multiplier)
    {
        projectileSpeedMultiplier *= multiplier;
        Debug.Log($"[WeaponBase] Projectile speed multiplier set to: {projectileSpeedMultiplier}");
    }

    public virtual void ModifyLifetime(float multiplier)
    {
        projectileLifetimeMultiplier *= multiplier;
        Debug.Log($"[WeaponBase] Projectile lifetime multiplier set to: {projectileLifetimeMultiplier}");
    }

    public virtual void ModifyHeatMultiplier(float multiplier)
    {
        heatMultiplier *= multiplier;
        Debug.Log($"[WeaponBase] Heat multiplier set to: {heatMultiplier}");
    }

    public virtual void ModifyHeatFlat(float amount)
    {
        heatPerShot += amount;
        Debug.Log($"[WeaponBase] Heat per shot now: {heatPerShot}");
    }

    public virtual void ModifyEnergyCostPerSecond(float amount)
    {
        energyCostPerSecond += amount;
        Debug.Log($"[WeaponBase] Energy cost per second now: {energyCostPerSecond}");
    }

    public virtual void ModifyEnergyCostPerShot(float amount)
    {
        energyCostPerShot += amount;
        Debug.Log($"[WeaponBase] Energy cost per shot now: {energyCostPerShot}");
    }
} 
