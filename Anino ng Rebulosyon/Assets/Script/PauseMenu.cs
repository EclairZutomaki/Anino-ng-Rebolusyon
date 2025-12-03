using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;

    private bool isPaused = false;

    void Start()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;

        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Debug.Log("Game Resumed");
    }

    // Button hook
    public void ResumeFromButton()
    {
        ResumeGame();
        isPaused = false;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
