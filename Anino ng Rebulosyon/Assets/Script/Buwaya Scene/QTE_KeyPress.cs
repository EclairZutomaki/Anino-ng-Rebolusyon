using UnityEngine;
using UnityEngine.Playables;

public class QTE_KeyPress : MonoBehaviour
{
    public KeyCode key;
    public PlayableDirector previousTimeline;
    public PlayableDirector successTimeline;
    public PlayableDirector failTimeline;

    public float windowTime = 1f;

    float timer;
    bool active = false;

    void OnEnable()
    {
        timer = windowTime;
        active = true;
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;

        // SUCCESS
        if (Input.GetKeyDown(key))
        {
            active = false;
            previousTimeline.Stop();
            failTimeline.Stop(); // just in case
            successTimeline.Play();
            gameObject.SetActive(false);
        }

        // FAIL
        else if (timer <= 0)
        {
            active = false;
            previousTimeline.Stop();
            successTimeline.Stop(); // just in case
            failTimeline.Play();
            gameObject.SetActive(false);
        }
    }
}
