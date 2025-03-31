using UnityEngine;

public abstract class WeaponModifierModule : MonoBehaviour
{
    public virtual float ModifyDamage(float currentDamage) { return currentDamage; }
    public virtual float ModifyHeat(float currentHeat) { return currentHeat; }
}