using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public string paperID; // Unique ID that matches the DocumentManager

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange)
        {
#if ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
#else
            if (Input.GetKeyDown(KeyCode.E))
#endif
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        if (!string.IsNullOrEmpty(paperID) && DocumentManager.Instance != null)
        {
            DocumentManager.Instance.CollectPaper(paperID);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}
