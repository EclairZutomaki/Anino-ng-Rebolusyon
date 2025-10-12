using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float startDelay = 0.5f;

    [Header("Tutorial UIs")]
    public GameObject mouseTutorialUI;
    public float mouseTutorialDelay = 0.5f;
    public float mouseTutorialDuration = 5f;

    public GameObject movementTutorialUI;
    public float movementTutorialDelay = 0.5f;
    public float movementTutorialDuration = 5f;

    [Header("Extra Tutorials")]
    public GameObject runTutorialUI; // 🏃‍♂️ SHIFT to run
    public float runTutorialDuration = 5f;

    [Header("UI Fade Settings")]
    public float uiFadeDuration = 0.5f;

    [Header("Debug")]
    public bool debugLogs = false;

    private void Start()
    {
        // Reset visuals
        if (fadeImage)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        HideAllTutorials();
        StartCoroutine(TutorialSequence());
    }

    private void HideAllTutorials()
    {
        if (mouseTutorialUI) mouseTutorialUI.SetActive(false);
        if (movementTutorialUI) movementTutorialUI.SetActive(false);
        if (runTutorialUI) runTutorialUI.SetActive(false);
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(startDelay);

        // Fade in from black
        if (fadeImage)
            yield return StartCoroutine(FadeImage(fadeImage, 1f, 0f, fadeDuration));

        // Mouse tutorial
        yield return new WaitForSeconds(mouseTutorialDelay);
        yield return StartCoroutine(ShowTutorial(mouseTutorialUI, mouseTutorialDuration));

        // Movement tutorial
        yield return new WaitForSeconds(movementTutorialDelay);
        yield return StartCoroutine(ShowTutorial(movementTutorialUI, movementTutorialDuration));
    }

    // 🧠 Generic reusable function to show any tutorial UI
    public IEnumerator ShowTutorial(GameObject ui, float duration)
    {
        if (ui == null) yield break;

        if (debugLogs) Debug.Log($"Showing tutorial: {ui.name}");

        yield return StartCoroutine(FadeCanvas(ui, true));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeCanvas(ui, false));
    }

    // 👇 This is for triggering tutorials anytime (like after subtitles)
    public void TriggerRunTutorial()
    {
        if (runTutorialUI != null)
            StartCoroutine(ShowTutorial(runTutorialUI, runTutorialDuration));
    }

    // Smooth fade for screen image
    private IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = img.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            img.color = c;
            yield return null;
        }

        c.a = endAlpha;
        img.color = c;
    }

    // Smooth fade for any tutorial UI
    private IEnumerator FadeCanvas(GameObject ui, bool fadeIn)
    {
        CanvasGroup cg = ui.GetComponent<CanvasGroup>();
        if (!cg) cg = ui.AddComponent<CanvasGroup>();

        ui.SetActive(true);
        float start = fadeIn ? 0f : 1f;
        float end = fadeIn ? 1f : 0f;
        float elapsed = 0f;

        while (elapsed < uiFadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / uiFadeDuration);
            yield return null;
        }

        cg.alpha = end;

        if (!fadeIn)
            ui.SetActive(false);
    }
}
