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
    public bool playCutscene = false;
    public PlayableDirector timelineDirector;
    public bool playCutsceneOnce = false;
    private bool hasPlayedCutscene = false;

    private GameObject playerObj;
    private Transform player;
    private bool isTouching = false;
    private bool isTransitioning = false;
    private bool hasJustTeleported = false;

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Start transparent
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

        // 1️⃣ Fade to Black
        yield return StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(waitBeforeTeleport);

        // 2️⃣ Teleport while black
        if (player != null && teleportDestination != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc) cc.enabled = true;
        }

        // 3️⃣ Handle Cutscene or Normal
        if (playCutscene && timelineDirector != null)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;

                // Disable player right before fade-in
                if (playerObj) playerObj.SetActive(false);

                // Keep screen black while teleport settles
                yield return new WaitForSeconds(0.3f);

                // ✨ Start cutscene THEN fade in
                timelineDirector.Play();
                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(Fade(1, 0));

                // Wait for cutscene to finish
                yield return new WaitUntil(() => timelineDirector.state != PlayState.Playing);

                // 4️⃣ Fade out at end of cutscene -> re-enable player
                yield return StartCoroutine(Fade(0, 1));
                if (playerObj) playerObj.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(Fade(1, 0));
            }
            else
            {
                // Cutscene already played, just fade out
                yield return StartCoroutine(Fade(1, 0));
            }
        }
        else
        {
            // 4️⃣ No cutscene — fade back out normally
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(Fade(1, 0));
        }

        // Reset transition flags
        isTransitioning = false;
        StartCoroutine(ResetTeleportPermission());
    }

    IEnumerator ResetTeleportPermission()
    {
        yield return new WaitUntil(() => !isTouching);
        hasJustTeleported = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        startAlpha = Mathf.Clamp01(startAlpha);
        endAlpha = Mathf.Clamp01(endAlpha);

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
