using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameOverMiniGame : MonoBehaviour
{
    [Header("Game Over UI Parent")]
    public GameObject gameOverUI;

    private List<Image> images = new List<Image>();
    private List<TMP_Text> texts = new List<TMP_Text>();

    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float holdTime = 3f;

    [Header("Image Alpha Limit (0–255)")]
    public float maxImageAlpha = 200f; // Your limit

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip fadeInSFX;
    public AudioClip fadeOutSFX;

    [Header("Slow Motion")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;

    [Header("Object Actions")]
    public ToggleActionData[] objectActions;

    [Header("Trigger Settings")]
    public string playerTag = "Player";

    private bool isRunning = false;
    private bool playerInside = false;

    [System.Serializable]
    public class ToggleActionData
    {
        public GameObject targetObject;
        public bool show = true;
        public float delay = 0f;
    }

    void Awake()
    {
        // Separate images and TMP texts
        images.AddRange(gameOverUI.GetComponentsInChildren<Image>(true));
        texts.AddRange(gameOverUI.GetComponentsInChildren<TMP_Text>(true));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = true;

        if (!isRunning)
            StartCoroutine(GameOverSequence());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            playerInside = false;
    }

    IEnumerator GameOverSequence()
    {
        isRunning = true;

        StartCoroutine(DoSlowMotion());

        yield return StartCoroutine(FadeAll(0f, 1f, fadeInDuration));

        yield return new WaitForSecondsRealtime(holdTime);

        yield return StartCoroutine(FadeAll(1f, 0f, fadeOutDuration));

        foreach (var obj in objectActions)
        {
            yield return new WaitForSecondsRealtime(obj.delay);
            if (obj.targetObject != null)
                obj.targetObject.SetActive(obj.show);
        }

        while (playerInside)
            yield return null;

        isRunning = false;
    }

    IEnumerator FadeAll(float start, float end, float duration)
    {
        gameOverUI.SetActive(true);

        if (audioSource != null)
        {
            if (end == 1 && fadeInSFX != null) audioSource.PlayOneShot(fadeInSFX);
            if (end == 0 && fadeOutSFX != null) audioSource.PlayOneShot(fadeOutSFX);
        }

        float t = 0f;
        float imgMax = maxImageAlpha / 255f; // Convert to 0–1

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Lerp(start, end, t / duration);

            // Fade images (0 → imgMax)
            foreach (var img in images)
            {
                if (img == null) continue;
                Color c = img.color;
                float targetAlpha = (end == 1) ? imgMax : lerp;
                img.color = new Color(c.r, c.g, c.b, targetAlpha);
            }

            // Fade texts full alpha (0 → 1)
            foreach (var txt in texts)
            {
                if (txt == null) continue;
                Color c = txt.color;
                txt.color = new Color(c.r, c.g, c.b, lerp);
            }

            yield return null;
        }

        if (end == 0)
            gameOverUI.SetActive(false);
    }

    IEnumerator DoSlowMotion()
    {
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * slowMoScale;

        yield return new WaitForSecondsRealtime(slowMoDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
