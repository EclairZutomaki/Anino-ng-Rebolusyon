using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    public float timer = 5f; // Time in seconds before changing scene
    public string Terrain; // Name of the scene to load

    void Update()
    {
        // Reduce the timer by the time passed since last frame
        timer -= Time.deltaTime;

        // When the timer reaches 0, load the next scene
        if (timer <= 0f)
        {
            SceneManager.LoadScene(Terrain);
        }
    }
}