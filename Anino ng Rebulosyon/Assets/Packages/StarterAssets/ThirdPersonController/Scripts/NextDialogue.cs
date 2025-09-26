using StarterAssets;
using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    int index = 2;

    private void Update()
    {
        if(Input.GetMouseButton(0) && transform.childCount > 1)
        {
            if (ThirdPersonController.dialogue)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                index += 1;
                if(transform.childCount == index)
                {
                    index = 2;
                    ThirdPersonController.dialogue = false;
                }
            }
            else
            {
                gameObject.SetActive(false);
                index = 2; // Reset index for next interaction
            }
        }
    }
}