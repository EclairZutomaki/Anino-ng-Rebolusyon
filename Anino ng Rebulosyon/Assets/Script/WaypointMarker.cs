using UnityEngine;
using UnityEngine.UI;

public class WaypointMarker : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public Image markerImage;
    public float edgeOffset = 50f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (target == null || player == null) return;

        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);

        // Check if target is behind the camera
        if (screenPos.z < 0)
        {
            screenPos.x = -screenPos.x;
            screenPos.y = -screenPos.y;
        }

        // Clamp marker to screen edges
        screenPos.x = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
        screenPos.y = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

        // Update marker position
        markerImage.transform.position = screenPos;

        // Optionally rotate marker to face the target
        Vector3 dir = (target.position - player.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        markerImage.transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
