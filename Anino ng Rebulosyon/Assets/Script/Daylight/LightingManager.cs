using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    [Header("Time Settings")]
    [SerializeField, Range(0, 24)] private float timeOfDay = 12f;
    [SerializeField] private float dayDurationInMinutes = 5f; // FULL day = 5 mins
    [SerializeField] private bool autoTime = true;

    private float timeRate;

    private void Update()
    {
        if (preset == null)
            return;

        // Convert real time to game time
        if (Application.isPlaying && autoTime)
        {
            timeRate = 24f / (dayDurationInMinutes * 60f);
            timeOfDay += Time.deltaTime * timeRate;
            timeOfDay %= 24f;

            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // ?? Ambient + Fog
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            // ?? Light color
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);

            // ?? Smooth realistic sun rotation
            float sunAngle = timePercent * 360f;

            directionalLight.transform.rotation = Quaternion.Euler(
                new Vector3(sunAngle - 90f, 170f, 0)
            );

            // ?? Optional: disable light at night
            directionalLight.intensity = Mathf.Clamp01(Mathf.Cos(timePercent * Mathf.PI * 2f) * 1.2f);
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsByType<Light>(FindObjectsSortMode.None);

            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    // ?? OPTIONAL: Control from other scripts
    public void SetTime(float hour)
    {
        timeOfDay = hour;
    }

    public float GetTime()
    {
        return timeOfDay;
    }
}