using UnityEngine;
using UnityEngine.Playables;

public class CrocKillTrigger : MonoBehaviour
{
    public PlayableDirector failTimeline; // yung Timeline ng nalapa scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the croc!");
            failTimeline.Play();
        }
    }
}
