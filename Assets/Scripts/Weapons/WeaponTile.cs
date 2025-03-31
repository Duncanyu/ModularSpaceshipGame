using UnityEngine;

public class WeaponTile : TileComponent
{
    private WeaponBase weapon;

    public void TryRegisterWeapon()
    {
        weapon = GetComponent<WeaponBase>();
        //Debug.Log($"[WeaponTile] Found weapon: {weapon}");

        WeaponController controller = GetComponentInParent<WeaponController>();
        //Debug.Log($"[WeaponTile] Found controller: {controller}");

        if (controller != null && weapon != null)
        {
            controller.RegisterWeapon(weapon);
            //Debug.Log("[WeaponTile] Registered weapon with controller.");
        }

        if (weapon != null)
        {
            weapon.transform.rotation = transform.rotation;
        }
    }
}
