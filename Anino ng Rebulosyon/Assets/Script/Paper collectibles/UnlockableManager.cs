using UnityEngine;

public class UnlockableManager : MonoBehaviour
{
    [Header("Required Paper")]
    public string requiredPaperID;

    [Header("Objects To Show When Unlocked")]
    public GameObject[] objectsToShow;

    [Header("Objects To Hide When Unlocked")]
    public GameObject[] objectsToHide;

    void Start()
    {
        ApplyState();
    }

    void Update()
    {
        // Optional: real-time update
        // Remove this if gusto mo once lang mag-check
        ApplyState();
    }

    void ApplyState()
    {
        bool unlocked = PlayerPrefs.GetInt(requiredPaperID, 0) == 1;

        // SHOW objects
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(unlocked);
        }

        // HIDE objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(!unlocked);
        }
    }
}