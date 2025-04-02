using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public Vector2Int gridPosition;
    public TileComponent currentComponent;

    public bool isComputerSlot;
    public bool isEngineSlot;
    public bool isReactorSlot;

    public Color emptyColor = Color.white;
    public Color filledColor = Color.green;
    public Color computerColor = Color.cyan;
    public Color engineColor = Color.yellow;
    public Color reactorColor = Color.red;

    private SpriteRenderer slotRenderer;
    private SpriteRenderer squareRenderer;

    public bool IsOccupied => currentComponent != null;

    private void Awake()
    {
        Transform squareChild = transform.Find("Square");
        if (squareChild != null)
        {
            squareRenderer = squareChild.GetComponent<SpriteRenderer>();
        }

        slotRenderer = GetComponent<SpriteRenderer>();
        UpdateSlotColor();
    }

    private void Update()
    {
        if (IsOccupied && squareRenderer.color != filledColor)
        {
            squareRenderer.color = filledColor;
        }
    }

    public bool CanPlace(TileComponent component)
    {
        if (IsOccupied) return false;

        if (component is EngineTile)
        {
            return isEngineSlot;
        }

        if (component is ReactorTile)
        {
            return !isComputerSlot && !isEngineSlot;
        }

        if (!isComputerSlot && !isEngineSlot && !isReactorSlot)
        {
            return true;
        }

        return false;
    }

    public void PlaceComponent(TileComponent component)
    {
        if (CanPlace(component))
        {
            currentComponent = Instantiate(component, transform.position, Quaternion.identity, transform);
            currentComponent.AssignToSlot(this);
            UpdateSlotColor();
        }
    }

    public void RemoveComponent()
    {
        if (currentComponent != null)
        {
            Destroy(currentComponent.gameObject);
            currentComponent = null;
            UpdateSlotColor();
        }
    }

    public void UpdateSlotColor()
    {
        if (squareRenderer == null) return;

        if (IsOccupied)
        {
            squareRenderer.color = filledColor;
        }
        else if (isComputerSlot)
        {
            squareRenderer.color = computerColor;
        }
        else if (isEngineSlot)
        {
            squareRenderer.color = engineColor;
        }
        else if (isReactorSlot)
        {
            squareRenderer.color = reactorColor;
        }
        else
        {
            squareRenderer.color = emptyColor;
        }
    }

    public void ToggleVisual(bool visible)
    {
        if (slotRenderer != null)
        {
            slotRenderer.enabled = visible;
        }

        if (squareRenderer != null)
        {
            squareRenderer.enabled = visible;
        }

        if (currentComponent != null)
        {
            currentComponent.ToggleVisual(visible);
        }
    }
}

public class TileVisualMarker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetVisualsVisible(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }

    public void ToggleVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }
}
