using UnityEngine;

public class PaperCollect : MonoBehaviour
{
    public string paperID;
    private bool playerNear = false;

    void Start()
    {
        // Check if already collected
        if (PlayerPrefs.GetInt(paperID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            CollectPaper();
        }
    }

    void CollectPaper()
    {
        PlayerPrefs.SetInt(paperID, 1);
        PlayerPrefs.Save();

        Debug.Log("Collected: " + paperID);

        gameObject.SetActive(false); // better than Destroy
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}