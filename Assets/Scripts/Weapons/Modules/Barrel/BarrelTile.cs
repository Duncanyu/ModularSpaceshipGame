using UnityEngine;

public class BarrelTile : TileComponent
{
    private BarrelModule barrel;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);
        barrel = GetComponent<BarrelModule>();

        if (barrel == null)
        {
            //Debug.LogWarning("[BarrelTile] No BarrelModule found on tile.");
        }
    }

    private void Update()
    {
        if (barrel == null) return;
    }
} 