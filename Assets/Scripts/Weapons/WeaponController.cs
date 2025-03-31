using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponBase[] equippedWeapons;
    public KeyCode fireKey = KeyCode.Space;

    private float[] fireCooldowns;
    private List<WeaponBase> registeredWeapons = new List<WeaponBase>();

    void Start()
    {
        fireCooldowns = new float[equippedWeapons.Length];
        registeredWeapons.AddRange(equippedWeapons);
    }

    void Update()
    {
        for (int i = 0; i < registeredWeapons.Count; i++)
        {
            WeaponBase weapon = registeredWeapons[i];
            if (weapon == null)
            {
                // Suppressed null weapon warning
                continue;
            }

            if (fireCooldowns.Length <= i)
            {
                System.Array.Resize(ref fireCooldowns, registeredWeapons.Count);
            }

            fireCooldowns[i] -= Time.deltaTime;

            float fireInterval = 60f / weapon.fireRateRPM;

            if (Input.GetKey(fireKey) && fireCooldowns[i] <= 0f)
            {
                //Debug.Log($"[WeaponController] Attempting to fire weapon {i}: {weapon}");
                weapon.Fire();
                fireCooldowns[i] = fireInterval;
            }
        }
    }

    public void RegisterWeapon(WeaponBase weapon)
    {
        if (!registeredWeapons.Contains(weapon))
        {
            registeredWeapons.Add(weapon);
        }
    }
}
