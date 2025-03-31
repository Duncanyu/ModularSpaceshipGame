#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class TileSlotEditor : EditorWindow
{
    private GameObject selectedShip;
    private GameObject tileSlotPrefab;
    private Vector2Int brushPosition = Vector2Int.zero;
    private bool mirrorMode = false;

    private const float gridSize = 1f;

    [MenuItem("Tools/Tile Slot Editor")]
    public static void ShowWindow()
    {
        GetWindow<TileSlotEditor>("Tile Slot Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Tile Slot Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        selectedShip = (GameObject)EditorGUILayout.ObjectField("Ship Hull Root", selectedShip, typeof(GameObject), true);
        tileSlotPrefab = (GameObject)EditorGUILayout.ObjectField("Tile Slot Prefab", tileSlotPrefab, typeof(GameObject), false);

        brushPosition = EditorGUILayout.Vector2IntField("Brush Position", brushPosition);
        EditorGUILayout.LabelField("Rotation (Z)", "(disabled - fixed at 0)");

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("←")) brushPosition.x--;
        if (GUILayout.Button("→")) brushPosition.x++;
        if (GUILayout.Button("↑")) brushPosition.y++;
        if (GUILayout.Button("↓")) brushPosition.y--;
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUI.enabled = selectedShip != null && tileSlotPrefab != null;
        if (GUILayout.Button("Place Tile Slot")) PlaceTileSlot();
        GUI.enabled = selectedShip != null;
        if (GUILayout.Button("Remove Tile Slot")) RemoveTileSlot();
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        SceneView.duringSceneGui += HandleKeyboardInput;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui -= HandleKeyboardInput;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (selectedShip == null) return;

        Handles.color = new Color(0, 1, 1, 0.2f);
        Vector3 worldPos = selectedShip.transform.position + new Vector3(brushPosition.x * gridSize, brushPosition.y * gridSize, 0);
        Handles.DrawSolidRectangleWithOutline(
            new[] {
                worldPos + new Vector3(-0.5f, -0.5f, 0),
                worldPos + new Vector3(-0.5f,  0.5f, 0),
                worldPos + new Vector3( 0.5f,  0.5f, 0),
                worldPos + new Vector3( 0.5f, -0.5f, 0)
            },
            new Color(0, 1, 1, 0.15f),
            new Color(0, 1, 1, 0.4f)
        );

        SceneView.RepaintAll();
    }

    void HandleKeyboardInput(SceneView sceneView)
    {
        if (Event.current == null || Event.current.type != EventType.KeyDown) return;

        switch (Event.current.keyCode)
        {
            case KeyCode.UpArrow: brushPosition.y++; Event.current.Use(); break;
            case KeyCode.DownArrow: brushPosition.y--; Event.current.Use(); break;
            case KeyCode.LeftArrow: brushPosition.x--; Event.current.Use(); break;
            case KeyCode.RightArrow: brushPosition.x++; Event.current.Use(); break;
            case KeyCode.Return:
                if (selectedShip != null && tileSlotPrefab != null) PlaceTileSlot();
                Event.current.Use();
                break;
            case KeyCode.Backspace:
                if (selectedShip != null) RemoveTileSlot();
                Event.current.Use();
                break;
            case KeyCode.M:
                mirrorMode = !mirrorMode;
                Debug.Log("Mirror Mode: " + (mirrorMode ? "ON" : "OFF"));
                Event.current.Use();
                break;
            case KeyCode.C:
                if (selectedShip != null && EditorUtility.DisplayDialog("Confirm Clear", "Are you sure you want to clear all tile slots?", "Yes", "No"))
                {
                    ClearAllSlots();
                }
                Event.current.Use();
                break;
        }
    }

        void PlaceTileSlot()
    {
        if (SlotExistsAt(brushPosition)) return;

        Vector3 worldPos = selectedShip.transform.position + new Vector3(brushPosition.x * gridSize, brushPosition.y * gridSize, 0);

        GameObject newSlot = (GameObject)PrefabUtility.InstantiatePrefab(tileSlotPrefab);
        newSlot.transform.position = worldPos;
        newSlot.transform.rotation = Quaternion.identity;
        newSlot.transform.SetParent(selectedShip.transform);

        TileSlot slot = newSlot.GetComponent<TileSlot>();
        if (slot != null)
            slot.gridPosition = brushPosition;

        Undo.RegisterCreatedObjectUndo(newSlot, "Place Tile Slot");

        if (mirrorMode)
        {
            Vector2Int mirrored = new Vector2Int(-brushPosition.x, brushPosition.y);
            if (!SlotExistsAt(mirrored))
            {
                Vector3 mirrorPos = selectedShip.transform.position + new Vector3(mirrored.x * gridSize, mirrored.y * gridSize, 0);
                GameObject mirroredSlot = (GameObject)PrefabUtility.InstantiatePrefab(tileSlotPrefab);
                mirroredSlot.transform.position = mirrorPos;
                mirroredSlot.transform.rotation = Quaternion.identity;
                mirroredSlot.transform.SetParent(selectedShip.transform);

                TileSlot mirrorSlot = mirroredSlot.GetComponent<TileSlot>();
                if (mirrorSlot != null)
                    mirrorSlot.gridPosition = mirrored;

                Undo.RegisterCreatedObjectUndo(mirroredSlot, "Place Mirrored Tile Slot");
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    void RemoveTileSlot()
    {
        Vector2Int[] positions = mirrorMode ? new[] { brushPosition, new Vector2Int(-brushPosition.x, brushPosition.y) } : new[] { brushPosition };

        foreach (var pos in positions)
        {
            Vector3 worldPos = selectedShip.transform.position + new Vector3(pos.x * gridSize, pos.y * gridSize, 0);
            float searchRadius = 0.1f;

            foreach (Transform child in selectedShip.transform)
            {
                if (Vector2.Distance(child.position, worldPos) < searchRadius && child.GetComponent<TileSlot>() != null)
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    break;
                }
            }
        }
    }

    void ClearAllSlots()
    {
        if (selectedShip == null) return;

        var children = new System.Collections.Generic.List<Transform>();
        foreach (Transform child in selectedShip.transform)
        {
            if (child.GetComponent<TileSlot>() != null)
                children.Add(child);
        }

        foreach (Transform child in children)
        {
            Undo.DestroyObjectImmediate(child.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    bool SlotExistsAt(Vector2Int gridPos)
    {
        Vector3 checkPos = selectedShip.transform.position + new Vector3(gridPos.x * gridSize, gridPos.y * gridSize, 0);
        float searchRadius = 0.1f;

        foreach (Transform child in selectedShip.transform)
        {
            if (Vector2.Distance(child.position, checkPos) < searchRadius && child.GetComponent<TileSlot>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
#endif