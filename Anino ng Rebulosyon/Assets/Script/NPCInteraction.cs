using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [HideInInspector] public bool hasTalked = false; // hides from inspector but still public
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // ✅ Mark that this NPC has been talked to
            hasTalked = true;

            // ✅ Optional log for debugging
            Debug.Log($"{gameObject.name} has been talked to.");

            // ⚡ Let your existing dialogue system handle the rest
            // (no need to start any dialogue here)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
