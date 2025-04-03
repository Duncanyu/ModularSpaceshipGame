using UnityEngine;

public class WeaponTile : TileComponent
{
    private WeaponBase weapon;

    private void Start()
    {
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
