using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    private List<ArmorBase> registeredArmor = new List<ArmorBase>();

    public void RegisterArmor(ArmorBase armor)
    {
        if (!registeredArmor.Contains(armor))
        {
            registeredArmor.Add(armor);
            //Debug.Log("[ShipHealth] Registered armor: " + armor.name);
        }
    }

    public float GetTotalArmorDurability()
    {
        float total = 0f;
        foreach (var armor in registeredArmor)
        {
            total += armor.durability;
        }
        return total;
    }

    public float GetMaxArmorDurability()
    {
        float total = 0f;
        foreach (var armor in registeredArmor)
        {
            total += armor.maxDurability;
        }
        return total;
    }

    public float GetArmorIntegrityPercent()
    {
        float max = GetMaxArmorDurability();
        if (max == 0f) return 0f;
        return GetTotalArmorDurability() / max;
    }

    public void RemoveArmor(ArmorBase armor)
    {
        if (registeredArmor.Contains(armor))
        {
            registeredArmor.Remove(armor);
            //Debug.Log("[ShipHealth] Removed armor: " + armor.name);
        }
    }
}
