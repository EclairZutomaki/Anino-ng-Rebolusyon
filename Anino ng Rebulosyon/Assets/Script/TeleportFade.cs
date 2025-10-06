using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportFade : MonoBehaviour
{
    [Header("References")]
    public Transform teleportDestination;   // The cube you’ll teleport to
    public Image fadeImage;                 // Black screen Image (UI)
    public float fadeDuration = 1f;
    public float waitBeforeTeleport = 0.3f;

    private Transform player;
    private bool isTouching = false;
    private bool isTransitioning = false;
    private bool hasJustTeleported = false;

    void Start()
    {
        // Auto-find Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Make sure fade starts transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void Update()
    {
        // Can only teleport when touching AND not in cooldown
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

        if (player != null && teleportDestination != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc != null) cc.enabled = true;
        }

        // Fade back to normal
        yield return StartCoroutine(Fade(1, 0));

        isTransitioning = false;

        // Wait until player exits trigger before re-enabling
        StartCoroutine(ResetTeleportPermission());
    }

    IEnumerator ResetTeleportPermission()
    {
        // Wait until player fully leaves trigger before reusing E
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
