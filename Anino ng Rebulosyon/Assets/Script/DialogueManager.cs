using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueUI;
    public TMP_Text nameText;         // 👈 NEW: for displaying who’s talking
    public TMP_Text dialogueText;
    public AudioSource voiceOverSource;

    private DialogueLine[] lines;
    private int currentLine = 0;
    private bool isDialogueActive = false;

    void Update()
    {
        if (isDialogueActive)
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current.leftButton.wasPressedThisFrame)
#else
            if (Input.GetMouseButtonDown(0))
#endif
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        ThirdPersonController.dialogue = true; // lock movement
        isDialogueActive = true;
        lines = dialogueLines;
        currentLine = 0;

        dialogueUI.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine line = lines[currentLine];

        if (nameText != null)
            nameText.text = line.speakerName;

        dialogueText.text = line.lineText;

        if (voiceOverSource && line.voiceClip)
        {
            voiceOverSource.Stop();
            voiceOverSource.clip = line.voiceClip;
            voiceOverSource.Play();
        }
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine < lines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isDialogueActive = false;
        ThirdPersonController.dialogue = false; // unlock movement
    }
}
