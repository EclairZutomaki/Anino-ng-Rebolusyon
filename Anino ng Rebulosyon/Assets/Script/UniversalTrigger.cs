using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class UniversalTrigger : MonoBehaviour
{
    [Header("Core Settings")]
    public bool requiresInput = true;
    public bool enableTeleport = false;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Timing Control")]
    public float beforeTeleportDelay = 0.3f;
    public float afterTeleportDelay = 0.5f;
    public float afterCutsceneDelay = 0.3f;

    [Header("Teleport Settings")]
    public Transform teleportDestination;

    [Header("Cutscene Settings")]
    public bool playCutscene = false;
    public PlayableDirector director;
    public bool autoFadeOutAfterCutscene = true;
    public bool playCutsceneOnce = false;

    private Transform player;
    private bool isPlayerNearby = false;
    private bool isTransitioning = false;
    private bool hasPlayedCutscene = false;
    private bool hasJustTeleported = false;

    private void Start()
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

        if (fadeImage)
            yield return StartCoroutine(Fade(0, 1));

        yield return new WaitForSeconds(beforeTeleportDelay);

        // ✅ FORCE teleport to exact XYZ of the destination
        if (enableTeleport && teleportDestination && player)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            // Set exact position and rotation (no offset)
            player.SetPositionAndRotation(
                teleportDestination.position,
                teleportDestination.rotation
            );

            yield return null; // Give physics one frame to update
            if (cc) cc.enabled = true;
        }

        yield return new WaitForSeconds(afterTeleportDelay);

        if (playCutscene && director)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;
                director.Play();
                yield return new WaitUntil(() => director.state != PlayState.Playing);
            }
        }

        yield return new WaitForSeconds(afterCutsceneDelay);

        if (fadeImage && (!playCutscene || autoFadeOutAfterCutscene))
            yield return StartCoroutine(Fade(1, 0));

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
