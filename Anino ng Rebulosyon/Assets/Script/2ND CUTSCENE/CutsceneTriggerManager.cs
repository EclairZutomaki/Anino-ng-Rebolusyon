using UnityEngine;
using UnityEngine.Playables;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CutsceneTriggerManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectAction
    {
        public GameObject targetObject;
        public bool showAfterInteraction = true; // True = show, False = hide
    }

    [Header("References")]
    public GameObject[] requiredNPCs;       // NPCs that need to be talked to
    public PlayableDirector secondCutscene; // Cutscene timeline
    public GameObject dialoguePanel;        // Dialogue Panel UI
    public TMP_Text dialogueText;           // TMP Text

    [Header("Next Cutscene Trigger")]
    public GameObject cutscene3Trigger;     // The invisible cube (leave unchecked in Inspector)

    [Header("Object Visibility After All NPCs Talked")]
    [Tooltip("Choose which objects to show or hide after all required NPCs are done talking.")]
    public ObjectAction[] objectActions;    // Replaces objectsToToggle

    [Header("Cutscene Settings")]
    [Tooltip("If checked, the 2nd cutscene will only play once.")]
    public bool playOnce = true;

    [Header("Reminder Settings")]
    [TextArea]
    [Tooltip("Message shown when player hasn’t talked to all required NPCs yet.")]
    public string reminderMessage = "I need to talk to them first.";
    public float reminderDuration = 2.5f;

    [Header("Debug")]
    public bool verboseLogs = true;

    private bool hasPlayed = false;
    private bool playerInRange = false;
    private bool actionsTriggered = false;

    private void Start()
    {
        // keep Inspector visibility states as-is
        foreach (var action in objectActions)
        {
            if (action != null && action.targetObject != null)
                action.targetObject.SetActive(action.targetObject.activeSelf);
        }
    }

    private void Update()
    {
        // ✅ automatically hide/show objects once all NPCs talked
        if (!actionsTriggered && AllNPCsTalkedTo())
        {
            ApplyObjectActions();
            actionsTriggered = true;
        }

        // ✅ cutscene trigger only happens on E press
        bool pressedE = false;
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
            pressedE = Keyboard.current.eKey.wasPressedThisFrame;
        else
            pressedE = Input.GetKeyDown(KeyCode.E);
#else
        pressedE = Input.GetKeyDown(KeyCode.E);
#endif

        if (playerInRange && pressedE)
        {
            if (AllNPCsTalkedTo())
            {
                TriggerCutscene();
            }
            else
            {
                ShowReminder(reminderMessage);
            }
        }
    }

    private bool AllNPCsTalkedTo()
    {
        bool all = true;
        foreach (GameObject npc in requiredNPCs)
        {
            if (npc == null)
            {
                if (verboseLogs) Debug.LogWarning("CutsceneTriggerManager: requiredNPCs contains a null entry.");
                all = false;
                continue;
            }

            NPCInteraction npcInteraction = npc.GetComponent<NPCInteraction>();
            if (npcInteraction == null)
            {
                if (verboseLogs) Debug.LogWarning($"CutsceneTriggerManager: '{npc.name}' is missing NPCInteraction component.");
                all = false;
                continue;
            }

            if (!npcInteraction.hasTalked)
            {
                if (verboseLogs) Debug.Log($"CutsceneTriggerManager: '{npc.name}' hasTalked = false");
                all = false;
            }
        }

        if (verboseLogs) Debug.Log($"CutsceneTriggerManager: AllNPCsTalkedTo() => {all}");
        return all;
    }

    private void ApplyObjectActions()
    {
        if (objectActions == null || objectActions.Length == 0)
        {
            if (verboseLogs) Debug.Log("CutsceneTriggerManager: objectActions is empty.");
            return;
        }

        foreach (var action in objectActions)
        {
            if (action == null || action.targetObject == null)
            {
                if (verboseLogs) Debug.LogWarning("CutsceneTriggerManager: Invalid ObjectAction entry.");
                continue;
            }

            action.targetObject.SetActive(action.showAfterInteraction);
            if (verboseLogs) Debug.Log($"CutsceneTriggerManager: set '{action.targetObject.name}' active = {action.showAfterInteraction}");
        }
    }

    private void TriggerCutscene()
    {
        if (playOnce && hasPlayed)
        {
            if (verboseLogs) Debug.Log("CutsceneTriggerManager: Cutscene already played once — skipping.");
            return;
        }

        if (secondCutscene != null)
        {
            secondCutscene.Play();
            hasPlayed = true;
            if (verboseLogs) Debug.Log("CutsceneTriggerManager: Second cutscene triggered!");
        }
        else
        {
            if (verboseLogs) Debug.LogWarning("CutsceneTriggerManager: secondCutscene is not assigned.");
        }

        if (cutscene3Trigger != null)
            cutscene3Trigger.SetActive(true);
    }

    private void ShowReminder(string message)
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = message;
            CancelInvoke(nameof(HideReminder));
            Invoke(nameof(HideReminder), reminderDuration);
        }
        else
        {
            Debug.Log(message);
        }
    }

    private void HideReminder()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
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

    [ContextMenu("Force Apply Object Actions")]
    private void ForceApplyObjectActions()
    {
        ApplyObjectActions();
        actionsTriggered = true;
        Debug.Log("CutsceneTriggerManager: ForceApplyObjectActions called.");
    }
}
