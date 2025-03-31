using UnityEngine;

public class EngineTile : TileComponent
{
    private EngineBase engine;

    public new void AssignToSlot(TileSlot slot)
    {
        base.AssignToSlot(slot);
        TryRegisterEngine();
    }

    private void TryRegisterEngine()
    {
        engine = GetComponent<EngineBase>();
        if (engine == null)
        {
            Debug.LogWarning("[EngineTile] No EngineBase found on tile.");
            return;
        }

        engine.TryRegisterEngine();
    }

    private void Update()
    {
        if (engine == null) return;
    }
}