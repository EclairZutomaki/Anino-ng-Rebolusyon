using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyToChangeScene : MonoBehaviour
{
    public string nextSceneName = "Cutscene"; // Set your target scene name here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Press L to trigger
        {
            Debug.Log("Pressed L! Loading scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
