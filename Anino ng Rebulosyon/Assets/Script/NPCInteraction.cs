using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName;  // e.g. "Padre Damaso"
    [HideInInspector] public bool hasTalked = false;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (!hasTalked)
        {
            hasTalked = true;
            Debug.Log($"Talked to {npcName}");
            // TODO: Trigger your dialogue system here
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
