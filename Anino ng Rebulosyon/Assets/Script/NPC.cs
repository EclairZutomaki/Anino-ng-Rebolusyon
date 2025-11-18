using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool repeatable = false;

    [Header("Trigger After Dialogue")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToHide;
    public float activationDelay = 0f;

    [Header("Scene Change (Optional)")]
    [Tooltip("Leave empty if you do NOT want to switch scenes.")]
    public string sceneToLoad = "";   // <-- NEW FEATURE

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
                // Only trigger once unless repeatable is enabled
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

        // Wait until player finishes ALL dialogue lines
        yield return new WaitUntil(() => dialogueManager.isDialogueFinished);

        // Optional delay
        if (activationDelay > 0)
            yield return new WaitForSeconds(activationDelay);

        // Activate GameObjects
        foreach (var obj in objectsToActivate)
            if (obj) obj.SetActive(true);

        // Hide GameObjects
        foreach (var obj in objectsToHide)
            if (obj) obj.SetActive(false);

        // 🔵 NEW: Load another scene (ONLY if assigned)
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
            yield break;
        }

        // Allow replay if repeatable
        if (repeatable)
            dialogueTriggered = false;
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
