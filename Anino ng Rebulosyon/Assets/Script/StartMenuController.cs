using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public void OnMagsimula()
    {
        SceneManager.LoadScene("Binondo");
    }

    public GameObject MainMenuPanel;
    public GameObject EkstraPanel;
    public GameObject SettingsPanel;

    public void OpenMainMenu()
    {
        MainMenuPanel.SetActive(true);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }
    public void OpenEkstra()
    {
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
  }


