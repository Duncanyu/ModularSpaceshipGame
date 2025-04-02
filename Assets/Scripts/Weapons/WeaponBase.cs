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

    protected int currentAmmo;
    protected float currentHeat;

    protected WeaponModifierModule[] modules;
    protected SpaceshipController controller;

    protected virtual void Awake()
    {
        modules = GetComponents<WeaponModifierModule>();
        currentAmmo = ammoCapacity;
        controller = GetComponentInParent<SpaceshipController>();
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

    protected abstract void Shoot(float damage);

    protected bool CanFire()
    {
        return isEnergyBased || currentAmmo > 0;
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
        currentHeat += heatPerShot;
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
}
