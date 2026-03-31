using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class TeleportFade : MonoBehaviour
{
    [Header("References")]
    public Transform player; // 👈 DRAG PLAYER HERE
    public Transform teleportDestination;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float waitBeforeTeleport = 0.3f;

    [Header("Cutscene Settings")]
    public bool playCutscene = false;
    public PlayableDirector timelineDirector;
    public bool playCutsceneOnce = false;
    private bool hasPlayedCutscene = false;

    private bool isTouching = false;
    private bool isTransitioning = false;
    private bool hasJustTeleported = false;

    void Start()
    {
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

        // 🔒 Freeze player
        StarterAssets.ThirdPersonController.dialogue = true;

        // 🧼 CLEAR INPUT (FIXES STUCK WALKING)
        var input = player.GetComponent<StarterAssets.StarterAssetsInputs>();
        if (input != null)
        {
            input.move = Vector2.zero;
            input.look = Vector2.zero;
            input.jump = false;
            input.sprint = false;
        }

        // 🧼 RESET ANIMATION (FIXES WALK ANIMATION STUCK)
        Animator anim = player.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
        }

        // 1️⃣ Fade to black
        yield return StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(waitBeforeTeleport);

        // 2️⃣ Teleport
        if (player != null && teleportDestination != null)
        {
            CharacterController cc = player.GetComponentInChildren<CharacterController>();
            if (cc) cc.enabled = false;

            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;

            if (cc) cc.enabled = true;
        }

        // 3️⃣ Cutscene or normal
        if (playCutscene && timelineDirector != null)
        {
            if (!playCutsceneOnce || (playCutsceneOnce && !hasPlayedCutscene))
            {
                hasPlayedCutscene = true;

                yield return new WaitForSeconds(0.3f);

                timelineDirector.Play();

                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(Fade(1, 0));

                yield return new WaitUntil(() => timelineDirector.state != PlayState.Playing);

                yield return StartCoroutine(Fade(0, 1));
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(Fade(1, 0));
            }
            else
            {
                yield return StartCoroutine(Fade(1, 0));
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(Fade(1, 0));
        }

        // 🔓 Unfreeze player
        StarterAssets.ThirdPersonController.dialogue = false;

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