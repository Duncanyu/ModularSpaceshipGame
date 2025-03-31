using UnityEngine;

public class Armor1 : ArmorBase
{
    [Header("Armor1 Stats")]
    public float extraExplosionResistance = 10f;
    public float visualEffectDuration = 1.5f;

    protected override void OnDestroyed()
    {
        //Debug.Log($"{gameObject.name} armor destroyed.");
        base.OnDestroyed();
    }

    private void Start()
    {
        //Debug.Log($"Armor1 initialized with {durability}/{maxDurability} durability.");
    }

    // Optional: Override TakeDamage if you want custom behavior
    // public override void TakeDamage(float amount)
    // {
    //     base.TakeDamage(amount - extraExplosionResistance);
    // }
}
