using UnityEngine;

public class TenyenteDialogueTrigger : MonoBehaviour
{
    [Header("Subtitle Settings")]
    public bool showSubtitle = true;

    [TextArea] public string subtitleText;
    public AudioClip voiceClip;
    [Tooltip("Overrides duration if no voice clip is assigned.")]
    public float overrideDuration = 0f;
    public bool playOnce = true;

    [Header("Speaker")]
    public string speakerName;
    public Color speakerNameColor = Color.white; // 👈 NEW (editable in Inspector)

    [Header("Special Events")]
    [Tooltip("Check this if this is the LAST dialogue and should trigger the run tutorial.")]
    public bool triggerRunTutorial = false;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Tenyente"))
        {
            if (showSubtitle && SubtitleManager.Instance != null)
            {
                SubtitleManager.Instance.ShowSubtitle(
                    speakerName,
                    subtitleText,
                    voiceClip,
                    overrideDuration,
                    speakerNameColor // 👈 pass color
                );
            }

            hasPlayed = true;

            if (triggerRunTutorial)
            {
                TutorialManager tutorial = Object.FindFirstObjectByType<TutorialManager>();
                if (tutorial != null)
                {
                    tutorial.TriggerRunTutorial();
                }
            }
        }
    }
}