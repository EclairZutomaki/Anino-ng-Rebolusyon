using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("Binondo");
    }


    public GameObject[] Panels;

    public void OpenPanel(int panelIndex)
    {
        for (int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(i == panelIndex);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}