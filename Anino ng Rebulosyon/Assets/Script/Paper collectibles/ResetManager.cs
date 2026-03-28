using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [Header("Optional: Reload Scene After Reset")]
    public bool reloadScene = true;

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("ALL DATA RESET");

        if (reloadScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Optional debug key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAllData();
        }
    }
}