using UnityEngine;

public class PaperCollect : MonoBehaviour
{
    public string paperID;
    private bool playerNear = false;

    [Header("UI")]
    public PaperCollectorUI paperUI; // drag your UI panel here

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
        // Save this paper as collected
        PlayerPrefs.SetInt(paperID, 1);
        PlayerPrefs.Save();

        Debug.Log("Collected: " + paperID);

        // Count ALL collected papers
        int totalCollected = CountCollectedPapers();

        // Update UI
        if (paperUI != null)
        {
            paperUI.SetPaperCount(totalCollected);
        }

        gameObject.SetActive(false);
    }

    int CountCollectedPapers()
    {
        // Find ALL PaperCollect objects in scene
        PaperCollect[] allPapers = FindObjectsByType<PaperCollect>(FindObjectsSortMode.None);

        int count = 0;

        foreach (var paper in allPapers)
        {
            if (PlayerPrefs.GetInt(paper.paperID, 0) == 1)
                count++;
        }

        return count;
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