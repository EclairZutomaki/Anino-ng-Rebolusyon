using UnityEngine;

public class StopSubtitleOnPlayerDisable : MonoBehaviour
{
    private void OnDisable()
    {
        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.StopSubtitleImmediate();
        }
    }
}