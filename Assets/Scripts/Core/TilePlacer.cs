using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacer : MonoBehaviour
{
    public HullGridManager gridManager;
    public Camera mainCamera;
    public TileComponent tilePrefab;
    public GameObject buildUI;
    public KeyCode toggleVisualKey = KeyCode.I;

    private TileComponent previewTile;
    private float currentAngle = 0f;
    private bool tileVisualsVisible = true;

    void Start()
    {
        SetTileToPlace(null);
        if (buildUI != null)
            buildUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isActive = buildUI.activeSelf;
            buildUI.SetActive(!isActive);
            SetTileToPlace(null);
            return;
        }

        if (Input.GetKeyDown(toggleVisualKey))
        {
            Debug.Log("Apple");
            tileVisualsVisible = !tileVisualsVisible;
            ToggleAllTileVisuals(tileVisualsVisible);
        }

        if (!buildUI.activeSelf || tilePrefab == null) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAngle -= 90f;
            currentAngle %= 360f;
        }

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        TileSlot targetSlot = FindClosestSlot(mouseWorldPos);

        if (targetSlot != null)
        {
            if (previewTile == null)
            {
                previewTile = Instantiate(tilePrefab);
                previewTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }

            previewTile.transform.position = targetSlot.transform.position;
            previewTile.transform.rotation = Quaternion.Euler(0, 0, currentAngle + targetSlot.transform.eulerAngles.z);

            if (Input.GetMouseButtonDown(1))
            {
                if (targetSlot.IsOccupied)
                {
                    targetSlot.RemoveComponent();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (!targetSlot.IsOccupied && targetSlot.CanPlace(tilePrefab))
                {
                    PlaceTile(targetSlot);
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

    public void SetTileToPlace(TileComponent newTile)
    {
        tilePrefab = newTile;
        if (previewTile != null)
        {
            Destroy(previewTile.gameObject);
        }
        previewTile = null;
        currentAngle = 0f;
    }
}
