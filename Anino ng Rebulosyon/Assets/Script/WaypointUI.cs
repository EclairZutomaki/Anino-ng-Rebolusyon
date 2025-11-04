using UnityEngine;
using UnityEngine.UI;

public class WaypointUI : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Camera cam;
    public Canvas canvas;

    [Header("Behavior")]
    public float hideDistance = 2f;
    public float showDistance = 100f;
    public bool clampToScreen = true;
    public float edgeBuffer = 30f;
    public Vector2 baseSize = new Vector2(60, 60);
    public float minScaleAtMaxDist = 0.6f;
    public float maxScaleAtClose = 1.15f;

    [Header("Fade Settings")]
    public float fadeSpeed = 5f;       // how fast fade happens
    public float fadeEdgeRange = 5f;   // distance range before hideDistance/showDistance to start fading

    private RectTransform rt;
    private Image img;
    private Transform player;
    private Camera mainCam;
    private Color currentColor;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        mainCam = cam != null ? cam : Camera.main;
        if (canvas == null) canvas = GetComponentInParent<Canvas>();

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        currentColor = img.color;
    }

    void Update()
    {
        if (target == null || mainCam == null || rt == null)
        {
            img.enabled = false;
            return;
        }

        Vector3 camPos = mainCam.transform.position;
        Vector3 toTarget = target.position - camPos;
        float dist = toTarget.magnitude;

        // calculate fade alpha based on distance
        float targetAlpha = 1f;

        if (dist <= hideDistance)
            targetAlpha = 0f; // too close
        else if (dist < hideDistance + fadeEdgeRange)
            targetAlpha = Mathf.InverseLerp(hideDistance, hideDistance + fadeEdgeRange, dist);

        else if (dist > showDistance)
            targetAlpha = 0f; // too far
        else if (dist > showDistance - fadeEdgeRange)
            targetAlpha = Mathf.InverseLerp(showDistance, showDistance - fadeEdgeRange, dist);

        // smoothly fade
        currentColor = img.color;
        currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        img.color = currentColor;

        // fully hidden? skip positioning logic
        if (currentColor.a <= 0.01f)
            return;

        // world → screen point
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);
        bool isBehind = screenPos.z < 0f;

        if (isBehind)
            screenPos *= -1f;

        // convert to canvas space
        Vector2 canvasPos;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 viewportPosition = mainCam.ScreenToViewportPoint(screenPos);
            canvasPos.x = (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f);
            canvasPos.y = (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f);
        }
        else
        {
            Vector2 screenPoint = new Vector2(screenPos.x, screenPos.y);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, mainCam, out canvasPos);
        }

        // clamp to screen edges
        if (clampToScreen)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 max = canvasRect.sizeDelta * 0.5f;
            Vector2 min = -max;
            float halfW = rt.sizeDelta.x * 0.5f + edgeBuffer;
            float halfH = rt.sizeDelta.y * 0.5f + edgeBuffer;
            canvasPos.x = Mathf.Clamp(canvasPos.x, min.x + halfW, max.x - halfW);
            canvasPos.y = Mathf.Clamp(canvasPos.y, min.y + halfH, max.y - halfH);
        }

        rt.anchoredPosition = canvasPos;
        rt.rotation = Quaternion.identity;

        // optional scale by distance
        float t = Mathf.InverseLerp(showDistance, hideDistance, dist);
        float scale = Mathf.Lerp(minScaleAtMaxDist, maxScaleAtClose, t);
        rt.sizeDelta = baseSize * scale;
    }
}
