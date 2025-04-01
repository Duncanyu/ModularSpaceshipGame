using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.2f;
    public Vector3 offset;
    public float zoomLerpSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomOutDistanceFactor = 0.1f;
    public float zoomOutSizeFactor = 0.5f;
    public float scrollZoomSensitivity = 2f;
    public float panResetThreshold = 0.1f;
    public KeyCode fireKey = KeyCode.Space;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private HullGridManager gridManager;
    private Rigidbody2D targetRb;
    private float manualZoomOffset = 0f;
    private float baseZoom = 0f;
    private Vector3 panOffset = Vector3.zero;
    private bool isPanning = false;
    private Vector3 lastMousePosition;
    private float maxPanDistance = 10f;

    private void Awake()
    {
        cam = Camera.main;
        gridManager = FindObjectOfType<HullGridManager>();
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector3 delta = cam.ScreenToWorldPoint(lastMousePosition) - cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 potentialPan = panOffset + delta;
            float maxPan = CalculateMaxPanDistance();
            if (potentialPan.magnitude <= maxPan)
            {
                panOffset = potentialPan;
            }
            else
            {
                panOffset = potentialPan.normalized * maxPan;
            }
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKey(fireKey))
        {
            panOffset = Vector3.Lerp(panOffset, Vector3.zero, Time.deltaTime * 5f);
        }

        Vector3 desiredPosition = target.position + offset + panOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        float distanceZoom = 0f;
        if (targetRb != null)
        {
            distanceZoom = targetRb.linearVelocity.magnitude * zoomOutDistanceFactor;
        }

        float sizeZoom = 0f;
        if (gridManager != null)
        {
            Vector3 center = gridManager.transform.position;
            Vector3 localMin = new Vector3(float.MaxValue, float.MaxValue);
            Vector3 localMax = new Vector3(float.MinValue, float.MinValue);

            foreach (Transform child in gridManager.transform)
            {
                if (child.GetComponent<TileSlot>())
                {
                    Vector3 localPos = gridManager.transform.InverseTransformPoint(child.position);
                    localMin = Vector3.Min(localMin, localPos);
                    localMax = Vector3.Max(localMax, localPos);
                }
            }

            float width = localMax.x - localMin.x;
            float height = localMax.y - localMin.y;
            float shipSize = Mathf.Max(width, height);
            sizeZoom = shipSize * zoomOutSizeFactor;

            float sizeFactorForDistanceZoom = Mathf.Max(1f, shipSize / 10f);
            distanceZoom *= sizeFactorForDistanceZoom;
        }

        float scrollDelta = -Input.mouseScrollDelta.y * scrollZoomSensitivity;
        float proposedZoom = baseZoom + manualZoomOffset + scrollDelta;
        float clampedZoom = Mathf.Clamp(proposedZoom, minZoom, baseZoom);
        manualZoomOffset = clampedZoom - baseZoom;

        float baseTargetZoom = Mathf.Clamp(Mathf.Max(distanceZoom, sizeZoom), minZoom, maxZoom);
        baseZoom = baseTargetZoom;

        float targetZoom = baseZoom + manualZoomOffset;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

    private float CalculateMaxPanDistance()
    {
        if (gridManager == null) return maxPanDistance;

        Bounds bounds = new Bounds(gridManager.transform.position, Vector3.zero);
        foreach (Transform child in gridManager.transform)
        {
            if (child.GetComponent<TileSlot>())
            {
                bounds.Encapsulate(child.position);
            }
        }

        float scaleFactor = Mathf.Max(bounds.size.x, bounds.size.y);
        return Mathf.Max(maxPanDistance, scaleFactor * 0.5f);
    }
}
