using UnityEngine;

public class ReactorBase : DamageableTileBase
{
    public float energyGenerationRate = 5f;
    public float maxEnergyContribution = 20f;
    public bool canExplode = true;

    private SpaceshipController controller;
    private TileSlot assignedSlot;

    public void TryRegisterReactor()
    {
        controller = GetComponentInParent<SpaceshipController>();

        if (controller != null)
        {
            Debug.Log("[ReactorBase] Registering reactor. Adding max energy: " + maxEnergyContribution);
            controller.AddMaxEnergy(maxEnergyContribution);
        }
        else
        {
            Debug.LogWarning("[ReactorBase] Controller not found in parent hierarchy.");
        }

        assignedSlot = GetComponentInParent<TileSlot>();
        if (assignedSlot != null && assignedSlot.isReactorSlot)
        {
            energyGenerationRate *= 1.5f;
            Debug.Log("[ReactorBase] Boosted generation rate on reactor slot. New rate: " + energyGenerationRate);
        }
    }

    protected void Update()
    {
        if (controller != null)
        {
            float amount = energyGenerationRate * Time.deltaTime;
            Debug.Log("[ReactorBase] Adding energy: " + amount);
            controller.AddEnergy(amount);
        }
        else
        {
            Debug.LogWarning("[ReactorBase] Controller is null during Update.");
        }
    }

    protected override void OnDestroyed()
    {
        if (canExplode)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f);
            foreach (var hit in hits)
            {
                DamageableTileBase tile = hit.GetComponent<DamageableTileBase>();
                if (tile != null && tile != this)
                {
                    tile.TakeDamage(20);
                }
            }
        }
        base.OnDestroyed();
    }
}
