using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueLine[] dialogueLines;

    [Header("Interaction Settings")]
    [Tooltip("If enabled, the player can talk to this NPC repeatedly.")]
    public bool repeatable = false;   // <-- NEW CHECKBOX

    [Header("Trigger After Dialogue")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToHide;
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
                // If NOT repeatable → only run once
                // If repeatable → always run
                if (!dialogueTriggered || repeatable)
                {
                    dialogueTriggered = true;
                    StartCoroutine(StartDialogueAndTrigger());
                }
            }
        }
    }

    private IEnumerator StartDialogueAndTrigger()
    {
        dialogueManager.StartDialogue(dialogueLines);

        yield return new WaitUntil(() => dialogueManager.isDialogueFinished);

        if (activationDelay > 0)
            yield return new WaitForSeconds(activationDelay);

        foreach (var obj in objectsToActivate)
            if (obj) obj.SetActive(true);

        foreach (var obj in objectsToHide)
            if (obj) obj.SetActive(false);

        // If repeatable, allow interaction again
        if (repeatable)
        {
            dialogueTriggered = false;  // <-- reset so player can talk again
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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
