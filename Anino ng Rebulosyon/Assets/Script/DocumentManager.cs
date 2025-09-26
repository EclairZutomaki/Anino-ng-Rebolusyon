using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DocumentManager : MonoBehaviour
{
    [System.Serializable]
    public class PaperEntry
    {
        public string paperID;         // Unique ID for each paper
        public string content;         // The text that will appear once collected
        public GameObject textObject;  // The UI Text (starts hidden)
    }

    [Header("All Possible Papers")]
    public List<PaperEntry> papers = new List<PaperEntry>();

    private HashSet<string> collectedPapers = new HashSet<string>();

    public static DocumentManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide all text objects at start
        foreach (var paper in papers)
        {
            if (paper.textObject != null)
                paper.textObject.SetActive(false);
        }
    }

    public void CollectPaper(string id)
    {
        // Add to collected list
        if (!collectedPapers.Contains(id))
        {
            collectedPapers.Add(id);

            // Reveal the paper in UI
            var paper = papers.Find(p => p.paperID == id);
            if (paper != null && paper.textObject != null)
            {
                paper.textObject.SetActive(true);

                // If you want to update text content dynamically
                var textComp = paper.textObject.GetComponent<TMPro.TextMeshProUGUI>();
                if (textComp != null)
                {
                    textComp.text = paper.content;
                }
            }

            Debug.Log($"Paper {id} collected!");
        }
    }

    public bool IsCollected(string id)
    {
        return collectedPapers.Contains(id);
    }
}
