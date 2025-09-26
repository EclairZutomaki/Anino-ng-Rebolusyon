using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI; // Panel for dialogue
    public TMP_Text dialogueText; // TextMeshPro for dialogue
    public AudioSource voiceOverSource; // AudioSource for voice playback

    private string[] lines; // Lines of dialogue
    private AudioClip[] voiceClips; // Matching voice lines
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

    public void StartDialogue(string[] dialogueLines, AudioClip[] audioClips)
    {
        ThirdPersonController.dialogue = true; // 🛑 Freeze movement
        isDialogueActive = true;
        lines = dialogueLines;
        voiceClips = audioClips;
        currentLine = 0;

        dialogueUI.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        dialogueText.text = lines[currentLine];
        if (voiceOverSource && voiceClips[currentLine])
        {
            voiceOverSource.Stop();
            voiceOverSource.clip = voiceClips[currentLine];
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
        ThirdPersonController.dialogue = false; // ✅ Unfreeze movement
    }
}
