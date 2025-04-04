using UnityEngine;

public abstract class WeaponModuleBase : MonoBehaviour
{
    public enum ModuleType
    {
        Barrel,
        AmmoRack,
        Autoloader,
        TurretBase,
        BeamFocuser,
        PulseCapacitor,
        Overcharger,
        FuelTank,
        Warhead,
        GuidanceCore,
        ThrusterPack,
        TrackingEnhancer,
        SensorArray,
        HeatSink,
        StabilityMount
    }

    [Header("Module Settings")]
    public ModuleType moduleType;

    protected WeaponBase parentWeapon;

    protected virtual void Start()
    {
    }

    public void SetWeapon(WeaponBase weapon)
    {
        parentWeapon = weapon;
    }

    public virtual void ApplyModuleEffect()
    {
        if (parentWeapon == null)
        {
            parentWeapon = GetComponentInParent<WeaponBase>();
            if (parentWeapon == null)
            {
                Debug.LogWarning($"[WeaponModuleBase] No WeaponBase found on {name}, cannot apply effect.");
                return;
            }
        }
    }

    public virtual void RemoveModuleEffect() {}
} 
