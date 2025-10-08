using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Universal cutscene trigger system.
/// - Only plays when player is near and presses "E"
/// - Supports teleportation before cutscene (optional)
/// - Automatically fades screen in/out
/// - Works with any Timeline (PlayableDirector)
/// </summary>
public class CutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public PlayableDirector director;            // Assign the Timeline (PlayableDirector)
    public bool playOnlyOnce = true;             // Prevent replays after first trigger
    public bool autoFadeOutAfterCutscene = true; // Fades back to clear when cutscene ends

    [Header("Fade Settings")]
    public Image fadeImage;                      // UI black panel for fade
    public float fadeDuration = 1f;

    [Header("Teleport Settings (Optional)")]
    public bool teleportPlayer = false;          // Toggle teleport on/off
    public Transform player;                     // Player transform
    public Transform teleportDestination;        // Where to move the player before the cutscene

    private bool isPlayerNearby = false;
    private bool hasPlayed = false;
    private bool isTransitioning = false;

    private void Start()
    {
        // Make sure fade starts clear
        if (fadeImage)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            if (!playOnlyOnce || !hasPlayed)
                StartCoroutine(PlayCutsceneSequence());
        }
    }

    private IEnumerator PlayCutsceneSequence()
    {
        isTransitioning = true;
        hasPlayed = true;

        // Fade to black
        if (fadeImage) yield return StartCoroutine(Fade(0, 1));

        // Optional: teleport player before cutscene
        if (teleportPlayer && player && teleportDestination)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;
            if (cc != null) cc.enabled = true;
        }

        // Play Timeline
        if (director)
        {
            director.Play();
            yield return new WaitUntil(() => director.state != PlayState.Playing);
        }

        // Fade back to clear after cutscene
        if (autoFadeOutAfterCutscene && fadeImage)
            yield return StartCoroutine(Fade(1, 0));

        isTransitioning = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }
}
