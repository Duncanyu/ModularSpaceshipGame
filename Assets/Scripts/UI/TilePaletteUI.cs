using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TilePaletteUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPrefab;
    public Transform contentParent;
    public GameObject panelRoot;
    public Button toggleButton;

    [Header("Tile Options")]
    public List<TileComponent> availableTiles;

    [Header("Placer Reference")]
    public TilePlacer tilePlacer;

    private bool isVisible = true;

    [Header("Keybinds")]
    public KeyCode toggleKey = KeyCode.B;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanelVisibility();
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("ToggleTileKey"))
        {
            string savedKey = PlayerPrefs.GetString("ToggleTileKey");
            if (System.Enum.TryParse(savedKey, out KeyCode savedKeyCode))
            {
                toggleKey = savedKeyCode;
            }
        }
        PopulateTileList();
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(TogglePanelVisibility);
        }
    }

    void PopulateTileList()
    {
        Debug.Log($"[TilePaletteUI] Populating {availableTiles.Count} tiles...");
    {
        foreach (TileComponent tileData in availableTiles)
        {
            TileComponent tile = tileData;
            GameObject buttonObj = Instantiate(buttonPrefab, contentParent);
            Button btn = buttonObj.GetComponent<Button>();

            Transform iconTransform = buttonObj.transform.Find("TileIcon");
            Image iconImage = null;

            if (iconTransform != null)
            {
                iconImage = iconTransform.GetComponent<Image>();
                Debug.Log("[TilePaletteUI] Found TileIcon object in prefab.");
            }
            else
            {
                iconImage = buttonObj.GetComponentInChildren<Image>();
                Debug.LogWarning("[TilePaletteUI] Could not find 'TileIcon' directly, using fallback to first child Image.");
            }
            Sprite previewSprite = tile.uiIcon != null ? tile.uiIcon : tile.GetComponent<SpriteRenderer>()?.sprite;

            if (iconImage != null)
            {
                if (previewSprite != null)
                {
                    iconImage.sprite = previewSprite;
                    Debug.Log($"[TilePaletteUI] Assigned sprite: {previewSprite.name} to button for tile: {tile.name}");
                }
                else
                {
                    Debug.LogWarning($"[TilePaletteUI] No sprite found for tile: {tile.name}");
                }
            }

            btn.onClick.AddListener(() => tilePlacer.SetTileToPlace(tile));
            }
        }
    }

    public void TogglePanelVisibility()
    {
        isVisible = !isVisible;
        panelRoot.SetActive(isVisible);
    }
}
