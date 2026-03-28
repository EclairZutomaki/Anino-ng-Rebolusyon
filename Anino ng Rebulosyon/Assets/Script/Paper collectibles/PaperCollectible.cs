using UnityEngine;

public class PaperCollectible : MonoBehaviour
{
    public string paperID;
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerInside = false;

    void Start()
    {
        // Auto hide if already collected
        if (PlayerPrefs.GetInt(paperID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(interactKey))
        {
            Collect();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void Collect()
    {
        PlayerPrefs.SetInt(paperID, 1);
        PlayerPrefs.Save();

        Debug.Log("Collected: " + paperID);

        gameObject.SetActive(false);
    }
}