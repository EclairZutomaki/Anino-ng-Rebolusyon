using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;                      // Black fade UI panel
    [Tooltip("Duration of screen fade-in at game start.")]
    public float fadeDuration = 1f;
    [Tooltip("Delay before fade starts when game loads.")]
    public float startDelay = 0.5f;

    [Header("Mouse Tutorial Settings")]
    public GameObject mouseTutorialUI;           // Assign Mouse tutorial UI
    [Tooltip("Delay before mouse tutorial appears (after fade).")]
    public float mouseTutorialDelay = 0.5f;
    [Tooltip("How long the mouse tutorial stays visible.")]
    public float mouseTutorialDuration = 5f;

    [Header("Movement Tutorial Settings")]
    public GameObject movementTutorialUI;        // Assign Movement tutorial UI
    [Tooltip("Delay before movement tutorial appears (after mouse tutorial ends).")]
    public float movementTutorialDelay = 0.5f;
    [Tooltip("How long the movement tutorial stays visible.")]
    public float movementTutorialDuration = 5f;

    [Header("UI Fade Settings")]
    [Tooltip("Duration for tutorial UI fade in/out.")]
    public float uiFadeDuration = 0.5f;

    [Header("Debug")]
    public bool debugLogs = false;

    private void Start()
    {
        // Set initial states
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f; // Start fully black
            fadeImage.color = c;
        }

        if (mouseTutorialUI) mouseTutorialUI.SetActive(false);
        if (movementTutorialUI) movementTutorialUI.SetActive(false);

        // Start sequence
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(startDelay);

        // Fade screen from black
        if (fadeImage) yield return StartCoroutine(FadeImage(fadeImage, 1f, 0f, fadeDuration));

        // Wait then show mouse tutorial
        yield return new WaitForSeconds(mouseTutorialDelay);
        if (mouseTutorialUI)
        {
            if (debugLogs) Debug.Log("Showing Mouse Tutorial");
            yield return StartCoroutine(FadeCanvas(mouseTutorialUI, true));
            yield return new WaitForSeconds(mouseTutorialDuration);
            yield return StartCoroutine(FadeCanvas(mouseTutorialUI, false));
        }

        // Wait then show movement tutorial
        yield return new WaitForSeconds(movementTutorialDelay);
        if (movementTutorialUI)
        {
            if (debugLogs) Debug.Log("Showing Movement Tutorial");
            yield return StartCoroutine(FadeCanvas(movementTutorialUI, true));
            yield return new WaitForSeconds(movementTutorialDuration);
            yield return StartCoroutine(FadeCanvas(movementTutorialUI, false));
        }

        if (debugLogs) Debug.Log("Tutorial sequence finished!");
    }

    // Smooth fade for black screen
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

    // Smooth fade for tutorial UI (using CanvasGroup)
    private IEnumerator FadeCanvas(GameObject ui, bool fadeIn)
    {
        if (!ui) yield break;

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