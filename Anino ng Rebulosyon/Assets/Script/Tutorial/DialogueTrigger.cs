using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Speaker")]
    public string speakerName;
    public Color speakerNameColor = Color.white;

    [Header("Subtitle")]
    [TextArea] public string subtitleText;
    public AudioClip voiceClip;
    [Tooltip("Overrides duration if no voice clip is assigned.")]
    public float overrideDuration = 0f;
    public bool playOnce = true;

    [Header("Special Events")]
    public bool triggerRunTutorial = false;

    private bool hasPlayed = false;
    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (hasPlayed && playOnce)
            return;

        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.ShowSubtitle(
                speakerName,
                subtitleText,
                voiceClip,
                overrideDuration,
                speakerNameColor
            );
        }

        hasPlayed = true;

        if (playOnce && triggerCollider != null)
            triggerCollider.enabled = false;

        if (triggerRunTutorial)
        {
            TutorialManager tutorial = Object.FindFirstObjectByType<TutorialManager>();
            if (tutorial != null)
                tutorial.TriggerRunTutorial();
        }
    }
}