using UnityEngine;

public class DialogueToggleUI : MonoBehaviour
{
    public GameObject dialogueCanvas;

    private bool isHidden = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHidden = !isHidden;
            dialogueCanvas.SetActive(!isHidden);
        }
    }
}