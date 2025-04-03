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
        TileSlot[] slots = GetComponentsInChildren<TileSlot>();

        foreach (var slot in slots)
        {
        }
    }

    public TileSlot GetSlotAt(Vector2Int position)
    {
        foreach (Transform child in transform)
        {
            TileSlot slot = child.GetComponent<TileSlot>();
            if (slot != null && slot.gridPosition == position)
                return slot;
        }
        return null;
    }
}
