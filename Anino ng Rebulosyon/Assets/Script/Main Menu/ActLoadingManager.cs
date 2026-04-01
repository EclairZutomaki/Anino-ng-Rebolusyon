using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ActLoadingManager : MonoBehaviour
{
    [System.Serializable]
    public class ActLoadingData
    {
        public string actName;     // 👉 dito mo ilalagay "YUGTO I: ..."
        public string sceneName;
        public Sprite loadingSprite;
    }

    [Header("Loading UI")]
    public GameObject loadingPanel;
    public Image loadingImage;
    public Slider progressSlider;
    public TMP_Text progressText;
    public TMP_Text actTitleText; // 🔥 NEW (YUGTO NAME)

    [Header("Acts")]
    public ActLoadingData act1;
    public ActLoadingData act2;
    public ActLoadingData act3;

    private bool isLoading = false;

    private void Start()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (progressSlider != null)
            progressSlider.value = 0f;

        if (progressText != null)
            progressText.text = "0%";

        if (actTitleText != null)
            actTitleText.text = "";
    }

    public void LoadAct1()
    {
        if (!isLoading)
            StartCoroutine(LoadSceneRoutine(act1));
    }

    public void LoadAct2()
    {
        if (!isLoading)
            StartCoroutine(LoadSceneRoutine(act2));
    }

    public void LoadAct3()
    {
        if (!isLoading)
            StartCoroutine(LoadSceneRoutine(act3));
    }

    private IEnumerator LoadSceneRoutine(ActLoadingData actData)
    {
        isLoading = true;

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // 🔥 SET LOADING IMAGE
        if (loadingImage != null && actData.loadingSprite != null)
            loadingImage.sprite = actData.loadingSprite;

        // 🔥 SET ACT TITLE (ITO NA YUNG YUGTO TEXT MO)
        if (actTitleText != null)
            actTitleText.text = actData.actName;

        if (progressSlider != null)
            progressSlider.value = 0f;

        if (progressText != null)
            progressText.text = "0%";

        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(actData.sceneName);
        operation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime * 0.5f);

            if (progressSlider != null)
                progressSlider.value = displayedProgress;

            if (progressText != null)
                progressText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

            if (displayedProgress >= 0.99f && operation.progress >= 0.9f)
            {
                if (progressSlider != null)
                    progressSlider.value = 1f;

                if (progressText != null)
                    progressText.text = "100%";

                yield return new WaitForSeconds(0.3f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}