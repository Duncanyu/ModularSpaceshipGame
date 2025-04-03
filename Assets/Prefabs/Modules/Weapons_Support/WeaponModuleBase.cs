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
        parentWeapon = GetComponentInParent<WeaponBase>();
        if (parentWeapon == null)
        {
            //Debug.LogWarning($"[WeaponModule] No parent WeaponBase found on {name}.");
        }
    }

    public virtual void ApplyModuleEffect() {}
    public virtual void RemoveModuleEffect() {}
} 
