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
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public AudioSource voiceOverSource;

    private DialogueLine[] lines;
    private int currentLine = 0;
    private bool isDialogueActive = false;

    [HideInInspector]
    public bool isDialogueFinished = false;

    void Update()
    {
        if (!isDialogueActive) return;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current.leftButton.wasPressedThisFrame)
#else
        if (Input.GetMouseButtonDown(0))
#endif
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        // lock yung movement
        ThirdPersonController.dialogue = true;

        isDialogueActive = true;
        isDialogueFinished = false;

        lines = dialogueLines;
        currentLine = 0;

        dialogueUI.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLine >= lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentLine];

        // text
        if (nameText != null)
            nameText.text = line.speakerName;

        dialogueText.text = line.lineText;

        // 🔊 audio — always hard stop before new play
        if (voiceOverSource != null)
        {
            voiceOverSource.Stop();

            if (line.voiceClip != null)
            {
                voiceOverSource.clip = line.voiceClip;
                voiceOverSource.Play();
            }
        }
    }

    private void NextLine()
    {
        // cut current audio when skipping
        if (voiceOverSource != null && voiceOverSource.isPlaying)
            voiceOverSource.Stop();

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
        // ensure audio is stopped on exit
        if (voiceOverSource != null && voiceOverSource.isPlaying)
            voiceOverSource.Stop();

        dialogueUI.SetActive(false);
        isDialogueActive = false;
        isDialogueFinished = true;

        // unlock movement
        ThirdPersonController.dialogue = false;
    }
}
