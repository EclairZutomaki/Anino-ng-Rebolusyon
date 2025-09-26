using UnityEngine;
using UnityEngine.UI; // Needed for Button

public class BagManager : MonoBehaviour
{
    [Header("Bag UI Panels")]
    public GameObject bagUI;            // Main Bag container
    public GameObject documentsPanel;   // Documents tab panel
    public GameObject mapPanel;         // Map tab panel
    public GameObject dictionaryPanel;  // Dictionary tab panel

    [Header("Tab Buttons")]
    public Button mapButton;
    public Button documentsButton;
    public Button dictionaryButton;

    private void Start()
    {
        // 🔒 Hide Bag & all panels at startup
        bagUI.SetActive(false);
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);
    }

    public void ShowBag()
    {
        bagUI.SetActive(true);
        ShowMap(); // ✅ Default to Map
    }

    public void HideBag()
    {
        bagUI.SetActive(false);

        // Optional: Reset panel states when closing
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);
    }

    public void ShowDocuments()
    {
        documentsPanel.SetActive(true);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);
        HighlightButton(documentsButton);
    }

    public void ShowMap()
    {
        documentsPanel.SetActive(false);
        mapPanel.SetActive(true);
        dictionaryPanel.SetActive(false);
        HighlightButton(mapButton);
    }

    public void ShowDictionary()
    {
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(true);
        HighlightButton(dictionaryButton);
    }

    private void HighlightButton(Button activeButton)
    {
        // 🎨 Reset all buttons to default color
        ResetButtonColors();

        // 🎨 Highlight the active button
        activeButton.image.color = Color.green;
    }

    private void ResetButtonColors()
    {
        mapButton.image.color = Color.white;
        documentsButton.image.color = Color.white;
        dictionaryButton.image.color = Color.white;
    }
}
