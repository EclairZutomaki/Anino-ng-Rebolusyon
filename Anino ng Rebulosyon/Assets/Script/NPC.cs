using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueLine[] dialogueLines; // ✅ Each element has name, text, voice

    private bool playerInRange = false;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }
    }

    void Update()
    {
        if (playerInRange && dialogueManager != null)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.eKey.wasPressedThisFrame)
#else
            if (Input.GetKeyDown(KeyCode.E))
#endif
            {
                dialogueManager.StartDialogue(dialogueLines);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // TODO: Show "Press E to Talk" prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // TODO: Hide prompt
        }
    }
}
