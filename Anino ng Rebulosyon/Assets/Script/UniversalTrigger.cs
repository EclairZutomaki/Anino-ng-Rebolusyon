using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class UniversalTrigger : MonoBehaviour
{
    [Header("Core Settings")]
    public bool requiresInput = true;              // Press E to trigger
    public bool playOnlyOnce = false;              // Optional one-time trigger

    [Header("Fade Settings")]
    public Image fadeImage;                        // Black fade UI image
    public float fadeDuration = 1f;
    public float waitBeforeTeleport = 0.3f;

    [Header("Teleport Settings")]
    public bool enableTeleport = false;            // Optional teleport
    public Transform teleportDestination;          // Where player goes

    [Header("Cutscene Settings")]
    public bool playCutscene = false;              // Optional Timeline
    public PlayableDirector director;              // The cutscene timeline
    public bool autoFadeOutAfterCutscene = true;

    private Transform player;
    private bool isPlayerNearby = false;
    private bool hasTriggered = false;
    private bool isTransitioning = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if (isTransitioning || hasTriggered && playOnlyOnce) return;

        if (isPlayerNearby)
        {
            if (!requiresInput || Input.GetKeyDown(KeyCode.E))
                StartCoroutine(TriggerSequence());
        }
    }

    private IEnumerator TriggerSequence()
    {
        isTransitioning = true;
        hasTriggered = true;

        // Fade to black
        if (fadeImage) yield return StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(waitBeforeTeleport);

        // Teleport (if enabled)
        if (enableTeleport && teleportDestination && player)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc) cc.enabled = true;
        }

        // Play cutscene (if available)
        if (playCutscene && director)
        {
            director.Play();
            yield return new WaitUntil(() => director.state != PlayState.Playing);
        }

        // Fade back in
        if (fadeImage && (!playCutscene || autoFadeOutAfterCutscene))
            yield return StartCoroutine(Fade(1, 0));

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
