using UnityEngine;

public class PaperResetManager : MonoBehaviour
{
    [Header("Paper IDs (Optional)")]
    public string[] paperIDs; // optional kung gusto mo manual

    [ContextMenu("RESET ALL PAPER DATA")]
    public void ResetAllPaperData()
    {
        // Reset total count
        PlayerPrefs.DeleteKey("CollectedPaperCount");

        // Option 1: reset via list (recommended)
        foreach (string id in paperIDs)
        {
            PlayerPrefs.DeleteKey(id);
        }

        // Option 2: brute force (if you used Paper_01 format)
        for (int i = 1; i <= 100; i++)
        {
            PlayerPrefs.DeleteKey("Paper_" + i.ToString("00"));
        }

        PlayerPrefs.Save();

        Debug.Log("✅ ALL PAPER DATA RESET!");
    }
}