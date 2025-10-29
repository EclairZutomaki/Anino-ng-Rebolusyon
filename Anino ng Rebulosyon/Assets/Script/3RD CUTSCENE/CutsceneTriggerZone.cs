using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggerZone : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector cutscene; // Drag your 3rd cutscene Timeline here

    [Header("Cutscene Settings")]
    [Tooltip("If checked, the cutscene will only play once.")]
    public bool playOnce = true; // ✅ toggle in Inspector

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playOnce && hasPlayed)
        {
            Debug.Log("Cutscene already played once — skipping.");
            return;
        }

        hasPlayed = true;
        cutscene.Play();
        Debug.Log("3rd cutscene triggered!");
    }
}
