using UnityEngine;
using UnityEngine.InputSystem; // only if using the new Input System

public class InteractionUI : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag your '[E] Interact' UI panel or image here.")]
    public GameObject interactUIPanel;

    [Header("Interaction Settings")]
    [Tooltip("The key used to interact (default = E).")]
    public Key interactKey = Key.E;

    [Tooltip("Destroy the object after interacting (for pickups, etc).")]
    public bool destroyAfterInteract = false;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactUIPanel != null)
            interactUIPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactUIPanel != null)
                interactUIPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactUIPanel != null)
                interactUIPanel.SetActive(false);
        }
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (playerInRange && Keyboard.current != null && Keyboard.current[interactKey].wasPressedThisFrame)
        {
            Interact();
        }
#else
        // fallback for old input system
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
#endif
    }

    private void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        // Example action: pick up or talk
        if (destroyAfterInteract)
            Destroy(gameObject);

        if (interactUIPanel != null)
            interactUIPanel.SetActive(false);
    }
}
