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

    void Start()
    {
        SetAlpha(0);
        gameObject.SetActive(false);
    }

    public void SetPaperCount(int amount)
    {
        currentPapers = amount;
        UpdateUI();
        ShowUI();
    }

    void UpdateUI()
    {
        paperText.text = "Mga Papel: " + currentPapers + "/" + totalPapers;
    }

    void ShowUI()
    {
        gameObject.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float t = 0;

        // Fade in
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0, 1, t / fadeDuration));
            yield return null;
        }

        SetAlpha(1);

        // Stay visible
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1, 0, t / fadeDuration));
            yield return null;
        }

        SetAlpha(0);
        gameObject.SetActive(false);
    }

    void SetAlpha(float alpha)
    {
        Color c = paperText.color;
        c.a = alpha;
        paperText.color = c;
    }
}