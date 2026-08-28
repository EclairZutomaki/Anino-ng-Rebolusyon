using UnityEngine;

public class PaperCollect : MonoBehaviour
{
    [Header("Paper Settings")]
    public string paperID;

    [Header("UI")]
    public PaperCollectorUI paperUI;

    [Header("Interaction UI")]
    public GameObject interactUIPanel;

    private bool playerNear = false;

    private const string PaperCountKey = "CollectedPaperCount";

    private void Start()
    {
        if (string.IsNullOrEmpty(paperID))
        {
            Debug.LogError("PaperCollect: Missing paperID on " + gameObject.name);
            return;
        }

        // Hide paper if already collected
        if (PlayerPrefs.GetInt(paperID, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (paperUI == null)
        {
            Debug.LogWarning("PaperCollect: paperUI is not assigned on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            CollectPaper();
        }
    }

    private void CollectPaper()
    {
        // Prevent duplicate collection
        if (PlayerPrefs.GetInt(paperID, 0) == 1)
        {
            Debug.Log("Paper already collected: " + paperID);
            return;
        }

        // Hide the interaction UI when the paper is collected
        if (interactUIPanel != null)
        {
            interactUIPanel.SetActive(false);
        }

        // Save this paper as collected
        PlayerPrefs.SetInt(paperID, 1);

        // Increase total paper count
        int currentCount = PlayerPrefs.GetInt(PaperCountKey, 0);
        currentCount++;
        PlayerPrefs.SetInt(PaperCountKey, currentCount);

        PlayerPrefs.Save();

        Debug.Log("Collected paper: " + paperID);
        Debug.Log("Total collected papers: " + currentCount);

        // Update UI
        if (paperUI != null)
        {
            paperUI.SetPaperCount(currentCount);
        }

        // Hide collected paper
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player near paper: " + paperID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}