using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPCInteraction : MonoBehaviour
{
    [HideInInspector] public bool hasTalked = false;
    private bool playerInRange = false;
    private bool interacting = false;

    // assign these in inspector if you want the NPC to start its own dialogue
    public DialogueLine[] dialogueLines;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
            Debug.LogWarning("DialogueManager not found in scene. NPCInteraction will still set hasTalked if no dialogue is used.");
    }

    private void Update()
    {
        if (playerInRange && !interacting)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.eKey.wasPressedThisFrame)
#else
            if (Input.GetKeyDown(KeyCode.E))
#endif
            {
                // if we have a DialogueManager and assigned lines, start the dialogue and wait for it to finish
                if (dialogueManager != null && dialogueLines != null && dialogueLines.Length > 0)
                {
                    StartCoroutine(StartDialogueAndMark());
                }
                else
                {
                    // fallback: immediately mark as talked (keeps previous behavior)
                    hasTalked = true;
                    Debug.Log($"{gameObject.name} marked hasTalked (no DialogueManager/lines).");
                }
            }
        }
    }

    private IEnumerator StartDialogueAndMark()
    {
        interacting = true;
        dialogueManager.StartDialogue(dialogueLines);

        // wait until DialogueManager reports finished
        yield return new WaitUntil(() => dialogueManager.isDialogueFinished);

        hasTalked = true;
        interacting = false;
        Debug.Log($"{gameObject.name} dialogue finished -> hasTalked = true");
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
