using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float baseDamage;
    public float fireRateRPM;
    public float heatPerShot;

    public int ammoCapacity;
    public float cooldownRate;

    public bool isEnergyBased;

    protected int currentAmmo;
    protected float currentHeat;

    protected WeaponModifierModule[] modules;

    protected virtual void Awake()
    {
        modules = GetComponents<WeaponModifierModule>();
        currentAmmo = ammoCapacity;
    }

    protected virtual void Update()
    {
        CoolDownWeapon();
    }

    public void Fire()
    {
        if (CanFire())
        {
            float finalDamage = CalculateDamage();
            Shoot(finalDamage);
            ApplyHeat();
            ConsumeAmmo();
        }
    }

    protected abstract void Shoot(float damage);

    protected bool CanFire()
    {
        return isEnergyBased ? true : currentAmmo > 0;
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
