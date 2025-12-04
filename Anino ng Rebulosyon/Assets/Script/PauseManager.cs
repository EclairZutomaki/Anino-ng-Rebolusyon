using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI PANELS")]
    public GameObject pauseUI;

    [Header("Buttons")]
    public Button returnButton;
    public Button saveButton;
    public Button quitButton;

    private void Start()
    {
        pauseUI.SetActive(false);

        returnButton.onClick.AddListener(ReturnGame);
        saveButton.onClick.AddListener(SaveGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    public void ShowPause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HidePause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ReturnGame()
    {
        StarterAssets.ThirdPersonController.isSettingsOpen = false;
        HidePause();
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void SaveGame()
    {
        Debug.Log("Save feature coming soon...");
    }
}
