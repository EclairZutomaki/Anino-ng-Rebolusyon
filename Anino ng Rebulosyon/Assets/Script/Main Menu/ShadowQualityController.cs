using UnityEngine;

public class ShadowQualityController : MonoBehaviour
{
    public Light directionalLight;

    public void SetMababa()
    {
        directionalLight.shadows = LightShadows.None;
        Debug.Log("Shadow: OFF");
    }

    public void SetKatamtaman()
    {
        directionalLight.shadows = LightShadows.Hard;
        Debug.Log("Shadow: HARD");
    }

    public void SetMataas()
    {
        directionalLight.shadows = LightShadows.Soft;
        Debug.Log("Shadow: SOFT");
    }
}