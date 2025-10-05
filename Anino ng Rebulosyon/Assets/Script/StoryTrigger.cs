using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class StoryTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Tag of the object that activates the trigger.")]
    public string playerTag = "Player";

    [Tooltip("If true, trigger activates only once.")]
    public bool playOnce = true;

    [Header("Dialogue / Voice Settings")]
    [TextArea(2, 4)]
    public string subtitleText;

    [Tooltip("Optional voice line to play.")]
    public AudioClip voiceLine;

    [Tooltip("If true, subtitle duration will match voice line length.")]
    public bool autoUseVoiceLength = true;

    [Tooltip("If not auto, how long subtitle will stay (in seconds).")]
    [Range(0.5f, 10f)] public float subtitleDuration = 3f;

    [Header("Future Expansion Hooks")]
    public UnityEvent onTriggerActivated;
    // 🧩 Use this for adding NPC dialogue, cutscenes, animations, etc.

    private bool hasTriggered = false;
    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"{name}: Collider was not set as trigger, fixed automatically.");
        }

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (hasTriggered && playOnce) return;

        hasTriggered = true;

        // 🔊 Play voice line if assigned
        if (voiceLine != null)
        {
            audioSource.clip = voiceLine;
            audioSource.Play();
        }

        // 💬 Show subtitle (manual or auto duration)
        float duration = autoUseVoiceLength && voiceLine != null ? voiceLine.length : subtitleDuration;
        if (!string.IsNullOrEmpty(subtitleText))
        {
            SubtitleManagerTMP.ShowSubtitle(subtitleText, duration);
        }

        // ⚙️ Invoke future actions
        onTriggerActivated?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.3f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
    }
#endif
}
