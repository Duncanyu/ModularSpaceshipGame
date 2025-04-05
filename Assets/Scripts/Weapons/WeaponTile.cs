using UnityEngine;

public class WeaponTile : TileComponent
{
    private WeaponBase weapon;

    private void Start()
    {
        if (AssignedSlot == null)
        {
            TileSlot foundSlot = GetComponentInParent<TileSlot>();
            if (foundSlot != null)
            {
                AssignToSlot(foundSlot);
                Debug.Log("[WeaponTile] Slot was missing. Assigned manually.");
            }
        }

        TryRegisterWeapon();
    }

    public void TryRegisterWeapon()
    {
        weapon = GetComponent<WeaponBase>();

        WeaponController controller = GetComponentInParent<WeaponController>();
        if (controller != null && weapon != null)
        {
            controller.RegisterWeapon(weapon);
        }

        SpaceshipController ship = GetComponentInParent<SpaceshipController>();
        if (ship != null && weapon != null)
        {
            ship.RegisterWeapon(weapon);
        }

        if (weapon != null)
        {
            weapon.transform.rotation = transform.rotation;

            TileComponent tileComponent = GetComponent<TileComponent>();
            if (tileComponent != null && tileComponent.AssignedSlot != null)
            {
                weapon.AssignSlot(tileComponent.AssignedSlot);
                Debug.Log("[WeaponTile] Assigned slot to WeaponBase using AssignSlot().");
                weapon.ScanForModules();
            }
        }
    }
} 
