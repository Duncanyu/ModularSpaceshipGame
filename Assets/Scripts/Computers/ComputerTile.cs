using UnityEngine;

public class ComputerTile : TileComponent
{
    private ComputerBase computer;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);
        computer = GetComponent<ComputerBase>();
        if (computer == null)
        {
            Debug.LogWarning("[ComputerTile] No ComputerBase found on tile.");
        }
    }

    private void Update()
    {
        if (computer == null) return;
    }
}