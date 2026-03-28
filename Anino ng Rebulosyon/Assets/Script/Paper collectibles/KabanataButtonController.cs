using UnityEngine;

public class KabanataButtonController : MonoBehaviour
{
    [System.Serializable]
    public class ButtonPair
    {
        public string paperID;

        public GameObject lockedButton;
        public GameObject unlockedButton;
    }

    public ButtonPair[] buttons;

    void OnEnable()
    {
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        foreach (ButtonPair pair in buttons)
        {
            // SAFETY CHECK
            if (pair == null || pair.lockedButton == null || pair.unlockedButton == null)
            {
                Debug.LogWarning("Missing button reference in KabanataButtonController!", this);
                continue;
            }

            if (PlayerPrefs.GetInt(pair.paperID, 0) == 1)
            {
                pair.lockedButton.SetActive(false);
                pair.unlockedButton.SetActive(true);
            }
            else
            {
                pair.lockedButton.SetActive(true);
                pair.unlockedButton.SetActive(false);
            }
        }
    }
}