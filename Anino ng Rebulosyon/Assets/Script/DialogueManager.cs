using UnityEngine;
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

    private void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    private void Update()
    {
        if (!isDialogueActive) return;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
#else
        if (Input.GetMouseButtonDown(0))
#endif
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueManager: dialogueUI is not assigned.");
            return;
        }

        if (dialogueText == null)
        {
            Debug.LogError("DialogueManager: dialogueText is not assigned.");
            return;
        }

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

        if (nameText != null)
        {
            nameText.text = line.speakerName;
            nameText.color = line.speakerNameColor;
            nameText.gameObject.SetActive(!string.IsNullOrEmpty(line.speakerName));
        }

        dialogueText.text = line.lineText;

        if (voiceOverSource != null)
        {
            voiceOverSource.Stop();
            voiceOverSource.clip = null;

            if (line.voiceClip != null)
            {
                voiceOverSource.clip = line.voiceClip;
                voiceOverSource.Play();
            }
        }
    }

    private void NextLine()
    {
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
        if (voiceOverSource != null)
        {
            voiceOverSource.Stop();
            voiceOverSource.clip = null;
        }

        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        isDialogueActive = false;
        isDialogueFinished = true;

        ThirdPersonController.dialogue = false;
    }
}