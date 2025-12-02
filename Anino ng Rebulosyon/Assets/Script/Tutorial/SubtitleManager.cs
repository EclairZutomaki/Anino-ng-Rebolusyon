using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    [Header("UI References")]
    public Canvas subtitleCanvas;
    public GameObject subtitlePanel;
    public TextMeshProUGUI speakerNameText;   // NEW
    public TextMeshProUGUI subtitleText;
    public AudioSource audioSource;

    [Header("Display Settings")]
    public float fadeDuration = 0.5f; // fade in/out time
    public float defaultSubtitleDuration = 3f;

    private Coroutine activeRoutine;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);
    }

    // --- NEW METHOD WITH NAME ---
    public void ShowSubtitle(string speakerName, string text, AudioClip voiceClip = null, float overrideDuration = 0f)
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(PlaySubtitle(speakerName, text, voiceClip, overrideDuration));
    }

    // --- OLD METHOD STILL WORKS (NO NAME) ---
    public void ShowSubtitle(string text, AudioClip voiceClip = null, float overrideDuration = 0f)
    {
        ShowSubtitle("", text, voiceClip, overrideDuration);
    }

    private IEnumerator PlaySubtitle(string speakerName, string text, AudioClip voiceClip, float overrideDuration)
    {
        if (subtitlePanel == null || subtitleText == null)
            yield break;

        subtitlePanel.SetActive(true);

        // Apply text
        subtitleText.text = text;

        // Apply speaker name (if any)
        if (speakerNameText != null)
            speakerNameText.text = speakerName;

        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(subtitlePanel, 0, 1, fadeDuration));

        // Play audio if provided
        float duration = overrideDuration > 0 ? overrideDuration : defaultSubtitleDuration;

        if (voiceClip)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();
            duration = voiceClip.length;
        }

        yield return new WaitForSeconds(duration);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(subtitlePanel, 1, 0, fadeDuration));
        subtitlePanel.SetActive(false);

        activeRoutine = null;
    }

    private IEnumerator FadeCanvasGroup(GameObject target, float start, float end, float duration)
    {
        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (!cg) cg = target.AddComponent<CanvasGroup>();

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
