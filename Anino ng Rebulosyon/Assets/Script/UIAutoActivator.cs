using UnityEngine;

public class UIAutoActivator : MonoBehaviour
{
    [Header("STEP 1: Timeline activates this")]
    public GameObject tabTutorialUI;  // "Press TAB to open map"

    [Header("STEP 2: Map tutorial UI (hidden)")]
    public GameObject mapTutorialUI;  // tutorial inside map

    [Header("STEP 3: (Removed timer) - Instant click to continue UI")]
    public GameObject clickToContinueUI; // "Click anywhere to continue"

    [Header("Map Canvas (para kahit saan e-click gagana)")]
    public GameObject mapCanvas;

    private bool hasTriggeredMapTutorial = false;
    private bool waitingForClick = false;

    void OnEnable()
    {
        // Show first tutorial UI (Press TAB)
        if (tabTutorialUI != null)
            tabTutorialUI.SetActive(true);
    }

    void Update()
    {
        // TAB = show 2nd tutorial instantly
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TriggerMapTutorial();
        }

        // If we are waiting for click (instant)
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            EndAllTutorials();
        }
    }

    void TriggerMapTutorial()
    {
        if (hasTriggeredMapTutorial) return;

        hasTriggeredMapTutorial = true;

        // Activate map canvas
        if (mapCanvas != null)
            mapCanvas.SetActive(true);

        // Show STEP 2 tutorial
        if (mapTutorialUI != null)
            mapTutorialUI.SetActive(true);

        // Instant show “click to continue”
        if (clickToContinueUI != null)
            clickToContinueUI.SetActive(true);

        // Now it's ready for clicking instantly
        waitingForClick = true;
    }

    void EndAllTutorials()
    {
        waitingForClick = false;

        if (tabTutorialUI != null)
            tabTutorialUI.SetActive(false);

        if (mapTutorialUI != null)
            mapTutorialUI.SetActive(false);

        if (clickToContinueUI != null)
            clickToContinueUI.SetActive(false);

        // OPTIONAL: disable whole parent tutorial object
        gameObject.SetActive(false);
    }
}
