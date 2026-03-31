using UnityEngine;
using UnityEngine.UI;

public class UICounter : MonoBehaviour
{
    public int totalNPCs = 5;

    private int currentCount = 0;
    private Text uiText;

    void Awake()
    {
        uiText = GetComponent<Text>();
        UpdateUI();
    }

    public void AddCount()
    {
        if (currentCount >= totalNPCs) return;

        currentCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        uiText.text = currentCount + "/" + totalNPCs;
    }
}