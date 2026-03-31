using UnityEngine;

public class InteractionCounter : MonoBehaviour
{
    public UICounter uiCounter;

    private bool playerInside = false;
    private bool hasInteracted = false;

    void Update()
    {
        if (playerInside && !hasInteracted && Input.GetKeyDown(KeyCode.E))
        {
            uiCounter.AddCount();
            hasInteracted = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}