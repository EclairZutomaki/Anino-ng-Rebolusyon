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
    public GameObject DictionaryPanel;
    public GameObject ChapterPaperPanel;



    public void OpenMainMenu()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        DictionaryPanel.SetActive(false);
        ChapterPaperPanel.SetActive(false);
    }
    public void OpenSimula()
    {
        SimulaPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        DictionaryPanel.SetActive(false);
        ChapterPaperPanel.SetActive(false);
    }

    public void OpenEkstra()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        DictionaryPanel.SetActive(false);
        ChapterPaperPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OpenDictionary()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        DictionaryPanel.SetActive(true);
        ChapterPaperPanel.SetActive(false);
    }

    public void OpenChapterPaper()
    {
        SimulaPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        EkstraPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        DictionaryPanel.SetActive(false);
        ChapterPaperPanel.SetActive(true);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
  }


