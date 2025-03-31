using UnityEngine;

public class HullGridManager : MonoBehaviour
{
    public TileSlot[,] Grid { get; private set; }

    void Awake()
    {
        CacheGridSlots();
    }

    void CacheGridSlots()
    {
        // Get all child TileSlots in hierarchy
        TileSlot[] slots = GetComponentsInChildren<TileSlot>();

        // Optionally populate a grid lookup if needed
        foreach (var slot in slots)
        {
            // You can use slot.gridPosition here if you want to organize into a 2D array
            // For now, TilePlacer just uses the transform children, so this is optional
        }
    }

    public TileSlot GetSlotAt(Vector2Int position)
    {
        // This method is now optional unless you're using gridPosition lookups
        foreach (Transform child in transform)
        {
            TileSlot slot = child.GetComponent<TileSlot>();
            if (slot != null && slot.gridPosition == position)
                return slot;
        }
        return null;
    }
}
