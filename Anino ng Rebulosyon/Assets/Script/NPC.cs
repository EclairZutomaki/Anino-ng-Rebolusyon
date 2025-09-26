using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPC : MonoBehaviour
{
    public string[] dialogueLines;
    public AudioClip[] voiceLines;

    private bool playerInRange = false;

    // ✅ Cache DialogueManager reference for performance
    private DialogueManager dialogueManager;

    private void Start()
    {
        // Unity 6+ API: FindFirstObjectByType
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
                dialogueManager.StartDialogue(dialogueLines, voiceLines);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // TODO: Show "Press E to Talk" UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // TODO: Hide "Press E to Talk" UI
        }
    }
}
