using UnityEngine;

public class TenyenteDialogueTrigger : MonoBehaviour
{
    [TextArea] public string subtitleText;
    public AudioClip voiceClip;
    [Tooltip("Overrides duration if no voice clip is assigned.")]
    public float overrideDuration = 0f;
    public bool playOnce = true;

    [Header("Special Events")]
    [Tooltip("Check this if this is the LAST dialogue and should trigger the run tutorial.")]
    public bool triggerRunTutorial = false;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        // ✅ Player triggers the dialogue when entering Tenyente's collider
        if (other.CompareTag("Tenyente"))
        {
            if (SubtitleManager.Instance != null)
                SubtitleManager.Instance.ShowSubtitle(subtitleText, voiceClip, overrideDuration);

            hasPlayed = true;

            // Optional: trigger run tutorial
            if (triggerRunTutorial)
            {
                TutorialManager tutorial = Object.FindFirstObjectByType<TutorialManager>();
                if (tutorial != null)
                    tutorial.TriggerRunTutorial();
            }
        }
    }
}
