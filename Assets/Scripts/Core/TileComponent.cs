using UnityEngine;

public abstract class TileComponent : MonoBehaviour
{
    public string componentName;
    public Vector2Int size = Vector2Int.one;
    public TileType type;
    public Sprite uiIcon;
    protected TileSlot parentSlot;

    public void AssignToSlot(TileSlot slot)
    {
        parentSlot = slot;
        transform.position = slot.transform.position;
    }

    public void ToggleVisual(bool visible)
    {
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = visible;
        }
    }
}

public enum TileType
{
    Engine,
    Reactor,
    Weapon,
    WeaponSupport,
    Armor,
    Computer,
    Support
}
