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
    public Color highlightColor = Color.green; // 🎨 Color wheel freedom

    private void Start()
    {
        bagUI.SetActive(false);
        documentsPanel.SetActive(false);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);

        if (mainQuestIcon) mainQuestIcon.SetActive(false);
        if (sideQuestIcon) sideQuestIcon.SetActive(false);
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

        if (mainQuestIcon) mainQuestIcon.SetActive(false);
        if (sideQuestIcon) sideQuestIcon.SetActive(false);
    }

    public void ShowDocuments()
    {
        documentsPanel.SetActive(true);
        mapPanel.SetActive(false);
        dictionaryPanel.SetActive(false);

        HighlightButton(documentsButton);
        ToggleMapIcons(false);
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

    private void HighlightButton(Button activeButton)
    {
        ResetButtonColors();
        activeButton.image.color = highlightColor;
    }

    private void ResetButtonColors()
    {
        mapButton.image.color = Color.white;
        documentsButton.image.color = Color.white;
        dictionaryButton.image.color = Color.white;
    }

    private void ToggleMapIcons(bool show)
    {
        if (mainQuestIcon) mainQuestIcon.SetActive(show);
        if (sideQuestIcon) sideQuestIcon.SetActive(show);
    }
}
