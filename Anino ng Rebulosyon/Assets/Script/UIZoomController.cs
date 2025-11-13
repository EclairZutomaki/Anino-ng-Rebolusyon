using UnityEngine;

public class UIZoomController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;          // How fast it zooms
    public float minZoom = 0.5f;            // Minimum scale
    public float maxZoom = 2f;              // Maximum scale

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Vector3 scale = rect.localScale;
            scale += Vector3.one * scroll * zoomSpeed;

            // Clamp zoom range
            scale.x = Mathf.Clamp(scale.x, minZoom, maxZoom);
            scale.y = Mathf.Clamp(scale.y, minZoom, maxZoom);
            scale.z = 1f; // Keep flat (2D UI)

            rect.localScale = scale;
        }
    }
}
