using UnityEngine;

public class UnlockableObject : MonoBehaviour
{
    public string requiredPaperID;

    [Header("Objects To Show When Unlocked")]
    public GameObject[] objectsToShow;

    [Header("Objects To Hide When Unlocked")]
    public GameObject[] objectsToHide;

    void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        bool unlocked = PlayerPrefs.GetInt(requiredPaperID, 0) == 1;

        // Show objects
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(unlocked);
        }

        // Hide objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(!unlocked);
        }
    }
}