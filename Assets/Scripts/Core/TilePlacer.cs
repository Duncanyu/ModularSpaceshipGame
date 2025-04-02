using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacer : MonoBehaviour
{
    public HullGridManager gridManager;
    public Camera mainCamera;
    public TileComponent tilePrefab;
    public GameObject buildUI;
    public KeyCode toggleVisualKey = KeyCode.I;
    public KeyCode mirrorKey = KeyCode.M;
    public KeyCode clearKey = KeyCode.C;

    private TileComponent previewTile;
    private TileComponent mirrorPreviewTile;
    private float currentAngle = 0f;
    private bool tileVisualsVisible = true;
    private bool mirrorMode = false;
    private float lastClearPressTime = -1f;
    private float clearConfirmDelay = 0.4f;
    private bool isDragging = false;

    private TileSlot lastHoveredSlot;

    public GameObject mirrorIndicator;

    void Start()
    {
        SetTileToPlace(null);
        if (buildUI != null)
            buildUI.SetActive(false);
        if (mirrorIndicator != null)
            mirrorIndicator.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isActive = buildUI.activeSelf;
            buildUI.SetActive(!isActive);
            SetTileToPlace(null);
            ResetSlotVisual(lastHoveredSlot);
            return;
        }

        if (Input.GetKeyDown(toggleVisualKey))
        {
            tileVisualsVisible = !tileVisualsVisible;
            ToggleAllTileVisuals(tileVisualsVisible);
        }

        if (!buildUI.activeSelf)
        {
            ResetSlotVisual(lastHoveredSlot);
            return;
        }

        if (Input.GetKeyDown(mirrorKey))
        {
            mirrorMode = !mirrorMode;
            if (mirrorIndicator != null)
                mirrorIndicator.SetActive(mirrorMode);
        }

        if (Input.GetKeyDown(clearKey))
        {
            if (Time.time - lastClearPressTime <= clearConfirmDelay)
            {
                foreach (Transform child in gridManager.transform)
                {
                    TileSlot slot = child.GetComponent<TileSlot>();
                    if (slot != null) slot.RemoveComponent();
                }
                lastClearPressTime = -1f;
            }
            else
            {
                lastClearPressTime = Time.time;
            }
        }

        if (tilePrefab == null) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAngle -= 90f;
            currentAngle %= 360f;
        }

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        TileSlot targetSlot = FindClosestSlot(mouseWorldPos);

        if (lastHoveredSlot != null && lastHoveredSlot != targetSlot)
        {
            ResetSlotVisual(lastHoveredSlot);
        }

        if (targetSlot != null)
        {
            lastHoveredSlot = targetSlot;

            if (!targetSlot.IsOccupied && targetSlot.CanPlace(tilePrefab))
            {
                HighlightSlot(targetSlot, Color.green);
            }
            else
            {
                HighlightSlot(targetSlot, Color.red);
            }

            if (previewTile == null)
            {
                previewTile = Instantiate(tilePrefab);
                previewTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
            previewTile.transform.position = targetSlot.transform.position;
            previewTile.transform.rotation = Quaternion.Euler(0, 0, currentAngle + targetSlot.transform.eulerAngles.z);

            if (mirrorMode)
            {
                TileSlot mirrorSlot = FindMirrorSlot(targetSlot);
                if (mirrorSlot != null)
                {
                    if (mirrorPreviewTile == null)
                    {
                        mirrorPreviewTile = Instantiate(tilePrefab);
                        mirrorPreviewTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    }
                    mirrorPreviewTile.transform.position = mirrorSlot.transform.position;
                    mirrorPreviewTile.transform.rotation = Quaternion.Euler(0, 0, currentAngle + mirrorSlot.transform.eulerAngles.z);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (targetSlot.IsOccupied)
                {
                    TileComponent existing = targetSlot.currentComponent;
                    SetTileToPlace(existing);
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0) && targetSlot.IsOccupied)
            {
                targetSlot.RemoveComponent();
                if (mirrorMode)
                {
                    TileSlot mirrorSlot = FindMirrorSlot(targetSlot);
                    if (mirrorSlot != null && mirrorSlot.IsOccupied)
                        mirrorSlot.RemoveComponent();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                if (!targetSlot.IsOccupied && targetSlot.CanPlace(tilePrefab))
                {
                    PlaceTile(targetSlot);
                    if (mirrorMode)
                    {
                        TileSlot mirrorSlot = FindMirrorSlot(targetSlot);
                        if (mirrorSlot != null && !mirrorSlot.IsOccupied && mirrorSlot.CanPlace(tilePrefab))
                        {
                            PlaceTile(mirrorSlot);
                        }
                    }
                }
            }
        }
    }

    void ToggleAllTileVisuals(bool visible)
    {
        foreach (Transform child in gridManager.transform)
        {
            TileSlot slot = child.GetComponent<TileSlot>();
            if (slot != null)
            {
                slot.ToggleVisual(visible);
                if (slot.currentComponent != null)
                {
                    TileVisualMarker marker = slot.currentComponent.GetComponentInChildren<TileVisualMarker>();
                    if (marker != null) marker.ToggleVisuals();
                }
            }
        }
    }

    TileSlot FindClosestSlot(Vector2 position)
    {
        TileSlot closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform child in gridManager.transform)
        {
            TileSlot slot = child.GetComponent<TileSlot>();
            if (slot == null) continue;

            float dist = Vector2.Distance(position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = slot;
            }
        }

        return closest;
    }

    TileSlot FindMirrorSlot(TileSlot originalSlot)
    {
        foreach (Transform child in gridManager.transform)
        {
            TileSlot slot = child.GetComponent<TileSlot>();
            if (slot != null && slot.gridPosition.x == -originalSlot.gridPosition.x && slot.gridPosition.y == originalSlot.gridPosition.y)
            {
                return slot;
            }
        }
        return null;
    }

    void PlaceTile(TileSlot slot)
    {
        Quaternion finalRotation = Quaternion.Euler(0, 0, currentAngle + slot.transform.eulerAngles.z);
        TileComponent placedTile = Instantiate(tilePrefab, slot.transform.position, finalRotation);
        placedTile.AssignToSlot(slot);
        placedTile.transform.SetParent(slot.transform, true);
        slot.currentComponent = placedTile;

        WeaponTile weaponTile = placedTile.GetComponent<WeaponTile>();
        if (weaponTile != null)
        {
            weaponTile.TryRegisterWeapon();
        }
    }

    void HighlightSlot(TileSlot slot, Color color)
    {
        SpriteRenderer renderer = slot.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }
    }

    void ResetSlotVisual(TileSlot slot)
    {
        if (slot != null)
            slot.UpdateSlotColor();

        if (mirrorPreviewTile != null)
        {
            Destroy(mirrorPreviewTile.gameObject);
            mirrorPreviewTile = null;
        }
    }

    public void SetTileToPlace(TileComponent newTile)
    {
        tilePrefab = newTile;
        if (previewTile != null)
        {
            Destroy(previewTile.gameObject);
        }
        if (mirrorPreviewTile != null)
        {
            Destroy(mirrorPreviewTile.gameObject);
        }
        previewTile = null;
        mirrorPreviewTile = null;
        currentAngle = 0f;
    }
}