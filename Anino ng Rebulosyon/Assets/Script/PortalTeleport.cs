using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destinationPortal; // The other portal
    public KeyCode teleportKey = KeyCode.E; // Key to trigger teleport

    private bool isPlayerInRange = false;
    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(teleportKey))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        if (player != null && destinationPortal != null)
        {
            // Move player to the destination portal position
            player.position = destinationPortal.position + Vector3.up * 1.5f; // lift a bit to avoid clipping
        }
    }
}