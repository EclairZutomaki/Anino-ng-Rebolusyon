using UnityEngine;
using UnityEngine.Playables;
using TMPro; // ✅ For TextMeshPro

public class CutsceneTriggerManager : MonoBehaviour
{
    [Header("References")]
    public GameObject[] requiredNPCs;       // NPCs that need to be talked to
    public PlayableDirector secondCutscene; // Cutscene timeline
    public GameObject dialoguePanel;        // Dialogue Panel UI
    public TMP_Text dialogueText;           // ✅ TMP Text

    [Header("Next Cutscene Trigger")]
    public GameObject cutscene3Trigger;     // The invisible cube (leave unchecked in Inspector)

    [Header("Cutscene Settings")]
    [Tooltip("If checked, the 2nd cutscene will only play once.")]
    public bool playOnce = true;

    [Header("Reminder Settings")]
    [TextArea]
    [Tooltip("Message shown when player hasn’t talked to all required NPCs yet.")]
    public string reminderMessage = "I need to talk to them first."; // ✅ Editable in Inspector
    public float reminderDuration = 2.5f; // How long the message stays on screen

    private bool hasPlayed = false;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
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
        foreach (GameObject npc in requiredNPCs)
        {
            NPCInteraction npcInteraction = npc.GetComponent<NPCInteraction>();
            if (npcInteraction == null || !npcInteraction.hasTalked)
                return false;
        }
        return true;
    }

    private void TriggerCutscene()
    {
        if (playOnce && hasPlayed)
        {
            Debug.Log("Cutscene already played once — skipping.");
            return;
        }

        secondCutscene.Play();
        hasPlayed = true;
        Debug.Log("Second cutscene triggered!");

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
}
