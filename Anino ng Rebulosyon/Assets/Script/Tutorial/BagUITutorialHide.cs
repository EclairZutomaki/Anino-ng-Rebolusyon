using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class BagUITutorialController : MonoBehaviour
{
    [Header("Tutorial UI To Hide")]
    public GameObject bagUITutorial;

    void Update()
    {
        if (bagUITutorial == null) return;

        bool pressedE = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            pressedE = true;
#else
        if (Input.GetKeyDown(KeyCode.E))
            pressedE = true;
#endif

        if (pressedE && bagUITutorial.activeSelf)
        {
            bagUITutorial.SetActive(false);
        }
    }
}