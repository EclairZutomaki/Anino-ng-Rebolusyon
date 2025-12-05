using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("Binondo");
    }

    public void LoadGame()
    {
        PlayerData data = SavingSystem.LoadPlayer();

        if (data == null)
        {
            Debug.Log("No save file found!");
            return;
        }

        // Load saved scene
        SceneManager.LoadScene(data.sceneName);

        // After scene loads, we need to spawn the player in saved position
        PlayerSpawner.LoadPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
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
