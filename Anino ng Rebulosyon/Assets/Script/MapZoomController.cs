using UnityEngine;
using UnityEngine.EventSystems;

public class MapZoomController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.2f;
    public float minZoom = 1f;
    public float maxZoom = 3f;

    [Header("Drag Settings")]
    public float dragSpeed = 1f;

    private RectTransform mapContent;
    private RectTransform viewport;
    private Vector2 lastMousePos;
    private bool isDragging;

    void Start()
    {
        viewport = GetComponent<RectTransform>();
        mapContent = transform.GetChild(0).GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // Reset drag state whenever this panel is enabled
        isDragging = false;
        lastMousePos = Vector2.zero;
    }

    void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && mapContent != null)
        {
            Vector3 scale = mapContent.localScale;
            scale += Vector3.one * scroll * zoomSpeed;
            scale.x = Mathf.Clamp(scale.x, minZoom, maxZoom);
            scale.y = Mathf.Clamp(scale.y, minZoom, maxZoom);
            mapContent.localScale = scale;

            ClampMapPosition();
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
            isDragging = true;

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging && mapContent != null)
        {
            Vector2 mousePos = Input.mousePosition;
            if (lastMousePos != Vector2.zero)
            {
                Vector2 delta = (mousePos - lastMousePos) * dragSpeed;
                mapContent.anchoredPosition += delta;
                ClampMapPosition();
            }
            lastMousePos = mousePos;
        }
        else
        {
            lastMousePos = Vector2.zero;
        }
    }

    void ClampMapPosition()
    {
        if (mapContent == null || viewport == null) return;

        Vector2 contentSize = mapContent.rect.size * mapContent.localScale;
        Vector2 viewportSize = viewport.rect.size;

        float limitX = Mathf.Max(0, (contentSize.x - viewportSize.x) / 2);
        float limitY = Mathf.Max(0, (contentSize.y - viewportSize.y) / 2);

        Vector2 pos = mapContent.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        pos.y = Mathf.Clamp(pos.y, -limitY, limitY);

        mapContent.anchoredPosition = pos;
    }
}
