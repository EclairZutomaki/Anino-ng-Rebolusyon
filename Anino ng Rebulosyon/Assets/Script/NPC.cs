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
    public string sceneToLoad = "";

    private bool playerInRange = false;
    private DialogueManager dialogueManager;
    private bool dialogueTriggered = false;
    private bool isStartingDialogue = false;

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
        if (!playerInRange || dialogueManager == null || isStartingDialogue)
            return;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
#else
        if (Input.GetKeyDown(KeyCode.E))
#endif
        {
            if (!dialogueTriggered || repeatable)
            {
                StartCoroutine(StartDialogueAndTrigger());
            }
        }
    }

    private IEnumerator StartDialogueAndTrigger()
    {
        isStartingDialogue = true;
        dialogueTriggered = true;

        dialogueManager.StartDialogue(dialogueLines);

        yield return new WaitUntil(() => dialogueManager.isDialogueFinished);

        if (activationDelay > 0)
            yield return new WaitForSeconds(activationDelay);

        foreach (var obj in objectsToActivate)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objectsToHide)
            if (obj != null) obj.SetActive(false);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
            yield break;
        }

        if (repeatable)
            dialogueTriggered = false;

        isStartingDialogue = false;
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