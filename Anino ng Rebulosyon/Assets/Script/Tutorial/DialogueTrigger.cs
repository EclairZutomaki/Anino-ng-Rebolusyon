using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea] public string subtitleText;
    public AudioClip voiceClip;
    [Tooltip("Overrides duration if no voice clip is assigned.")]
    public float overrideDuration = 0f;
    public bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            SubtitleManager.Instance.ShowSubtitle(subtitleText, voiceClip, overrideDuration);
            hasPlayed = true;
        }
    }
}
