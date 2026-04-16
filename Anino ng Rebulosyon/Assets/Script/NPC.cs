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
    public bool repeatable = false;

    [Header("Trigger After Dialogue")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToHide;
    public float activationDelay = 0f;

    [Header("Scene Change (Optional)")]
    [Tooltip("Leave empty if you do NOT want to switch scenes.")]
    public string sceneToLoad = "";
    [Tooltip("Optional loading screen background for this NPC scene change.")]
    public Sprite loadingBackground;

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

        if (!string.IsNullOrEmpty(sceneToLoad) && SceneLoadingUI.Instance == null)
        {
            Debug.LogWarning("SceneLoadingUI not found in the scene! Scene changes will not show a loading screen.");
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

        if (activationDelay > 0f)
            yield return new WaitForSeconds(activationDelay);

        if (objectsToActivate != null)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        if (objectsToHide != null)
        {
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            if (SceneLoadingUI.Instance != null)
            {
                SceneLoadingUI.Instance.LoadScene(sceneToLoad, loadingBackground);
            }
            else
            {
                Debug.LogWarning("SceneLoadingUI missing. Loading scene without loading screen.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            }

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