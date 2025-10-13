using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class TeleportFade : MonoBehaviour
{
    [Header("References")]
    public Transform teleportDestination;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float waitBeforeTeleport = 0.3f;

    [Header("Cutscene Settings")]
    [Tooltip("If enabled, the Timeline will play after teleport.")]
    public bool playCutscene = false;
    public PlayableDirector timelineDirector;
    [Tooltip("If true, cutscene plays only the first time this trigger is used.")]
    public bool playCutsceneOnce = false;
    private bool hasPlayedCutscene = false;

    private Transform player;
    private bool isTouching = false;
    private bool isTransitioning = false;
    private bool hasJustTeleported = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void Update()
    {
        if (isTouching && !isTransitioning && !hasJustTeleported && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        isTransitioning = true;
        hasJustTeleported = true;

        // Fade to black
        yield return StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(waitBeforeTeleport);

        // --- TELEPORT ---
        if (player != null && teleportDestination != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc != null) cc.enabled = true;
        }

        // --- CUTSCENE ---
        if (playCutscene && timelineDirector != null)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;

                // ✅ Fade back in BEFORE the cutscene plays (so you can see it)
                yield return StartCoroutine(Fade(1, 0));

                timelineDirector.Play();

                // Wait until timeline finishes
                yield return new WaitUntil(() => timelineDirector.state != PlayState.Playing);

                // Optional: fade out again after cutscene if you want smooth transition
                yield return StartCoroutine(Fade(0, 1));
                yield return StartCoroutine(Fade(1, 0));
            }
        }
        else
        {
            // If no cutscene, just fade back to normal
            yield return StartCoroutine(Fade(1, 0));
        }

        isTransitioning = false;

        // Wait until player exits trigger before re-enabling teleport
        StartCoroutine(ResetTeleportPermission());
    }

    IEnumerator ResetTeleportPermission()
    {
        yield return new WaitUntil(() => !isTouching);
        hasJustTeleported = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
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
            isTouching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isTouching = false;
    }
}
