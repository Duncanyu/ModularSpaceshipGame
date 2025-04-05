using UnityEngine;

public class WeaponTile : TileComponent
{
    private WeaponBase weapon;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);

        weapon = GetComponent<WeaponBase>();
        if (weapon != null)
        {
            weapon.AssignSlot(slot);
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
        }
    }
}
