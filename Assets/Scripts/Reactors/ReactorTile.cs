using UnityEngine;

public class ReactorTile : TileComponent
{
    private ReactorBase reactor;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);
        reactor = GetComponent<ReactorBase>();
        if (reactor == null)
        {
            Debug.LogWarning("[ReactorTile] No ReactorBase found on tile.");
            return;
        }
        reactor.TryRegisterReactor();
    }

    private void Update()
    {
        if (reactor == null) return;
    }
}