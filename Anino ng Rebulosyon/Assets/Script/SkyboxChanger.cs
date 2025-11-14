using UnityEngine;
using UnityEngine.Rendering;

public class SkyboxChanger : MonoBehaviour
{
    [Header("Skybox Material to Use When Active")]
    public Material newSkybox;

    [Header("Optional: Restore old skybox when deactivated")]
    public bool revertOnDisable = true;

    private Material originalSkybox;

    void OnEnable()
    {
        // Make ambient lighting follow the skybox
        RenderSettings.ambientMode = AmbientMode.Skybox;

        // Save current skybox
        originalSkybox = RenderSettings.skybox;

        // Apply new skybox
        if (newSkybox != null)
            RenderSettings.skybox = newSkybox;

        // Force lighting update (Fix #1)
        DynamicGI.UpdateEnvironment();
    }

    void OnDisable()
    {
        if (revertOnDisable && originalSkybox != null)
        {
            // Restore previous skybox
            RenderSettings.skybox = originalSkybox;

            // Make ambient follow skybox again
            RenderSettings.ambientMode = AmbientMode.Skybox;

            // Force lighting update again
            DynamicGI.UpdateEnvironment();
        }
    }
}
