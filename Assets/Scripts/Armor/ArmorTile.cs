using UnityEngine;

public class ArmorTile : TileComponent
{
    private ArmorBase armor;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);
        TryRegisterArmor();
    }

    private void TryRegisterArmor()
    {
        armor = GetComponent<ArmorBase>();
        if (armor == null)
        {
            Debug.LogWarning("[ArmorTile] No ArmorBase found on tile.");
            return;
        }

        Ship ship = GetComponentInParent<Ship>();
        if (ship == null)
        {
            Debug.LogWarning("[ArmorTile] No Ship found in hierarchy.");
            return;
        }

        ShipHealth shipHealth = ship.GetComponent<ShipHealth>();
        if (shipHealth == null)
        {
            Debug.LogWarning("[ArmorTile] No ShipHealth found on Ship.");
            return;
        }

        shipHealth.RegisterArmor(armor);
        //Debug.Log("[ArmorTile] Armor registered with ship health.");
    }

    private void Update()
    {
        if (armor == null) return;

        if (armor.durability <= 0f)
        {
            Debug.Log($"[ArmorTile] Armor {gameObject.name} destroyed.");
        }
    }
}
