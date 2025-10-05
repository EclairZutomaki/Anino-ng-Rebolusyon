using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RunTutorialTrigger : MonoBehaviour
{
    [Tooltip("Reference to the TutorialUIManager in your scene.")]
    public TutorialUIManager tutorialManager;

    [Tooltip("Tag of the player that can activate this.")]
    public string playerTag = "Player";

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"{name}: Collider was not set as trigger, fixed automatically.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            tutorialManager?.TriggerRunUI();
        }
    }
}
