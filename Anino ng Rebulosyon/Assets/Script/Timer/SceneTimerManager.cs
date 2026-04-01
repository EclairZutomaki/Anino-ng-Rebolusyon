using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTimerManager : MonoBehaviour
{
    [Header("Timer")]
    public float startTime = 60f;   // starting time in seconds
    public bool timerRunning = true;

    [Header("UI")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;

    [Header("Return To Game")]
    public float returnToGameBonusTime = 10f; // time added if player chooses Return to Game

    [Header("Restart Game")]
    public string mainMenuSceneName = "MainMenu"; // change this to your menu/start scene name

    private float currentTime;
    private bool isGameOver = false;

    private void Start()
    {
        currentTime = startTime;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateTimerUI();
    }

    private void Update()
    {
        if (!timerRunning || isGameOver)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerUI();
            GameOver();
            return;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        isGameOver = true;
        timerRunning = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // BUTTON: Return to Game
    public void ReturnToGame()
    {
        currentTime = returnToGameBonusTime;
        isGameOver = false;
        timerRunning = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
        UpdateTimerUI();
    }

    // BUTTON: Restart current scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // BUTTON: Restart game / go to main menu
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Optional: add more time from other scripts
    public void AddTime(float amount)
    {
        currentTime += amount;
        UpdateTimerUI();
    }

    // Optional: set exact time from other scripts
    public void SetTime(float newTime)
    {
        currentTime = newTime;
        UpdateTimerUI();
    }
}