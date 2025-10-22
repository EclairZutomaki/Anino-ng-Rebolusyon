using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TableInteraction : MonoBehaviour
{
    [Header("Required NPCs to Talk To")]
    public List<NPCInteraction> requiredNPCs = new List<NPCInteraction>();

    [Header("UI")]
    public GameObject dialogueUI;
    public Text dialogueText;

    [Header("Cutscene")]
    public GameObject secondCutscene;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        List<string> notTalkedTo = new List<string>();

        foreach (var npc in requiredNPCs)
        {
            if (npc != null && !npc.hasTalked)
                notTalkedTo.Add(npc.npcName);
        }

        if (notTalkedTo.Count > 0)
        {
            dialogueUI.SetActive(true);
            dialogueText.text = "I need to talk to " + string.Join(", ", notTalkedTo) + " first.";
        }
        else
        {
            dialogueUI.SetActive(false);
            secondCutscene.SetActive(true); // or trigger your timeline
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
