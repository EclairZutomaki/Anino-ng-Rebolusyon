using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaperCollectorUI : MonoBehaviour
{
    [Header("UI")]
    public Text paperText;

    [Header("Settings")]
    public int totalPapers = 10;
    public float displayTime = 2f;
    public float fadeDuration = 1f;

    private int currentPapers = 0;
    private Coroutine fadeCoroutine;

    private const string PaperCountKey = "CollectedPaperCount";

    private void Start()
    {
        if (paperText == null)
        {
            Debug.LogError("PaperCollectorUI: paperText is not assigned!");
            return;
        }

        currentPapers = PlayerPrefs.GetInt(PaperCountKey, 0);
        UpdateUI();

        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    public void SetPaperCount(int amount)
    {
        currentPapers = Mathf.Clamp(amount, 0, totalPapers);
        UpdateUI();
        ShowUI();
    }

    public void RefreshFromSave()
    {
        currentPapers = PlayerPrefs.GetInt(PaperCountKey, 0);
        currentPapers = Mathf.Clamp(currentPapers, 0, totalPapers);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (paperText != null)
        {
            paperText.text = "Mga Papel: " + currentPapers + "/" + totalPapers;
        }
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float t = 0f;

        // Fade In
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        SetAlpha(1f);

        // Stay visible
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        if (paperText == null) return;

        Color c = paperText.color;
        c.a = alpha;
        paperText.color = c;
    }
}