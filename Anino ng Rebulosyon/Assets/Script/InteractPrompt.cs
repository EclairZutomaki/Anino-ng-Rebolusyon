using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [Header("Prompt Settings")]
    [Tooltip("Drag your UI Canvas or 'Press E to Interact' text here.")]
    public GameObject promptUI;

    [Tooltip("Set how close the player needs to be to show the prompt.")]
    public float activationDistance = 3f;

    [Tooltip("Tag of the player object.")]
    public string playerTag = "Player";

    private Transform player;
    private bool isPlayerNearby = false;

    private void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        // Hide the prompt by default
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (player == null || promptUI == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Show or hide based on distance
        if (distance <= activationDistance && !isPlayerNearby)
        {
            promptUI.SetActive(true);
            isPlayerNearby = true;
        }
        else if (distance > activationDistance && isPlayerNearby)
        {
            promptUI.SetActive(false);
            isPlayerNearby = false;
        }
    }

    private void OnDisable()
    {
        // Hide when object is disabled
        if (promptUI != null)
            promptUI.SetActive(false);
    }
}
