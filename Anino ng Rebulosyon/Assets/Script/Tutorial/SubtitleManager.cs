using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    [Header("UI References")]
    public Canvas subtitleCanvas;
    public GameObject subtitlePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI subtitleText;
    public AudioSource audioSource;

    [Header("Display Settings")]
    public float fadeDuration = 0.5f;
    public float defaultSubtitleDuration = 3f;

    private Coroutine activeRoutine;
    private AudioClip currentClip;

    private void Awake()
    {
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

    public void ShowSubtitle(string speakerName, string text, AudioClip voiceClip, float overrideDuration, Color nameColor)
    {
        // Prevent same clip from replaying while already playing
        if (voiceClip != null && voiceClip == currentClip && audioSource != null && audioSource.isPlaying)
            return;

        StopSubtitleImmediate();

        activeRoutine = StartCoroutine(PlaySubtitle(speakerName, text, voiceClip, overrideDuration, nameColor));
    }

    public void ShowSubtitle(string speakerName, string text, AudioClip voiceClip = null, float overrideDuration = 0f)
    {
        ShowSubtitle(speakerName, text, voiceClip, overrideDuration, Color.white);
    }

    public void ShowSubtitle(string text, AudioClip voiceClip = null, float overrideDuration = 0f)
    {
        ShowSubtitle("", text, voiceClip, overrideDuration, Color.white);
    }

    public void StopSubtitleImmediate()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        currentClip = null;

        if (subtitlePanel != null)
        {
            CanvasGroup cg = subtitlePanel.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = 0f;

            subtitlePanel.SetActive(false);
        }

        if (subtitleText != null)
            subtitleText.text = "";

        if (speakerNameText != null)
            speakerNameText.text = "";
    }

    private IEnumerator PlaySubtitle(string speakerName, string text, AudioClip voiceClip, float overrideDuration, Color nameColor)
    {
        if (subtitlePanel == null || subtitleText == null)
            yield break;

        subtitlePanel.SetActive(true);

        subtitleText.text = text;

        if (speakerNameText != null)
        {
            speakerNameText.text = speakerName;
            speakerNameText.color = nameColor;
        }

        yield return StartCoroutine(FadeCanvasGroup(subtitlePanel, 0f, 1f, fadeDuration));

        float duration = overrideDuration > 0 ? overrideDuration : defaultSubtitleDuration;

        if (voiceClip != null && audioSource != null)
        {
            currentClip = voiceClip;
            audioSource.Stop();
            audioSource.clip = voiceClip;
            audioSource.Play();
            duration = voiceClip.length;
        }

        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadeCanvasGroup(subtitlePanel, 1f, 0f, fadeDuration));

        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);

        currentClip = null;
        activeRoutine = null;
    }

    private IEnumerator FadeCanvasGroup(GameObject target, float start, float end, float duration)
    {
        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = target.AddComponent<CanvasGroup>();

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}