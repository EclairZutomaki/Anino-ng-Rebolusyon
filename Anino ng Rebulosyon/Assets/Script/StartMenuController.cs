using UnityEngine;
using UnityEngine.SceneManagement;

public class Kabanata : MonoBehaviour
{
    public GameObject[] Panels;

    private int currentPanelIndex = 0;

    private void Start()
    {
        // Ensure default panel is shown
        OpenPanel(currentPanelIndex);
    }

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

        SceneManager.LoadScene(data.sceneName);

        PlayerSpawner.LoadPosition = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]
        );
    }

    public void OpenPanel(int panelIndex)
    {
        currentPanelIndex = panelIndex;

        for (int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(i == currentPanelIndex);
        }
    }

    // NEXT (+1)
    public void NextPanel()
    {
        currentPanelIndex++;

        // Clamp so it doesn't go out of bounds
        if (currentPanelIndex >= Panels.Length)
            currentPanelIndex = Panels.Length - 1;

        OpenPanel(currentPanelIndex);
    }

    // PREVIOUS (-1)
    public void PreviousPanel()
    {
        currentPanelIndex--;

        if (currentPanelIndex < 0)
            currentPanelIndex = 0;

        OpenPanel(currentPanelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}