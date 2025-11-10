using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueLine[] dialogueLines; // Each element has name, text, voice

    [Header("Trigger After Dialogue")]
    [Tooltip("GameObjects that will be activated after the dialogue finishes.")]
    public GameObject[] objectsToActivate;

    [Tooltip("GameObjects that will be hidden after the dialogue finishes.")]
    public GameObject[] objectsToHide;

    [Tooltip("Delay (in seconds) before applying changes after the dialogue ends.")]
    public float activationDelay = 0f;

    private bool playerInRange = false;
    private DialogueManager dialogueManager;
    private bool dialogueTriggered = false;

    private void Start()
    {
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }
    }

    private void Update()
    {
        if (playerInRange && dialogueManager != null)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.eKey.wasPressedThisFrame)
#else
            if (Input.GetKeyDown(KeyCode.E))
#endif
            {
                // Only start once
                if (!dialogueTriggered)
                {
                    dialogueTriggered = true;
                    StartCoroutine(StartDialogueAndTrigger());
                }
            }
        }
    }

    private IEnumerator StartDialogueAndTrigger()
    {
        // Start dialogue
        dialogueManager.StartDialogue(dialogueLines);

        // Wait until dialogue finishes
        yield return new WaitUntil(() => dialogueManager.isDialogueFinished);

        // Optional delay
        if (activationDelay > 0)
            yield return new WaitForSeconds(activationDelay);

        // Show objects
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Hide objects
        foreach (var obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Optional: show "Press E to talk" UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
