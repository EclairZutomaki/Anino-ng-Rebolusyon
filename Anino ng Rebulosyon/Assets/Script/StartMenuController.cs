using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public void OnGameStart()
    {
        SceneManager.LoadScene("Binondo");
    }

    public GameObject MainMenuPanel;
    public GameObject EkstraPanel;
    public GameObject SettingsPanel;
    public GameObject SimulaPanel;


    public void OpenMainMenu()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }
    public void OpenSimula()
    {
        SimulaPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void OpenEkstra()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
  }


