using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Speaker")]
    public string speakerName;    // NEW NAME FIELD

    [Header("Subtitle")]
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
            // USES THE NEW SHOWSUBTITLE WITH NAME
            SubtitleManager.Instance.ShowSubtitle(speakerName, subtitleText, voiceClip, overrideDuration);

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
