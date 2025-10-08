using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class UniversalTrigger : MonoBehaviour
{
    [Header("Core Settings")]
    public bool requiresInput = true;              // Press E to trigger
    public bool enableTeleport = false;            // Allows teleporting through trigger

    [Header("Fade Settings")]
    public Image fadeImage;                        // Black fade UI image
    [Tooltip("How long the fade in/out lasts.")]
    public float fadeDuration = 1f;

    [Header("Timing Control")]
    [Tooltip("Delay before teleport happens (after fade to black).")]
    public float beforeTeleportDelay = 0.3f;

    [Tooltip("Delay after teleport, before cutscene starts.")]
    public float afterTeleportDelay = 0.5f;

    [Tooltip("Delay after cutscene finishes, before fade out.")]
    public float afterCutsceneDelay = 0.3f;

    [Header("Teleport Settings")]
    public Transform teleportDestination;

    [Header("Cutscene Settings")]
    public bool playCutscene = false;
    public PlayableDirector director;
    public bool autoFadeOutAfterCutscene = true;
    [Tooltip("If enabled, the cutscene will only play the first time. Teleporting still works.")]
    public bool playCutsceneOnce = false; // ✅ NEW FEATURE

    private Transform player;
    private bool isPlayerNearby = false;
    private bool isTransitioning = false;
    private bool hasPlayedCutscene = false; // ✅ Track if cutscene has been played
    private bool hasJustTeleported = false;

    private void Start()
    {
        // Auto-detect player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Ensure fade starts invisible
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if (isTransitioning) return;

        if (isPlayerNearby && !hasJustTeleported)
        {
            if (!requiresInput || Input.GetKeyDown(KeyCode.E))
                StartCoroutine(TriggerSequence());
        }
    }

    private IEnumerator TriggerSequence()
    {
        isTransitioning = true;
        hasJustTeleported = true;

        // Fade to black
        if (fadeImage)
            yield return StartCoroutine(Fade(0, 1));

        // Wait before teleport
        yield return new WaitForSeconds(beforeTeleportDelay);

        // Teleport player
        if (enableTeleport && teleportDestination && player)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc) cc.enabled = true;
        }

        // Wait before cutscene
        yield return new WaitForSeconds(afterTeleportDelay);

        // ✅ Play cutscene (only once if chosen)
        if (playCutscene && director)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;
                director.Play();
                yield return new WaitUntil(() => director.state != PlayState.Playing);
            }
        }

        // Wait after cutscene
        yield return new WaitForSeconds(afterCutsceneDelay);

        // Fade back to normal
        if (fadeImage && (!playCutscene || autoFadeOutAfterCutscene))
            yield return StartCoroutine(Fade(1, 0));

        // ✅ Prevent immediate retrigger spam
        yield return new WaitUntil(() => !isPlayerNearby);
        hasJustTeleported = false;
        isTransitioning = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
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
