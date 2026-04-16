using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoadingUI : MonoBehaviour
{
    public static SceneLoadingUI Instance;

    [Header("UI References")]
    public GameObject loadingPanel;
    public Image backgroundImage;
    public Slider progressSlider;
    public TMP_Text progressText;

    [Header("Optional")]
    [Tooltip("Minimum time the loading screen stays visible.")]
    public float minimumLoadScreenTime = 0.5f;

    private bool isLoading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (progressSlider != null)
            progressSlider.value = 0f;

        if (progressText != null)
            progressText.text = "0%";
    }

    public void LoadScene(string sceneName, Sprite backgroundSprite = null)
    {
        if (isLoading || string.IsNullOrEmpty(sceneName))
            return;

        StartCoroutine(LoadSceneRoutine(sceneName, backgroundSprite));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, Sprite backgroundSprite)
    {
        isLoading = true;

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        if (backgroundImage != null && backgroundSprite != null)
            backgroundImage.sprite = backgroundSprite;

        if (progressSlider != null)
            progressSlider.value = 0f;

        if (progressText != null)
            progressText.text = "0%";

        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressSlider != null)
                progressSlider.value = progress;

            if (progressText != null)
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            if (operation.progress >= 0.9f && timer >= minimumLoadScreenTime)
            {
                if (progressSlider != null)
                    progressSlider.value = 1f;

                if (progressText != null)
                    progressText.text = "100%";

                yield return new WaitForSecondsRealtime(0.15f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoading = false;
    }
}