using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{
    [Header("UI PANELS")]
    public GameObject bagUI;
    public GameObject documentsPanel;
    public GameObject mapPanel;
    public GameObject dictionaryPanel;

    [Header("Tab Buttons")]
    public Button mapButton;
    public Button documentsButton;
    public Button dictionaryButton;

    [Header("Map Icons")]
    public GameObject mainQuestIcon;
    public GameObject sideQuestIcon;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.green;

    // --------------------------
    // NEW: Document Page System
    // --------------------------
    [Header("Document Pages")]
    public GameObject[] documentImages;   // assign your document images/pages here
    public Button nextButton;             // NEXT button
    public Button previousButton;         // PREV button

    private int currentIndex = 0;
    // --------------------------

    private void Start()
    {
        bagUI.SetActive(false);
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);

        if (mainQuestIcon) mainQuestIcon.SetActive(false);
        if (sideQuestIcon) sideQuestIcon.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousPage);

        UpdateDocumentPages();
    }

    public void ShowBag()
    {
        bagUI.SetActive(true);
        ShowMap();
    }

    public void HideBag()
    {
        bagUI.SetActive(false);

        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);

        ToggleMapIcons(false);
    }

    public void ShowDocuments()
    {
        documentsPanel.SetActive(true);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);

        HighlightButton(documentsButton);
        ToggleMapIcons(false);

        currentIndex = 0;
        UpdateDocumentPages();
    }

    public void ShowMap()
    {
        documentsPanel.SetActive(false);
        mapPanel.SetActive(true);
        dictionaryPanel.SetActive(false);

        HighlightButton(mapButton);
        ToggleMapIcons(true);
    }

    public void ShowDictionary()
    {
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(true);

        HighlightButton(dictionaryButton);
        ToggleMapIcons(false);
    }

    private void HighlightButton(Button active)
    {
        ResetButtonColors();
        active.image.color = highlightColor;
    }

    private void ResetButtonColors()
    {
        mapButton.image.color = Color.white;
        documentsButton.image.color = Color.white;
        dictionaryButton.image.color = Color.white;
    }

    private void ToggleMapIcons(bool state)
    {
        if (mainQuestIcon) mainQuestIcon.SetActive(state);
        if (sideQuestIcon) sideQuestIcon.SetActive(state);
    }

    // --------------------------
    // PAGE SWITCHING FUNCTIONS
    // --------------------------
    private void UpdateDocumentPages()
    {
        if (documentImages == null || documentImages.Length == 0) return;

        for (int i = 0; i < documentImages.Length; i++)
            documentImages[i].SetActive(i == currentIndex);
    }

    public void NextPage()
    {
        if (documentImages == null || documentImages.Length == 0) return;

        currentIndex++;

        if (currentIndex >= documentImages.Length)
            currentIndex = documentImages.Length - 1; // stops at last page

        UpdateDocumentPages();
    }

    public void PreviousPage()
    {
        if (documentImages == null || documentImages.Length == 0) return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = 0; // stops at first page

        UpdateDocumentPages();
    }
}
