using UnityEngine;
using UnityEngine.Playables;
using TMPro; // ✅ for TextMeshPro

public class CutsceneTriggerManager : MonoBehaviour
{
    [Header("References")]
    public GameObject[] requiredNPCs;       // NPCs that need to be talked to
    public PlayableDirector secondCutscene; // Cutscene timeline
    public GameObject dialoguePanel;        // Your Dialogue Panel UI
    public TMP_Text dialogueText;           // ✅ TMP Text instead of legacy Text

    [Header("Next Cutscene Trigger")]
    public GameObject cutscene3Trigger;     // The invisible cube (leave unchecked in Inspector)

    [Header("Cutscene Settings")]
    [Tooltip("If checked, the 2nd cutscene will only play once.")]
    public bool playOnce = true;            // ✅ check/uncheck in Inspector

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
                ShowReminder("Kailangan ko muna kausapin sina Kapitan Tiyago, Tenyente, at Padre Damaso.");
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

        // ✅ Activate the cube for 3rd cutscene after the 2nd one starts (or ends)
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
            Invoke(nameof(HideReminder), 2.5f);
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
