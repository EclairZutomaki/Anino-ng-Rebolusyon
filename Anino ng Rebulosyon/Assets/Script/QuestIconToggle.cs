using UnityEngine;
using System.Collections.Generic;

public class QuestIconToggle : MonoBehaviour
{
    [Header("Scale Settings")]
    public float selectedScale = 1.3f;
    public float normalScale = 1f;

    [Header("Other Icon Reference")]
    public QuestIconToggle otherIcon; // 👈 assign the opposite quest icon

    [Header("Objects to Show/Hide")]
    public List<GameObject> objectsToShow = new List<GameObject>(); // appear when selected
    public List<GameObject> objectsToHide = new List<GameObject>(); // disappear when selected

    private bool isSelected = false;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.localScale = Vector3.one * normalScale;
    }

    public void OnClick()
    {
        // Toggle selection state
        if (isSelected)
        {
            Deselect();
            return;
        }

        // Select this one
        isSelected = true;
        rect.localScale = Vector3.one * selectedScale;

        // Deselect other icon if assigned
        if (otherIcon != null)
            otherIcon.Deselect();

        // Show all in show list
        foreach (var obj in objectsToShow)
            if (obj != null) obj.SetActive(true);

        // Hide all in hide list
        foreach (var obj in objectsToHide)
            if (obj != null) obj.SetActive(false);

        Debug.Log($"{gameObject.name} selected!");
    }

    public void Deselect()
    {
        isSelected = false;
        rect.localScale = Vector3.one * normalScale;

        // When deselected → revert all visibility changes
        foreach (var obj in objectsToShow)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in objectsToHide)
            if (obj != null) obj.SetActive(true);

        Debug.Log($"{gameObject.name} deselected!");
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
