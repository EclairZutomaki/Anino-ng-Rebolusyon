using UnityEngine;

public class DialogueTrigger : MonoBehaviour
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

        if (other.CompareTag("Player"))
        {
            SubtitleManager.Instance.ShowSubtitle(subtitleText, voiceClip, overrideDuration);
            hasPlayed = true;

            // Only trigger the run tutorial on the last subtitle
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
